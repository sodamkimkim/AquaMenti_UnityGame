using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectSpellManager;
public class SelectStaffManager : MonoBehaviour
{
    [Header("Aggregation")]
    [SerializeField]
    private GameObject selectStaffUIGo_ = null;
    private Item[] itemArr = null;


    public delegate void OpenInvenUICallback_();
    public delegate void CloseInvenUICallback_(NowWearingInfo.NowWearingItem _selectItem);

    private OpenInvenUICallback_ openCallback_;
    private CloseInvenUICallback_ closeCallback_;
    private bool isSelectStaffUIOpen_ { get; set; }
    private void Awake()
    {
        isSelectStaffUIOpen_ = false;
    }
    public void Init(OpenInvenUICallback_ _OpenCallback, CloseInvenUICallback_ _CloseCallback_)
    {
        openCallback_ += _OpenCallback;
        closeCallback_ += _CloseCallback_;
    }
    /// <summary>
    /// inventory UI���� CurrentStaff��ư ������ ����Ǵ� �ݹ�
    /// </summary>
    /// 
    public void OnCurrentStaffClickCallback()
    {

    }
    private void CloseSelectSpellCallback()
    {

    }
    public void OpenSelectStaffUI()
    {

    }
    public void CloseSelectStaffUI()
    {

    }
    /// <summary>
    /// item ���� �Ϸ� �������ִ� �޼���
    /// �ش� ui�� ����
    /// </summary>
    public void SelectItem()
    {

    }
}  // end of class
