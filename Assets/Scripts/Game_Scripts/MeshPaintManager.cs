using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintManager : MonoBehaviour
{
    private List<MeshPaintTarget> meshTargetList_ = null;


    private void Awake()
    {
        // Scene�� �����ϴ� Object�� ������� MeshPaintTarget�� �����ɴϴ�.
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
#if UNITY_EDITOR
        // Debug.Log("[MeshPaintManager] target Count: " + meshTargetList_.Count);
#endif
    }

    private void Update()
    {
        // �ӽ� Save ��ư
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveTargetMask();
        }
    }


    public void Init()
    {
        if (meshTargetList_.Count <= 0) return;

        LoadTargetMask();
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
                Debug.LogWarning("�Ϻ� ����� Mask�� �ҷ����µ� �����Ͽ����ϴ�.");
            }
        }
    }

}
