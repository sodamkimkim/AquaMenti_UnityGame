using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NowWearingInfo : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ���� ������ ���� ��Ͽ� ����� structure
    /// </summary>
    public struct NowWearingItem
    { // # staff : [0], # spell : [1]
        // item����
        public string itemCategory_;
        public string itemName_;
        public string itemImg_; // Asset�� ����� image���� �̸�
        public string itemDescription_;
        public NowWearingItem(string _itemCategory, string _itemName, string _itemImg, string _itemDescription)
        {
            this.itemCategory_ = _itemCategory;
            this.itemName_ = _itemName;
            this.itemImg_ = _itemImg;
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
        // TODO
        // UI������Ʈ �Լ� ���� �� �־��ֱ�
    }

    /// <summary>
    /// : ���� �������� Spell���� ����
    /// </summary>
    /// <param name="_selectItem"></param>
    public void SetNowWearingSpell(NowWearingItem _selectItem)
    {
        nowWearingArr_[1] = _selectItem;
        // TODO
        // UI������Ʈ �Լ� ���� �� �־��ֱ�
        Debug.Log(nowWearingArr_[0].ToString());
        Debug.Log(nowWearingArr_[1].ToString());
    }
} // end of class
