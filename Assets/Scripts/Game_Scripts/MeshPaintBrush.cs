using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider;
    private Vector2 uvPos = Vector2.zero;

    private MeshPaintTarget target = null;

    [SerializeField, Range(0.5f, 10f)]
    private float effectiveDistance = 1f; // ��ȿ ��Ÿ�
    [SerializeField, Range(1f, 16f)]
    private float maxDistance = 2f; // �ִ� ��Ÿ�
    private bool effective = false;

    // Option
    [SerializeField, Range(1, 20)]
    private int size = 10;
    private Vector4 color = new Vector4(0f, 0f, 0f, 1f);
    private bool isPainting = false;

    private float drawTiming = 0.01f;
    private float waitTime = 0.1f;

    private bool drawCoroutine = false;
    private bool runCoroutine = false;


    // �ӽ�
    private enum MagicType { Zero, One, Two, Three, Four }

    private struct DirtyLv // ����Ÿ��
    {
        public int rLv { get; set; } // ǥ��
        public int gLv { get; set; } // �ڵ���
        public int bLv { get; set; } // ���
    }

    private struct Stick
    {
        public DirtyLv cleanLv;
        public MagicType magicType; // ���� => Nozzle
    }

    // �ӽ�
    // R: ǥ��, G: �ڵ���, B: ���
    //private int rDirtyLv = 5;
    //private int rCleanLv = 3;
    //private int magicType = 2; // Nozzle. Maybe Enum.
    private Stick stick;
    private DirtyLv dirty;


    private void Start()
    {
        // ������ ���� ����
        stick = new Stick();
        DirtyLv cleanLv = new DirtyLv();
        cleanLv.rLv = 5;
        cleanLv.gLv = 3;
        cleanLv.bLv = 1;
        stick.cleanLv = cleanLv;
        stick.magicType = 0;

        // ��� ������ ����
        dirty = new DirtyLv();
        dirty.rLv = 10;
        dirty.gLv = 0;
        dirty.bLv = 1;
    }
    // End �ӽ� //

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //IsPainting(true);
            //PaintToTarget();
            TimingDraw();
            Debug.Log("���콺 ��Ŭ��");
        }
        else if (IsPainting() == true)
        {
            IsPainting(false);
            StopCheckTargetProcess();
            StopTimingDraw();
            Debug.Log("���콺 ��Ŭ�� ����");
        }
        //else if (drawCoroutine == true)
        //{
        //    StopTimingDraw();
        //    Debug.Log("�� �� ȣ��ǳ���?");
        //}

        // �ӽ� ���� ���� //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stick.magicType = MagicType.Zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stick.magicType = MagicType.One;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stick.magicType = MagicType.Two;
        }
        // End �ӽ� ���� ���� //

        // Utility
        // Dirty ��� ���� (���� %�� �����ϸ� ����� �κ�(������ ���ϴ��))
        if (target != null && Input.GetKeyDown(KeyCode.E))
        {
            target.ClearTexture();
        }
        // Dirty �ʱ�ȭ (�ʱ�ȭ ��ư�� �����ٸ� ������ �κ�(������ ���ϴ��))
        if (target != null && Input.GetKeyDown(KeyCode.R))
        {
            target.ResetTexture();
        }
        if (target != null && target.IsDrawable() && Input.GetKeyDown(KeyCode.T))
        {
            target.CompleteTwinkle();
        }
    }


    private bool IsPainting()
    {
        return isPainting;
    }
    private void IsPainting(bool _do)
    {
        isPainting = _do;
    }


    // �ӽ� ��Ī
    public void PaintToTarget(Ray _ray)
    {
#if UNITY_EDITOR
        Debug.Log("Try");
        //Debug.DrawRay(screenRay.origin, screenRay.direction, Color.green);
#endif
        if (Physics.Raycast(_ray, out var hitInfo))
        {
#if UNITY_EDITOR
            Debug.Log("In Raycast");
#endif
            if (prevCollider != hitInfo.collider)
            {
                prevCollider = hitInfo.collider;
                hitInfo.collider.TryGetComponent<MeshPaintTarget>(out target);
            }
#if UNITY_EDITOR
            Debug.Log("target? " + target);
#endif
            if (target != null &&
                target.IsDrawable() == true)
            {
#if UNITY_EDITOR
                //Debug.LogFormat("uvPos: {0} | coord: {1}", uvPos, hitInfo.textureCoord);
#endif
                if (uvPos != hitInfo.textureCoord)
                {
#if UNITY_EDITOR
                    Debug.Log("Coord is not Same.");
#endif
                    uvPos = hitInfo.textureCoord;
                    if (maxDistance >= hitInfo.distance)
                        effective = true;
                    else if (effectiveDistance >= hitInfo.distance)
                        effective = true;
                    else
                        effective = false;

#if UNITY_EDITOR
                    Debug.Log("Before Draw" + effective);
#endif
                    // ��ȿ�� ��Ÿ���� DrawRender�� ����
                    if (effective)
                    {
                        // target���� Drawó���ϰ� �ϴµ�
                        // uvPos�� Paint�� Color, Size, Distance, Drawable�� �ѱ� (Compute Shader���� ó���� �κ�)

                        // ��ȿ��Ÿ� ���� �ִٸ� ȿ�� 1, �ִ��Ÿ��� �����Ѵٸ� ȿ�� 1 ~ 0
                        float distance;
                        if (hitInfo.distance <= effectiveDistance)
                            distance = 1f;
                        else 
                            distance = 1 - ((hitInfo.distance - effectiveDistance) / (maxDistance - effectiveDistance));

                        // ��ô��
                        color = WashPower();
#if UNITY_EDITOR
                        Debug.LogFormat("r: {0}, g: {1}, b: {2}", color.x, color.y, color.z);
#endif
                        if (target.IsClear() == false)
                            target.DrawRender(isPainting, uvPos, color, size, distance);
                        target.DrawWet(isPainting, uvPos, size, distance);
                        CheckTargetProcess();
                    }
                }
            }
        }
    }


    // ���� ��ô�� ����
    private Vector4 WashPower()
    {
        DirtyLv cleanLv = stick.cleanLv;

        float rPow = CalculatePower(cleanLv.rLv, dirty.rLv, stick.magicType);
        float gPow = CalculatePower(cleanLv.gLv, dirty.gLv, stick.magicType);
        float bPow = CalculatePower(cleanLv.bLv, dirty.bLv, stick.magicType);

        return new Vector4(rPow, gPow, bPow, 1);
    }

    // ��ô�� ���
    private float CalculatePower(float _cleanLv, float _dirtyLv, MagicType _type)
    {
        float magicPw = GetMagicPower(_type);

        float cleanLvPw = (float)_cleanLv / _dirtyLv; // Ÿ���� ���� �Ϳ� ����

        float pw = cleanLvPw * magicPw; // �⺻: ������Pw(��ô��) * ����Pw(����)

        return pw;
    }

    // ���� ���翡 ���� ����ġ
    private float GetMagicPower(MagicType _type)
    {
        switch((int)_type)
        {
            default:
                return 1f;
            case 1: // 15��
                return 0.85f;
            case 2: // 25��
                return 0.7f;
            case 3: // 45��
                return 0.45f;
            case 4: // ��ô��
                return 1f;
        }
    }


    #region Paint Coroutine
    private void TimingDraw()
    {
        if (drawCoroutine == false)
        {
            drawCoroutine = true;
            IsPainting(true);
            StartCoroutine("TimingDrawCoroutine");
        }
    }
    private void StopTimingDraw()
    {
        if (drawCoroutine == true)
        {
            drawCoroutine = false;
            IsPainting(false);
            StopCoroutine("TimingDrawCoroutine");
        }
    }

    private void CheckTargetProcess()
    {
        if (runCoroutine == false)
        {
            runCoroutine = true;
            StartCoroutine("CheckTargetProcessCoroutine");
        }
    }
    private void StopCheckTargetProcess()
    {
        if (runCoroutine == true)
        {
            runCoroutine = false;
            StopCoroutine("CheckTargetProcessCoroutine");
        }
    }


    // Brush
    private IEnumerator TimingDrawCoroutine()
    {
        while(true)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            screenRay.origin = this.transform.position;
            PaintToTarget(screenRay);
            yield return new WaitForSeconds(drawTiming);
        }
    }

    private IEnumerator CheckTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("[CheckTargetProcessCoroutine]");
#endif
        while (true)
        {
            if (target != null && target.IsDrawable())
                target.CheckAutoClear();
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion
}
