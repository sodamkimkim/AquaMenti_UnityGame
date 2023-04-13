using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNamingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject map1_ = null;
    [SerializeField]
    private GameObject[] map1WorkSectionsArr_ = null;
    //[SerializeField]
    //private GameObject map2_ = null;
    ////[SerializeField]
    ////private GameObject[] map2WorkSectionsArr_ = null;

    private MeshPaintTarget[] meshPaintTarget_m1__w1 = null;
    private MeshPaintTarget[] meshPaintTarget_m1__w2 = null;

    private List<Dictionary<string, object>> doubleNameTargetDicList_ = new List<Dictionary<string, object>>();
    private void Awake()
    {
        meshPaintTarget_m1__w1 = map1WorkSectionsArr_[0].gameObject.GetComponentsInChildren<MeshPaintTarget>();
        meshPaintTarget_m1__w2 = map1WorkSectionsArr_[1].gameObject.GetComponentsInChildren<MeshPaintTarget>();

        ChangeTargetName_m1__w1();
    }
    private void ChangeTargetName_m1__w1()
    {
        foreach (MeshPaintTarget mpt in meshPaintTarget_m1__w1)
        {
            // Debug.Log(mpt.gameObject.name);
            string prevName = mpt.gameObject.name;
            mpt.gameObject.name = prevName + "_1_1";

        }
        //for (int i = 0; i < meshPaintTarget_m1__w1.Length; i++)
        //{
        //    if (CheckDoubleName(meshPaintTarget_m1__w1[i].gameObject.name, meshPaintTarget_m1__w1) > 1)
        //    {
        //        // 1. doubleName �̸� �̸����� ���� ��������. Dictionary<string, object>
        //        // 2. ����Ȱ� ���� �ҷ��� ��ȣ �ٿ�����.  - string ���� key search �ؼ� object ��ȣ �ٿ��ֱ�.
        //        string prevName = meshPaintTarget_m1__w1[i].gameObject.name;
        //        Dictionary<string, object> dic = new Dictionary<string, object>();
        //        dic.Add(prevName, meshPaintTarget_m1__w1[i]);

        //        doubleNameTargetDicList_.Add(dic);
        //        // meshPaintTarget_m1__w1[i].gameObject.name = prevName + "_" + i;
        //        PrintAllDicList(prevName);
        //    }
        //}

        // # dic Ű�� ��ȸ�ؼ� object ���� �ٲ��ֱ�
    }
    private void PrintAllDicList(string _name)
    {
        for (int i = 0; i < doubleNameTargetDicList_.Count; i++)
        {

            Debug.Log(doubleNameTargetDicList_[i].ContainsKey(_name));

        }
    }
    private void ChangeDoubleName(string _prevName)
    {
        for (int i = 0; i < doubleNameTargetDicList_.Count; i++)
        {
            //CheckDoubleName(doubleNameTargetDicList_[i])
            if (doubleNameTargetDicList_[i].ContainsKey(_prevName))
            {
                Debug.Log("???" + doubleNameTargetDicList_[i][_prevName]);
            }
            else
            {
                Debug.Log(_prevName + " -> �ش��ϴ� �ߺ� �̸� ����");

            }
        }
    }

    private int CheckDoubleName(string _name, MeshPaintTarget[] _worksectionTargetsArr)
    {
        int nameCount = 0;
        foreach (var item in _worksectionTargetsArr)
        {
            if (_name == item.gameObject.name)
            {
                nameCount += 1;
            }
        }
        return nameCount;
    }
} // end of class
