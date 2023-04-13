using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Aggregation")] // # Aggregation(���հ���) : �����ֱ⸦ �޸� ��. ex) �������, �����л�
    [SerializeField]
    private GameObject inventoryPanUIGo_ = null; // inventory panel

    // # item ���� ����
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo = null;

    [Header("Composition")] // # Composition(��������) : �����ֱ⸦ ���� ��. ex) �� & ����
    private NowWearingInfo nowWearingInfo_;
    private SelectItemManager selectStaffManager_;
    private SelectItemManager selectSpellManager_;
    [SerializeField]
    private GameObject[] staffArr_ = null;
    [SerializeField]
    private GameObject[] staffPartsArr_ = null; // # staff ���� brushgroup, waterpump gameObject
    [SerializeField]
    private GameObject[] bottomImgArr_ = null; // # [0] : staffImage, [1] : spellImage

    public bool isInventoryPanOpen_ { get; set; }
    private Staff nowStaff_ = null;
    [SerializeField]
    private GameObject[] invenUIArr_ = new GameObject[3]; // Inventory panel ������ Inventory, SelectStaff, SelectSpell UI GameObject �����ϴ� Arr
    private void Awake()
    {
        // # ���� �ʱ�ȭ
        isInventoryPanOpen_ = false;
        nowWearingInfo_ = invenUIArr_[0].GetComponent<NowWearingInfo>();

        selectStaffManager_ = invenUIArr_[1].GetComponent<SelectItemManager>();
        selectSpellManager_ = invenUIArr_[2].GetComponent<SelectItemManager>();
        // # ���� �޴��� ������ �ʱ�ȭ => �ݹ��Լ� ����
        selectStaffManager_.Init(CloseAllInvenUI, SelectItem);
        selectSpellManager_.Init(CloseAllInvenUI, SelectItem);
    }
    private void Start()
    {

        SetDefaultPlayerItem();
    }
    /// <summary>
    /// ���� ������ ��, default�� �����ϴ� ������ ����
    /// </summary>
    private void SetDefaultPlayerItem()
    {
        SetStaff(0);
        SetBottomUIStaffImg("AmberStaff");
       // SetSpell(0);
        SetBottomUISpellImg("Deg0MagicSpell");
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
            SetStaff(_selectItem);

        }
        else if (_selectItem.itemCategory_.Equals(InGameAllItemInfo.EItemCategory.Spell.ToString()))
        { // # Spell
            nowWearingInfo_.SetNowWearingItem(_selectItem);
            SetSpell(_selectItem); // TODO

        }
    }
    /// <summary>
    /// invenPanel ���� inven UI�� �� ���ִ� �޼���
    /// </summary>
    private void CloseAllInvenUI()
    {
        foreach (GameObject invenUIgo in invenUIArr_)
        {
            invenUIgo.SetActive(false);
        }
    }
    private void CloseAllInvenUIAndOpenDefaultUI()
    {
        CloseAllInvenUI();
        invenUIArr_[0].SetActive(true);
    }
    /// <summary>
    /// staff ������Ʈ ��ü�ϱ� ��, ��� staff �� ���ִ� �޼��� 
    /// </summary>
    private void CloseAllstaff()
    {
        foreach (GameObject go in staffArr_)
        {
            go.SetActive(false);
        }
    }

    private void SetStaff(NowWearingInfo.NowWearingItem _selectItem)
    {
        if (_selectItem.itemName_ == InGameAllItemInfo.EStaffName.AmberStaff.ToString())
        { // # AmberStaff �ѱ�
            SetStaff(0);
            SetBottomUIStaffImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.EStaffName.RubyStaff.ToString())
        { // # RubyStaff �ѱ�
            SetStaff(1);
            SetBottomUIStaffImg(_selectItem.itemImgFileName_);
        }
    }
    /// <summary>
    /// Staff �ٲ㳢���ִ� �޼���
    /// </summary>
    /// <param name="_idx"></param>
    private void SetStaff(int _idx)
    {
        CloseAllstaff();
        staffArr_[_idx].SetActive(true);
        foreach (GameObject go in staffPartsArr_)
        {
            go.SetActive(true);
            go.transform.SetParent(staffArr_[_idx].transform);
            //TODO
            // staff ������ ��, parts default ���� ����� ��!

        }
        nowStaff_ = staffArr_[_idx].gameObject.GetComponent<Staff>();
        playerFocusManager_.SetStaff(nowStaff_);
    }
    private void SetSpell(NowWearingInfo.NowWearingItem _selectItem)
    {
        // TODO
        if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg0MagicSpell.ToString())
        { // # AmberStaff �ѱ�
            //SetSpell(0);
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg15MagicSpell.ToString())
        { // # RubyStaff �ѱ�
            //SetSpell(1);
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg25MagicSpell.ToString())
        { // # RubyStaff �ѱ�
            //SetSpell(3);
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }
        else if (_selectItem.itemName_ == InGameAllItemInfo.ESpellName.Deg45MagicSpell.ToString())
        { // # RubyStaff �ѱ�
            //SetSpell(4);
            SetBottomUISpellImg(_selectItem.itemImgFileName_);
        }

    }
    private void SetBottomUIStaffImg(string _imgFileName)
    {
        Image img = bottomImgArr_[0].GetComponent<Image>();
        img.sprite = inGameAllItemInfo.GetItemImg(_imgFileName);
    }
    private void SetBottomUISpellImg(string _imgFileName)
    {
        Image img = bottomImgArr_[1].GetComponent<Image>();
        img.sprite = inGameAllItemInfo.GetItemImg(_imgFileName);
    }
} // end of class