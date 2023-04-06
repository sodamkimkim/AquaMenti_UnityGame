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
        public InGameAllItemInfo.EItemCategory itemCategory_;
        public string itemName_;
        public bool isUnLock_; // true: ��� ���� false: ���Ұ���(���)
        public bool isNowWearing_; // true: ���� ��, false: ���� x
    } // end of class
} // end of namespace