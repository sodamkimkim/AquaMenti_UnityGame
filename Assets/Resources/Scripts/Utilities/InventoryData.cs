using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    ///  : player�� ���� ��� ������ ������ �����ϴ� Ŭ����
    ///  - ���� ���� ��(NowWearing)�� ������ ������ ���ԵǾ� ����.
    ///  - save��
    /// </summary>
    public class InventoryData : MonoBehaviour
    {
        public InGameAllItemInfo.EItemCategory itemCategory;
        public string itemName;
        public bool isUnLock; // true: ��� ���� false: ���Ұ���(���)
        public bool isNowWearing; // true: ���� ��, false: ���� x
    } // end of class
} // end of namespace