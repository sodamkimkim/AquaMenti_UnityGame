using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// csv���� ���Ӽ��� ��� ������ ���� �ҷ��� ������ ���� Ŭ����
/// </summary>
public class InGameAllItemInfo : MonoBehaviour
{
    private List<Dictionary<string, object>> itemInfoList = new List<Dictionary<string, object>>();
    public enum EItemCategory { Staff, MagicSpell, Len}

    private void Start()
    {
        itemInfoList = CSVReader.Read("Datas/GameInfo/ItemInfo");
        GetAllItemInfo();
    }

    /// <summary>
    /// ��� �ΰ��� ������ ���� ��������
    /// </summary>
    public void GetAllItemInfo()
    {
        for (int i = 0; i < itemInfoList.Count; i++)
        {
            Debug.Log($"ItemCategory : {itemInfoList[i]["ItemCategory"].ToString()}"+
                $"/ ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                $"/ Status : {itemInfoList[i]["Status"].ToString()}");
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == MagicSpell ������ ���� ��������
    /// </summary>
    public void GetAllStaffInfo()
    {
        for (int i = 0; i<itemInfoList.Count; i++)
        {
            if (itemInfoList[i]["ItemCategory"].ToString() == EItemCategory.Staff.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList[i]["Status"].ToString()}");
            }
        }
    }
    /// <summary>
    /// ��� �ΰ��� EItemCategory == MagicSpell ������ ���� ��������
    /// </summary>
    public void GetAllMagicSpellInfo()
    {
        for(int i = 0; i<itemInfoList.Count; i++)
        {
            if (itemInfoList[i]["ItemCategory"].ToString() == EItemCategory.MagicSpell.ToString())
            {
                Debug.Log($"ItemName : {itemInfoList[i]["ItemName"].ToString()}" +
                    $"/ Description : {itemInfoList[i]["Description"].ToString()}" +
                    $"/ Status : {itemInfoList[i]["Status"].ToString()}");
            }
        }
    }

} // end of class
