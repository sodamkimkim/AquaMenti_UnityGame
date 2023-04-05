using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// csv���� ���Ӽ��� ��� ������ ���� �ҷ��� ������ ���� Ŭ����
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList_ = new List<Dictionary<string, object>>();
    public enum EItemCategory { Staff, MagicSpell, Len}

    private void Start()
    {
        itemInfoList_ = CSVReader.Read("Datas/GameInfo/ItemInfo");
        GetAllItemInfo();
    }

    /// <summary>
    /// ��� �ΰ��� ������ ���� ��������
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList_.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList_[i]["ItemCategory"].ToString()}"+
                $"/ ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == MagicSpell ������ ���� ��������
    /// </summary>
    public void GetAllStaffInfo()
    {
        for (int i = 0; i<itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
            }
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == MagicSpell ������ ���� ��������
    /// </summary>
    public void GetAllMagicSpellInfo()
    {
        for(int i = 0; i<itemInfoList_.Count; i++)
        {
            if (itemInfoList_[i]["ItemCategory"].ToString() == EItemCategory.MagicSpell.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList_[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList_[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList_[i]["Status"].ToString()}");
            }
        }
    }

} // end of class
