using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider;
    private Vector2 uvPos = Vector2.zero;

    private MeshPaintTarget target = null;

    [SerializeField, Range(0.5f, 10f)]
    private float effectiveDistance = 1f; // 유효 사거리
    [SerializeField, Range(1f, 16f)]
    private float maxDistance = 2f; // 최대 사거리
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


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //IsPainting(true);
            //PaintToTarget();
            TimingDraw();
        }
        else if (IsPainting() == true)
        {
            IsPainting(false);
            StopCheckTargetProcess();
        }
        else if (drawCoroutine == true)
        {
            StopTimingDraw();
#if UNITY_EDITOR
            Debug.Log("몇 번 호출되나요?");
#endif
        }

        // Utility
        // Dirty 모두 제거 (일정 %에 도달하면 사용할 부분(지금은 단일대상))
        if (target != null && Input.GetKeyDown(KeyCode.E))
        {
            target.ClearTexture();
        }
        // Dirty 초기화 (초기화 버튼을 누른다면 적용할 부분(지금은 단일대상))
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


    // 임시 명칭
    private void PaintToTarget()
    {
#if UNITY_EDITOR
        Debug.Log("Try");
#endif
        // Viewport상 
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        screenRay.origin = this.transform.position;
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
                    // 유효한 사거리라면 DrawRender를 실행
                    if (effective)
                    {
                        // target에서 Draw처리하게 하는데
                        // uvPos와 Paint의 Color, Size, Distance, Drawable을 넘김 (Compute Shader에서 처리할 부분)

                        // 유효사거리 내에 있다면 효과 1, 최대사거리에 근접한다면 효과 1 ~ 0
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


    #region Paint Coroutine
    private void TimingDraw()
    {
        if (drawCoroutine == false)
        {
            drawCoroutine = true;
            StartCoroutine("TimingDrawCoroutine");
        }
    }
    private void StopTimingDraw()
    {
        if (drawCoroutine == true)
        {
            drawCoroutine = false;
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
            IsPainting(true);
            PaintToTarget();
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
            if (target != null)
                target.CheckAutoClear();
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion
}
