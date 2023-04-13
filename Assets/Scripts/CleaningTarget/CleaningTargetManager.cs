using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// mainRay�� �浹�� cleaning target object�̸� �����ͼ�
/// ingame ui�� �̸�, û�� ���»ѷ��ְ�
/// �������� �̸�, û�һ���, ����, ȹ�� �� ����ؼ� ���̶� �� ���� ������ dataŬ������ �ѷ���
/// </summary>
public class CleaningTargetManager : MonoBehaviour
{
    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo = null;
    [SerializeField]
    private GameObject playerGo_ = null;
    private WandRaySpawner wandRaySpawner_ = null;

    [SerializeField]
    private GameObject washInfoGO_ = null;

    private ObjectNameUI objectNameUI_ = null;
    private CleaningProgressPanUI cleaningProgressPanUI = null;
    private CleaningPercentageUI cleaningPercentageUI = null;
    //private 
    private void Awake()
    {
        wandRaySpawner_ = playerGo_.GetComponentInChildren<WandRaySpawner>();

        // # UI
        objectNameUI_ = washInfoGO_.GetComponentInChildren<ObjectNameUI>();
        cleaningProgressPanUI = washInfoGO_.GetComponentInChildren<CleaningProgressPanUI>();
        cleaningPercentageUI = washInfoGO_.GetComponentInChildren<CleaningPercentageUI>();
    }
    // # wandrayspawner�� targetname_�� �����ͼ�, 
    // �̸�, û�һ��� �ѷ��ְ�, �ƴϸ� �̸� : "", percentage "", progress : 0
    private void Update()
    {
        SetCleaningTargetStatusUI();

    }
    private void SetCleaningTargetStatusUI()
    {
    
        // ex) object Name: Barrel_1_1
        string[] targetNameSplitArr = wandRaySpawner_.cleaningTargetName_.Split("_");
        if (wandRaySpawner_.cleaningTargetName_ != "")
        {

            objectNameUI_.SetObjectName(targetNameSplitArr[0]);
            cleaningProgressPanUI.SetCleaningProgressImgFillAmt(0.5f);
            cleaningPercentageUI.SetCleaningPercentageUI(Mathf.Round(0.776345f*100)*0.01f);
            cleaningPercentageUI.SetActive(true);
        }
        else
        {
            objectNameUI_.SetObjectName(wandRaySpawner_.cleaningTargetName_);
            cleaningProgressPanUI.SetCleaningProgressImgFillAmt(0f);
            cleaningPercentageUI.SetCleaningPercentageUI(100f);
            cleaningPercentageUI.SetActive(false);
        }
    }
} // end of class
