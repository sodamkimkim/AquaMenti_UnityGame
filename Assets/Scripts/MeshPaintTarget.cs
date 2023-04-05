using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintTarget : MonoBehaviour
{
    // ���� �߿����� RenderTexture �״�� ����� �ϰ� Save�� �õ��� �� RenderTexture�� Pixel�� �����ؼ� ���纻�� �����ϴ� ������ ����.
    // ������ ���� Save�� �� �� ����� �Ǵ� ��� RenderTexture�� ���� Pixel�� �����ؼ� ���纻�� �����ϴ� �۾��� ������ �� ����.
    // ��> �� �κ��� ComputeShader�� ���ؼ� ���纻���� Pixel ���ϼ��� ������ �ϰ� �ٸ��ٸ� ������ �����ϰ� �ϸ� ������ ������ ����.
    // ��> ComputeShader�� ����Ѵٰ� ���� �� �������� ������ �������� �� �ٷ� �۾��� �ߴ��ϰ� ���������� �ϴ� ����� ������?

    [SerializeField]
    private ComputeShader paintShader = null;
    [SerializeField]
    private ComputeShader countShader = null;

    private MeshRenderer mr = null; // ����� MeshRenderer
    private Texture originTex = null; // UV����
    private RenderTexture targetTexture = null; // ���� Mask
    [SerializeField]
    private int resolution = 512;
    private int depth = 32;

    [SerializeField]
    private bool drawable = false;
    [SerializeField]
    private bool isClear = false;
    [SerializeField, Range(50, 100)]
    private int clearPercent = 90;

    // Compute Shader
    private int kernelNoise;
    private int kernelPaint;
    private int kernelCopy;
    private int kernelClear;

    private int kernelInitCount;
    private int kernelCount;

    private readonly string kernelNoiseName = "CSNoise";
    private readonly string kernelPaintName = "CSPaint";
    private readonly string kernelCopyName = "CSCopy";
    private readonly string kernelClearName = "CSClear";

    private readonly string kernelInitCountName = "CSInitCount";
    private readonly string kernelCountName = "CSCount";

    private int threadGroupX;
    private int threadGroupY;

    private readonly string timerName = "_Timer";
    private readonly string twinkleSpeed = "_TwinkleSpeed";
    private bool isCompleteTwinkle { get; set; }
    private bool isDirtyTwinkle { get; set; }


    #region Properties Getter/Setter
    public bool IsDrawable()
    {
        return drawable;
    }
    public void IsDrawable(bool _able)
    {
        drawable = _able;
    }

    public bool IsClear()
    {
        return isClear;
    }
    public void IsClear(bool _clear)
    {
        isClear = _clear;
    }
    #endregion


    private void Awake()
    {
        // MeshRenderer�ϼ��� SkinnedMeshRenderer�ϼ��� �����Ƿ� �׳� �޴� ������ Ÿ�Կ� ���ֹ޵��� ���׸��� ���� ����
        TryGetComponent(out mr);
    }

    private void Start()
    {
        if (drawable)
            Init();
    }

    private void Update()
    {
        // �׸� �� �ִ� ����̰� Ŭ��� ���� �ʾҴٸ� ���� ������ ������ �� �ֵ��� ��
        if (IsDrawable() && !IsClear() && Input.GetKeyDown(KeyCode.Tab))
        {
            DirtyTwinkle();
        }

        // SaveToPNG Test (���� ���̹� ���� �ʿ�)
        if (IsDrawable() && Input.GetKeyDown(KeyCode.Slash))
        {
            SaveToPNG(ToTexture(targetTexture));
        }
    }


    private void Init()
    {
        // _MainTex: �𵨸��� Texture
        // _PaintTex: ���� Texture
        // _PaintUv: ���� UV Texture
        // _PaintMask: ���� Mask (default: none). �ڵ�󿡼� �߰��� ����

        originTex = mr.material.GetTexture("_PaintUv");

        targetTexture = new RenderTexture(originTex.width, originTex.height, depth);
        targetTexture.name = originTex.name;
        targetTexture.enableRandomWrite = true; // Graphics.Blit�� �ϱ� ���� ������ �� �ְ� ��������� �����
        Graphics.Blit(originTex, targetTexture); // Texture�� RenderTexture�� ����

        threadGroupX = Mathf.CeilToInt(targetTexture.width / 8);
        threadGroupY = Mathf.CeilToInt(targetTexture.height / 8);

        // !!���� ������ �ؽ��Ĵ� ������ ������ ���̴����� ����ϰ� ������ ����!!
        // *Compute Shader���� �����ϴµ� �ڿ������� �̾Ƴ��� �������� Shader Graph�� ����� ���
        SetNoiseTexture();
        SetBasicTwinkleProperties();

        mr.material.SetTexture("_PaintMask", targetTexture);
    }


    // RenderTexture�� Paint Rendering�� ��
    public void DrawRender(bool _drawable, Vector2 _uvPos, Color _color, float _size, float _distance/*, Texture2D _texture*/)
    {
        if (IsDrawable() == false || IsClear() == true) return;
        // Brush�� Texture�� �޾Ƽ� ����ϰ��� �Ͽ����� ������ �߻��Ͽ� ������ ������� ����
#if UNITY_EDITOR
        Debug.Log("DrawRender");
#endif
        /*
            RWTexture2D<float4> PaintMask;
            Texture2D<float4> BrushTex;
            float2 UvPos;
            float4 Color;
            float Size;
            float Distance;
            bool Drawable;
        */

        // 1) Kernel�� ������
        kernelPaint = paintShader.FindKernel(kernelPaintName);

        // 2) �ʱ�ȭ�� �ʿ��� ��� ���⼭ �ʱ�ȭ
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        // uvPos�� ȭ��󿡼��� �����̹Ƿ� �ػ� ���� ����
        Vector2 uvPos = new Vector2((uint)Mathf.CeilToInt(_uvPos.x * resolution), (uint)Mathf.CeilToInt(_uvPos.y * resolution));

        // 3) ������ �����ƴٸ� shader�� �ѱ�
        paintShader.SetTexture(kernelPaint, "Result", targetTexture); // Target�� RenderTexture
        //shader.SetTexture(kernelPaint, "BrushTex", _texture); // Brush�� Texture
        paintShader.SetVector("UvPos", uvPos);
        paintShader.SetVector("Color", _color); // Brush�� ����
        paintShader.SetFloat("Size", _size); // Brush�� ������
        paintShader.SetFloat("Distance", _distance); // �ִ��Ÿ� / �浹�Ÿ�
        paintShader.SetBool("Paintable", _drawable); // �׸� �� �ִ��� ����

        // 4) �ʿ��� ���� �� �Ѱ�ٸ� shader ����
        // ���� Shader�� numthreads(8, 8, 1)�̸� shader.Dispatch(kernel, width / 8, height / 8, 1);
        paintShader.Dispatch(kernelPaint, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Shader Dispatch");
#endif

        // 5) ó���� ������ �����ϴ� �κ�
        // Buffer�� ���¾��ٸ� Data�� �������� Release �� null ó��
        Graphics.Blit(targetTexture, targetTexture);

        // 6) ��ȯ�� ���� �ִٸ� �Լ��� ��ȯ���� ���� �� ��ȯ
        // return;
    }


    public void SetNoiseTexture()
    {
        RenderTexture rTex = new RenderTexture(resolution, resolution, depth);
        rTex.enableRandomWrite = true;
        rTex.Create();

        kernelNoise = paintShader.FindKernel(kernelNoiseName);

        paintShader.SetTexture(kernelNoise, "Result", rTex);

        paintShader.Dispatch(kernelNoise, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Make Noise Texture");
#endif
        Graphics.Blit(rTex, rTex);
        mr.material.SetTexture("_PaintTex", rTex);
    }


    #region Pixel Counter
    public void CheckAutoClear()
    {
        if (IsClear() == true) return;

        if (clearPercent < GetProcessPercent())
        {
            ClearTexture();
            CompleteTwinkle();
        }
    }
    public float GetProcessPercent()
    {
        uint origin = PixelCount(originTex);
        uint target = PixelCount(targetTexture);

        float percent = (origin - target) / (float)origin * 100;
#if UNITY_EDITOR
        Debug.LogFormat("[CheckPercent] origin: {0}, target: {1}, percent: {2}", origin, target, percent);
#endif

        return percent;
    }

    private uint PixelCount(Texture _tex)
    {
        // 1) Kernel�� ������
        kernelInitCount = countShader.FindKernel(kernelInitCountName);
        kernelCount = countShader.FindKernel(kernelCountName);

        // 2) �ʱ�ȭ�� �ʿ��� ��� ���⼭ �ʱ�ȭ
        // ex) computeBuffer = new ComputeBuffer[count, sizeof(typeof) * cnt]; (uint4) => cnt: 4
        ComputeBuffer buffer = new ComputeBuffer(1, sizeof(uint) * 1); // size * count | uint1�� ����� ���̹Ƿ� 1�� ����
        uint[] data = new uint[1];

        // 3) ������ �����ƴٸ� shader�� �ѱ�
        countShader.SetTexture(kernelCount, "InputTexture", _tex); // ComputeShader�� �̹����� ����
        countShader.SetBuffer(kernelCount, "CountBuffer", buffer); // Buffer�� ����
        countShader.SetBuffer(kernelInitCount, "CountBuffer", buffer);

        // 4) �ʿ��� ���� �� �Ѱ�ٸ� shader ����
        // ���� Shader�� numthreads(8, 8, 1)�̸� shader.Dispatch(kernel, width / 8, height / 8, 1);
        countShader.Dispatch(kernelInitCount, 1, 1, 1);
        countShader.Dispatch(kernelCount, threadGroupX, threadGroupY, 1);

        // 5) ó���� ������ �����ϴ� �κ�
        // Buffer�� ���¾��ٸ� Data�� �������� Release �� null ó��
        buffer.GetData(data);
        buffer.Release();
        buffer = null;

        // 6) ��ȯ�� ���� �ִٸ� �Լ��� ��ȯ���� ���� �� ��ȯ
#if UNITY_EDITOR
        Debug.LogFormat("[CountWhiteColor] Left: {0}", data[0]);
#endif
        return data[0];
    }
    #endregion Pixel Counter


    #region Texture Function (Clear, Reset)
    public void ClearTexture()
    {
        if ((targetTexture.width != originTex.width) &&
            (targetTexture.height != originTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelClear = paintShader.FindKernel(kernelClearName);

        paintShader.SetTexture(kernelClear, "Result", targetTexture);

        paintShader.Dispatch(kernelClear, threadGroupX, threadGroupY, 1);
#if UNITY_EDITOR
        Debug.Log("Dirty Clear.");
#endif
    }

    // ����UV�� RenderTexture�� ����
    public void ResetTexture()
    {
        if ((targetTexture.width != originTex.width) &&
            (targetTexture.height != originTex.height))
        {
#if UNITY_EDITOR
            Debug.LogWarning("RenderTexture is different in size from origin. Check RenderTexture width and height.");
#endif
            return;
        }

        kernelCopy = paintShader.FindKernel(kernelCopyName);

        paintShader.SetTexture(kernelCopy, "Source", originTex);
        paintShader.SetTexture(kernelCopy, "Destination", targetTexture);

        paintShader.Dispatch(kernelCopy, threadGroupX, threadGroupY, 1);
        
        IsClear(false); // �ʱ�ȭ �����Ƿ� Clear -> false
#if UNITY_EDITOR
        Debug.Log("Reset Mask with Origin Texture.");
#endif
    }
    #endregion Texture Function


    #region Material Property
    private void SetBasicTwinkleProperties()
    {
        // Property �� ����
        mr.material.SetFloat("_TwinkleIntensity", 4f); // ��¦�� ���� Intensity(����)
        mr.material.SetFloat("_TwinkleSpeed", 4f); // ��¦�� �ӵ�
    }
    private void SetTwinkleProperties(bool _onlyDirty)
    {
        // Property ������ �ʱ�ȭ
        Color color;
        if (_onlyDirty)
            color = new Color(1f, 0.6501361f, 0.2783019f, 1f);
        else
            color = new Color(0.4009433f, 0.5723213f, 1f, 1f);

        // Property �� ����
        mr.material.SetFloat("_ActiveTwinkle", 1); // ��¦�� ���� ����
        mr.material.SetFloat("_OnlyDirty", _onlyDirty ? 1 : 0); // ���� ������� ����
        mr.material.SetColor("_TwinkleColor", color); // ����
#if UNITY_EDITOR
        Debug.Log("[SetTwinkleProperties] Before Return");
#endif
    }
    private void StopTwinkleProperties()
    {
        // Property �� ����
        mr.material.SetFloat("_ActiveTwinkle", 0); // ��¦�� ���� ����
        mr.material.SetFloat(timerName, 0f);
    }
    #endregion Material Property


    #region Utility
    // Mask Texture�� PNG�� �����ϴ� �뵵
    public void SaveToPNG(Texture2D _tex)
    {
        byte[] bytes = _tex.EncodeToPNG();

        StringBuilder savePath = new StringBuilder();
        savePath.Append(Application.dataPath);
        savePath.Append("/../");
        savePath.Append(_tex.name);
        savePath.Append(".png");

        File.WriteAllBytes(savePath.ToString(), bytes);
    }

    // RenderTexture�� Texture2D�� ��ȯ�Ͽ� ��ȯ
    private Texture2D ToTexture(RenderTexture _rTex)
    {
        Texture2D toTex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        var oldTex = RenderTexture.active;
        RenderTexture.active = _rTex;

        toTex.ReadPixels(new Rect(0, 0, _rTex.width, _rTex.height), 0, 0);
        toTex.name = _rTex.name;
        toTex.Apply();
        RenderTexture.active = oldTex;

        return toTex;
    }
    #endregion


    #region Coroutine
    public void CompleteTwinkle()
    {
        if (isCompleteTwinkle == false)
        {
            isCompleteTwinkle = true;
            StopCoroutine("DirtyTwinkleCoroutine");
            StartCoroutine("CompleteTwinkleCoroutine");
        }
    }
    public void DirtyTwinkle()
    {
        if (isDirtyTwinkle == false && IsClear() == false)
        {
            isDirtyTwinkle = true;
            StartCoroutine("DirtyTwinkleCoroutine");
        }
    }


    private IEnumerator CompleteTwinkleCoroutine()
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(false);
        mr.material.SetFloat(timerName, t);

        time /= mr.material.GetFloat(twinkleSpeed) * 0.5f;

        while (t < time)
        {
            t += Time.deltaTime;
            mr.material.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties();
        isDirtyTwinkle = false; // �� �Ǵ°� �±� ������ Ȥ�����Ͽ� ����
        isCompleteTwinkle = false;
        IsClear(true);
    }
    private IEnumerator DirtyTwinkleCoroutine()
    {
        float time = 3f;
        float t = 0f;

        SetTwinkleProperties(true);
        mr.material.SetFloat(timerName, t);

        time /= mr.material.GetFloat(twinkleSpeed) * 0.5f;
        while (t < time)
        {
            t += Time.deltaTime;
            mr.material.SetFloat(timerName, t);
            yield return null;
        }

        StopTwinkleProperties();
        isDirtyTwinkle = false;
    }
    #endregion
}
