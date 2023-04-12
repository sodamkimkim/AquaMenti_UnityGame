using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// csv���� ���Ӽ��� ��� ������ ���� �ҷ��� ������ ���� Ŭ����
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList_ = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> itemStaffInfoList_ = new List<Dictionary<string, object>>();
    private List<Dictionary<string, object>> itemSpellInfoList_ = new List<Dictionary<string, object>>();
    public enum EItemCategory { Staff, Spell, Len }
    public enum EStaffName { AmberStaff, RubyStaff, Len }
    public enum ESpellName
    {
        Deg0MagicSpell,
        Deg15MagicSpell,
        Deg25MagicSpell,
        Deg45MagicSpell
        , Len
    }
    public Sprite[] itemImgSpriteArr_ = null;
    private void Awake()
    {
        CSVReader.Read("Datas/GameInfo/ItemInfo", out itemInfoList_);
        GetAllStaffInfo();
        GetAllSpellInfo();
    }
    private void Start()
    {
        //GetAllItemInfo();
        // SearchItembyItemName("AmberStaff");
        itemImgSpriteArr_ = Resources.LoadAll<Sprite>("Textures\\UI\\Items");
    }

    /// <summary>
    /// ��� �ΰ��� ������ ���� ��������
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList_[i]["ItemCategory"].ToString()}" +
                $"/ ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
        }
    }

    public void SearchItembyItemName(string _itemName)
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemName"].ToString().Equals(_itemName))
            {
                Debug.Log($"ItemName���� ã��: ItemName : {itemInfoList_[i]["ItemName"].ToString()}");
            }
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == Staff ������ ���� ��������
    /// </summary>
    private void GetAllStaffInfo()
    {
        itemStaffInfoList_.Clear();
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                itemStaffInfoList_.Add(itemInfoList_[i]);
            }
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == Spell ������ ���� ��������
    /// </summary>
    private void GetAllSpellInfo()
    {
        itemSpellInfoList_.Clear();
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Spell.ToString())
            {
                itemSpellInfoList_.Add(itemInfoList_[i]);
            }
        }
    }
    /// <summary>
    /// ��� staff ���� �����ͼ� UI�����ϱ�
    /// </summary>
    public void SetItemStaffUIList(out List<Dictionary<string, object>> _itemStaffInfoList)
    {

        _itemStaffInfoList = itemStaffInfoList_;
    }
    /// <summary>
    /// ��� spell ���� �����ͼ� UI�����ϱ�
    /// </summary>
    public void SetItemSpellUIList(out List<Dictionary<string, object>> _itemSpellInfoList)
    {
        _itemSpellInfoList = itemSpellInfoList_;
    }
    /// <summary>
    /// file name ���� Item�̹��� ã��
    /// </summary>
    public Sprite SearchItemImg(string _imgFileName)
    {
        Sprite defaultSprite = itemImgSpriteArr_[0];
        foreach (Sprite sprite in itemImgSpriteArr_)
        {
  
            if (sprite.name == _imgFileName)
            {
                return sprite;
            }
        }
        return defaultSprite;
    }

} // end of class
