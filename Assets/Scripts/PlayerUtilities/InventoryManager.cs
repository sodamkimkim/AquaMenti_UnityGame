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
    private SelectStaffManager selectStaffManager_;
    private SelectSpellManager selectSpellManager_;

    public bool isInventoryPanOpen_ { get; set; }

    private void Awake()
    {
        // # ���� �ʱ�ȭ
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = GetComponent<NowWearingInfo>();
        selectStaffManager_ = invenUIArr[1].GetComponent<SelectStaffManager>();
        selectSpellManager_ = invenUIArr[2].GetComponent<SelectSpellManager>();

        // # ���� �޴��� ������ �ʱ�ȭ => �ݹ��Լ� ����
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);
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
            nowWearingInfo_.SetNowWearingStaff(_selectItem);
        }
        else if(_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingSpell(_selectItem);
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
} // end of class
