using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WandRaySpawner : MonoBehaviour
{
    // # Composition
    [SerializeField]
    private UIFocusPoint uIFocusPoint_;
    [SerializeField]
    private MeshPaintBrush[] sideRayBrushArr = new MeshPaintBrush[5];

    // # side rays endPosition���� �ʵ�
    [SerializeField]
    private float rayPosDefaultOffset_ = 0.05f; // 5�� �� ray�� offset, center�� �������� �ش� ����ŭ �������� ���� (Rotate�� �⺻ �� : X)
    [SerializeField]
    private float mainRayMaxDistance_ = 10f;
    [SerializeField]
    private float sideRayMaxDistance_ = 0f; // centerRayMaxDistance �� 0.5��ŭ���� �ʱ�ȭ

    private Ray centerRay_;

    private Vector3 screenCenter_;
    public Vector3 hitPos_ { get; set; }
    public bool isLadder_ { get; set; }

    private void Awake()
    {
        sideRayMaxDistance_ = mainRayMaxDistance_ * 0.5f;
        screenCenter_ = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        isLadder_ = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }
    public Vector3 GetLocalPos()
    {
        return transform.localPosition;
    }
    /// <summary>
    /// WandRaySpawner�κ��� SreenCenter�� Ray���ִ� �޼���
    /// Ray ���� IInteractableObject ��ü �ν� ��.
    /// </summary>
    public void RayScreenCenterShot()
    {
        // Debug.Log("FocusFixed()");
        centerRay_ = Camera.main.ScreenPointToRay(screenCenter_);
      //  staff_.LookAtRay(centerRay_.direction);
        uIFocusPoint_.SetPos(screenCenter_);
        Debug.DrawRay(GetPos(), GetTransform().forward * mainRayMaxDistance_, Color.red);
        RayFindObject();
    }
    /// <summary>
    /// WandRaySpawner�κ��� MousePosition���� Ray���ִ� �޼���
    /// </summary>
    public void RayMoveFocusShot()
    {
        // mousePosition�� ���� ȭ�� ���ư��� �ʰ� �� �����
        // Debug.Log("FocusMove()");
        Vector3 mousePos = Input.mousePosition;
        centerRay_ = Camera.main.ScreenPointToRay(mousePos);
       // staff_.LookAtRay(centerRay_.direction);
        RaycastHit hit;
        if (Physics.Raycast(centerRay_, out hit, mainRayMaxDistance_))
        {
            uIFocusPoint_.SetPos(mousePos);
            Debug.DrawRay(GetPos(), GetTransform().forward * mainRayMaxDistance_, Color.red);
            RayFindObject();
        }

    }
    private void RayFindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(centerRay_, out hit, mainRayMaxDistance_))
        {
            IInteractableObject target = hit.collider.GetComponentInParent<IInteractableObject>();
            if (target != null)
            {
                Debug.Log(target.GetName());

                // target�̸��� Ladder�̸� bool ���� �ٲ㼭 Ladder�� ��ġ�� �ű� �� ����
                if (target.GetName() == IInteractableTool.EInteractableTool.Ladder.ToString())
                {
                    isLadder_ = true;
                }
                else
                {
                    isLadder_ = false;
                }
            }
            hitPos_ = hit.point;
        }
        else
        {
            hitPos_ = GetPos() + GetTransform().forward * mainRayMaxDistance_;
        }
 
    }
    public Vector3 GetHitPos()
    {
        return hitPos_;
    }
    public void RaysIsPainting(bool _para)
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.IsPainting(_para);
        }
    }
    /// <summary>
    /// //////////////////////////////
    /// </summary>
    public void RaysTimingDraw()
    {
        sideRayBrushArr[0].TimingDraw(centerRay_);
        sideRayBrushArr[1].TimingDraw(centerRay_);
        sideRayBrushArr[2].TimingDraw(centerRay_);
        sideRayBrushArr[3].TimingDraw(centerRay_);
        sideRayBrushArr[4].TimingDraw(centerRay_);
    }
    public bool RaysIsPainting()
    {
        if (sideRayBrushArr[2].IsPainting() == true)
        {
            return true;
        }
        return false;
    }
    public void RaysStopCheckTargetProcess()
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.StopCheckTargetProcess();
        }
    }
    public void RaysStopTimingDrow()
    {
        foreach (MeshPaintBrush brushRay in sideRayBrushArr)
        {
            brushRay.StopTimingDraw();
        }
    }
} // end of class
