using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Aggregation")] // # Aggregation(���հ���) : �����ֱ⸦ �޸� ��. ex) �������, �����л�
    [SerializeField]
    private GameObject inventoryPanUIGo_ = null; // inventory panel
    [SerializeField]
    private GameObject[] invenUIArr = null; // Inventory panel ������ Inventory, SelectStaff, SelectSpell UI GameObject �����ϴ� Arr

    [Header("Composition")] // # Composition(��������) : �����ֱ⸦ ���� ��. ex) �� & ����
    private NowWearingInfo nowWearingInfo_;
    private SelectItemManager selectStaffManager_;
    private SelectItemManager selectSpellManager_;

    // # item ���� ����
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;   
    [SerializeField]
    private GameObject[] staffArr_= null;

    [SerializeField]
    private GameObject[] staffParts = null; // # staff ���� brushgroup, waterpump gameObject

    public bool isInventoryPanOpen_ { get; set; }

    private Staff nowStaff_ = null;
    private void Awake()
    {
        // # ���� �ʱ�ȭ
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = inventoryPanUIGo_.GetComponentInChildren<NowWearingInfo>();
        selectStaffManager_ = invenUIArr[1].GetComponent<SelectItemManager>();
        selectSpellManager_ = invenUIArr[2].GetComponent<SelectItemManager>();

        // # ���� �޴��� ������ �ʱ�ȭ => �ݹ��Լ� ����
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);

    }
    private void Start()
    {
        SetDefaultPlayerItem();
    }
    private void SetDefaultPlayerItem()
    {
        SetStaff(0);
        // TODO Spell
        // SetSpell(0);
    }
    private void SetStaff(int _idx)
    {
        CloseAllstaff();
        staffArr_[_idx].SetActive(true);
        foreach(GameObject go in staffParts)
        {
            go.SetActive(true);
            go.transform.SetParent(staffArr_[_idx].transform);
        }
        nowStaff_ = staffArr_[_idx].gameObject.GetComponent<Staff>();
        playerFocusManager_.SetStaff(nowStaff_);
    }


    public void OpenInventoryPan()
    {
        isInventoryPanOpen_ = true;
        CloseAllInvenUIAndOpenDefaultUI();
        inventoryPanUIGo_.SetActive(true);
    }
    public void CloseInventoryPan()
    {
        isInventoryPanOpen_ = false;
        inventoryPanUIGo_.SetActive(false);
    }
    /// <summary>
    /// : ���� �Ŵ������� item �����ϸ� �ݹ����� �� �޼��� ����.
    /// - invenUI�ٲ��ְ� invenUI���ִ� �޼���
    /// </summary>
    private void SelectItem(NowWearingInfo.NowWearingItem _selectItem)
    {
        CloseAllInvenUIAndOpenDefaultUI();
        // # staff spell �����ؼ� NowWearing �Լ� ȣ��
        if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Staff.ToString()))
        { // # Staff 
            nowWearingInfo_.SetNowWearingItem(_selectItem);
            if(_selectItem.itemName_==InGameAllItemInfo.EStaffName.AmberStaff.ToString())
            { // AmberStaff �ѱ�
                SetStaff(0);
            }
            else if(_selectItem.itemName_ == InGameAllItemInfo.EStaffName.RubyStaff.ToString())
            {
                SetStaff(1);
            }

        }
        else if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingItem(_selectItem);
            // TODO
        }
    }
    /// <summary>
    /// invenPanel ���� inven UI�� �� ���ִ� �޼���
    /// </summary>
    private void CloseAllInvenUI()
    {
        foreach (GameObject invenUIgo in invenUIArr)
        {
            invenUIgo.SetActive(false);
        }
    }
    private void CloseAllInvenUIAndOpenDefaultUI()
    {
        CloseAllInvenUI();
        invenUIArr[0].SetActive(true);
    }
    private void CloseAllstaff()
    {
        foreach(GameObject go in staffArr_)
        {
            go.SetActive(false);
        }
    }
} // end of class
