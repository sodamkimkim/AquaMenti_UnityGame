using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintManager : MonoBehaviour
{
    private List<MeshPaintTarget> meshTargetList_ = null;
    [SerializeField] GameManager gameManager_;


    private void Awake()
    {
        // Scene�� �����ϴ� Object�� ������� MeshPaintTarget�� �����ɴϴ�.
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
#if UNITY_EDITOR
        Debug.Log("[MeshPaintManager] target Count: " + meshTargetList_.Count);
#endif
    }

    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        /*        // �ӽ� Save ��ư
                if (Input.GetKeyDown(KeyCode.O))
                {
                    SaveTargetMask();
                }*/
        // �ӽ� Reset ��ư
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetTargetMask();
        }
    }


    public void Init()
    {
        if (meshTargetList_.Count <= 0) return;

        InitTarget();
        LoadTargetMask();
    }


    private void InitTarget()
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            target_.Init();
        }
    }


    public void SaveTargetMask()
    {
        // ���� Section�� �׸��� ����� ���� ����� ��
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                target_.SaveMask();
            }
        }
    }

    public void LoadTargetMask()
    {
        // �׸��� ����� �ƴϾ �ٸ� ������ ���൵�� �� �� �ֵ��� �ϱ� ���� ���� ����
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.LoadMask() == false)
            {
                Debug.LogWarning("�Ϻ� ����� Mask�� �ҷ����µ� �����Ͽ����ϴ�." + target_.name);
            }
        }
    }

    public void ResetTargetMask()
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
            {
                if (target_.ResetMask() == false)
                {
                    Debug.LogWarning("�Ϻ� ����� Mask�� �ʱ�ȭ�ϴµ� �����Ͽ����ϴ�.");
                }
            }
        }
    }

}
