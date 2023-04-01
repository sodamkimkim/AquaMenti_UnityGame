using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider;
    private Vector2 uvPos = Vector2.zero;

    private MeshPaintTarget target = null;

    private float effectiveDistance = 1f; // ��ȿ ��Ÿ�
    private float maxDistance = 2f; // �ִ� ��Ÿ�
    private bool effective = false;

    // Option
    [SerializeField, Range(1f, 20f)]
    private float size = 10f;
    private Vector4 color = new Vector4(0f, 0f, 0f, 1f);
    private bool isPainting = false;

    private float waitTime = 0.1f;

    private bool runCoroutine = false;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            IsPainting(true);
            PaintToTarget();
            // ������ Pos�� ���ؼ� Paint�� �������� ������ �� �Ѹ��� �Ͱ� Splash ����Ʈ�� ��ӵǾ����
        }
        else if (IsPainting() == true)
        {
            IsPainting(false);
            StopCheckTargetProcess();
        }

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
    private void PaintToTarget()
    {
#if UNITY_EDITOR
        Debug.Log("Try");
#endif
        // Viewport�� 
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
#if UNITY_EDITOR
        Debug.DrawRay(screenRay.origin, screenRay.direction, Color.green);
#endif
        if (Physics.Raycast(screenRay, out var hitInfo))
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
                target.IsDrawable() == true &&
                target.IsClear() == false)
            {
#if UNITY_EDITOR
                Debug.LogFormat("uvPos: {0} | coord: {1}", uvPos, hitInfo.textureCoord);
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

                        target.DrawRender(isPainting, uvPos, color, size, distance);
                        CheckTargetProcess();
                    }
                }
            }
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

    private IEnumerator CheckTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("[CheckTargetProcessCoroutine]");
#endif
        while (true)
        {
            if (target != null)
                target.CheckAutoClear();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
