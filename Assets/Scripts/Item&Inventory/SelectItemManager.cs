using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemManager : MonoBehaviour
{
    [Header("Aggregation")]

    private ItemInfo[] itemArr = null;
    public delegate void OpenInvenUICallback_();
    public delegate void CloseInvenUICallback_(NowWearingInfo.NowWearingItem _selectItem);

    private OpenInvenUICallback_ openCallback_;
    private CloseInvenUICallback_ closeCallback_;
    private bool isSelectItemUIOpen_ { get; set; }
    private void Awake()
    {
        isSelectItemUIOpen_ = false;
    }
    public void Init(OpenInvenUICallback_ _OpenCallback, CloseInvenUICallback_ _CloseCallback_)
    {
        openCallback_ += _OpenCallback;
        closeCallback_ += _CloseCallback_;
    }
    /// <summary>
    /// inventory UI���� CurrentSpell��ư ������ ����Ǵ� �ݹ�
    /// </summary>
    public void OnCurrentEquipedItemClickCallback()
    {
        openCallback_.Invoke();
        this.gameObject.SetActive(true);
        isSelectItemUIOpen_ = true;
        itemArr = GetComponentsInChildren<ItemInfo>();
        foreach (ItemInfo item in itemArr)
        {
            item.Init(SelectItemCallback);
        }
    }
    /// <summary>
    /// : selectSpell UI���� item ������ ����Ǵ� �ݹ�
    ///  1. Ŭ���� item ���� NowWearing�� �ѷ��ְ�
    ///  2. selectSpell UI ����
    /// </summary>
    private void SelectItemCallback(NowWearingInfo.NowWearingItem _selectItem)
    {

        // �����Ŵ������� ���� �ݹ��Լ� ����
        closeCallback_.Invoke(_selectItem);
        this.gameObject.SetActive(false);
        isSelectItemUIOpen_ = false;
    }
}  // end of class
