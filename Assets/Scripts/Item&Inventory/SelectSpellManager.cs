using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSpellManager : MonoBehaviour
{
    [Header("Aggregation")]

    private ItemInfo[] itemArr = null;

    public delegate void OpenInvenUICallback_();
    public delegate void CloseInvenUICallback_(NowWearingInfo.NowWearingItem _selectItem);

    private OpenInvenUICallback_ openCallback_;
    private CloseInvenUICallback_ closeCallback_;

    private bool isSelectSpellUIOpen_ { get; set; }

    private void Awake()
    {
        isSelectSpellUIOpen_ = false;

    }
    private void Start()
    {


    }
    public void Init(OpenInvenUICallback_ _OpenCallback, CloseInvenUICallback_ _CloseCallback_)
    {
        openCallback_ += _OpenCallback;
        closeCallback_ += _CloseCallback_;


    }
    /// <summary>
    /// inventory UI에서 CurrentSpell버튼 누르면 실행되는 콜백
    /// </summary>
    public void OnCurrentSpellClickCallback()
    {
        openCallback_.Invoke();
        this.gameObject.SetActive(true);
        isSelectSpellUIOpen_ = true;
        itemArr = GetComponentsInChildren<ItemInfo>();
        foreach (ItemInfo item in itemArr)
        {
            item.Init(SelectSpellItemCallback);
        }
    }
    /// <summary>
    /// : selectSpell UI에서 item 누르면 실행되는 콜백
    ///  1. 클릭한 item 정보 NowWearing에 뿌려주고
    ///  2. selectSpell UI 꺼줌
    /// </summary>
    private void SelectSpellItemCallback(NowWearingInfo.NowWearingItem _selectItem)
    {

        // 상위매니저에서 받은 콜백함수 실행
        closeCallback_.Invoke(_selectItem);
        this.gameObject.SetActive(false);
        isSelectSpellUIOpen_ = false;
    }
} // end of class
