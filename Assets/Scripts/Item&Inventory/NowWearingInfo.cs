using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NowWearingInfo : MonoBehaviour
{
    private EquipedStaff equipedStaff_ = null;
    private EquipedSpell equipedSpell_ = null;
    /// <summary>
    /// ���� ���� ���� ������ ���� ��Ͽ� ����� structure
    /// </summary>
    public struct NowWearingItem
    { // # staff : [0], # spell : [1]
        // item����
        public string itemCategory_;
        public string itemName_;
        public string itemImgFileName_; // Asset�� ����� image���� �̸�
        public string itemDescription_;
        public NowWearingItem(string _itemCategory, string _itemName, string _itemImgFileName_, string _itemDescription)
        {
            this.itemCategory_ = _itemCategory;
            this.itemName_ = _itemName;
            this.itemImgFileName_ = _itemImgFileName_;
            this.itemDescription_ = _itemDescription;
        } // end of construct
        public override string ToString()
        {
            return $"itemCategory_ : {itemCategory_}, itemName_ : {itemName_}, itemImg_ : {itemName_}, itemDescription_ : {itemDescription_}";
        }
    } // end of struct
    /// <summary>
    ///  : ���� �������� ������ ���� ���� arr
    ///  - [0] : Staff, [1] : Spell
    /// </summary>
    private NowWearingItem[] nowWearingArr_ = new NowWearingItem[2];

    private void Awake()
    {
        // # �⺻ Staff�� �⺻ Spell�� NowWearing���� �ʱ�ȭ
        // TODO
        //nowWearingArr_[0] = 
        //nowWearingArr_[1] = 
        equipedStaff_ = GetComponentInChildren<EquipedStaff>();
        equipedSpell_ = GetComponentInChildren<EquipedSpell>();
        SetDefaultItems();
    }
    private void SetDefaultItems()
    {
        NowWearingItem defaultStaff = new NowWearingItem();
        defaultStaff.itemCategory_ = "Staff";
        defaultStaff.itemName_ = "AmberStaff";
        defaultStaff.itemDescription_ = "ȣ�ں����� �����ִ� ������";
        defaultStaff.itemImgFileName_ = "AmberStaff";
        //nowWearingArr_[0] = defaultStaff;
        SetNowWearingStaff(defaultStaff);

        NowWearingItem defaultSpell = new NowWearingItem();
        defaultSpell.itemCategory_ = "Spell";
        defaultSpell.itemName_ = "Deg0MagicSpell";
        defaultSpell.itemDescription_ = "����0���� ����Ǵ� �� ����";
        defaultSpell.itemImgFileName_ = "Deg0MagicSpell";
        //nowWearingArr_[1] = defaultSpell;
        SetNowWearingSpell(defaultSpell);
    }
    /// <summary>
    /// : ���� �������� Staff���� ����
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingStaff(NowWearingItem _selectItem)
    {
        nowWearingArr_[0] = _selectItem;
        Debug.Log(nowWearingArr_[0].ToString());
        Debug.Log(nowWearingArr_[1].ToString());

        equipedStaff_.itemCategory = nowWearingArr_[0].itemCategory_;
        equipedStaff_.itemName = nowWearingArr_[0].itemName_;
        equipedStaff_.description = nowWearingArr_[0].itemDescription_;
        equipedStaff_.imageFileName = nowWearingArr_[0].itemImgFileName_;

        equipedStaff_.SetEquipedItemUI();

    }

    /// <summary>
    /// : ���� �������� Spell���� ����
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingSpell(NowWearingItem _selectItem)
    {
        nowWearingArr_[1] = _selectItem;
        Debug.Log(nowWearingArr_[0].ToString());
        Debug.Log(nowWearingArr_[1].ToString());
        equipedSpell_.itemCategory = nowWearingArr_[1].itemCategory_;
        equipedSpell_.itemName = nowWearingArr_[1].itemName_;
        equipedSpell_.description = nowWearingArr_[1].itemDescription_;
        equipedSpell_.imageFileName = nowWearingArr_[1].itemImgFileName_;

        equipedSpell_.SetEquipedItemUI();
    }
    public NowWearingItem GetNowWearingStaff()
    {
        return nowWearingArr_[0];
    }
    public NowWearingItem GetNowWearingSpell()
    {
        return nowWearingArr_[1];
    }
} // end of class
