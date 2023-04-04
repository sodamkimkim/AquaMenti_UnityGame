using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WandRaySpawner : MonoBehaviour
{
    private UIFocusPoint uIFocusPoint_;
    private Ray ray_;
    public Vector3 hitPos_ { get; set; }
    private float rayMaxDistance_ = 10;
    private Vector3 screenCenter_;


    public bool isLadder_ { get; set; }
    private void Awake()
    {
        uIFocusPoint_ = GameObject.FindWithTag("Canvas_Focus").GetComponentInChildren<UIFocusPoint>();
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
        Debug.Log("FocusFixed()");
        ray_ = Camera.main.ScreenPointToRay(screenCenter_);
        uIFocusPoint_.SetPos(screenCenter_);
        Debug.DrawRay(GetPos(), GetTransform().forward * rayMaxDistance_, Color.red);
        RayFindObject();
    }
    /// <summary>
    /// WandRaySpawner�κ��� MousePosition���� Ray���ִ� �޼���
    /// </summary>
    public void RayMoveFocusShot()
    {
        // mousePosition�� ���� ȭ�� ���ư��� �ʰ� �� �����
        // 
        Debug.Log("FocusMove()");
        Vector3 mousePos = Input.mousePosition;
        ray_ = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray_, out hit, rayMaxDistance_))
        {
            uIFocusPoint_.SetPos(mousePos);
            Debug.DrawRay(GetPos(), GetTransform().forward * rayMaxDistance_, Color.red);
            RayFindObject();
        }

    }
    private void RayFindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray_, out hit, rayMaxDistance_))
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
            hitPos_ = GetPos() + GetTransform().forward * rayMaxDistance_;
        }
    }

} // end of class
