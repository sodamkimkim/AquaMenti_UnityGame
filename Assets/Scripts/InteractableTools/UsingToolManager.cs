using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsingToolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTipUIGo_ = null;
    private TextMeshProUGUI toolTipTMPro_ = null;
    private WandRaySpawner wandRaySpawner_;
    private Ladder ladder_;
    private bool isLadderMoving_ { get; set; }

    private void Awake()
    {
        wandRaySpawner_ = GameObject.FindWithTag("Player").GetComponentInChildren<WandRaySpawner>();
        ladder_ = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
        toolTipTMPro_ = toolTipUIGo_.GetComponentInChildren<TextMeshProUGUI>();
        isLadderMoving_ = false;
    }
    private void Update()
    {
        Vector3 rayHitPos_ = wandRaySpawner_.hitPos_;
        if (wandRaySpawner_.isLadder_)
        {
            toolTipTMPro_.text = "��ٸ� ����� ���Ͻø� Ű���� F�� ��������.";
            toolTipUIGo_.SetActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                ladder_.isMoveable_ = true;
                toolTipUIGo_.SetActive(false);
            }
        }
        else { toolTipUIGo_.SetActive(false); }

        if (ladder_.isMoveable_)
        {
            toolTipUIGo_.SetActive(true);
            toolTipTMPro_.text = "��ٸ� ��ġ�� �����Ϸ��� ���콺 ���� ��ư�� ��������.";
            ladder_.SetLadderPos(new Vector3(rayHitPos_.x, ladder_.GetPos().y, rayHitPos_.z));
            // ray �� floor�� �浹 && MouseButtonDown(0) => floor�� ��ٸ� ���� �� ����.
            if (Input.GetMouseButtonDown(0))
            {
                ladder_.isMoveable_ = false;
                toolTipTMPro_.text = "";
                toolTipUIGo_.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ladder_.RotateLadderLeft();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                ladder_.RotateLadderRight();
            }
        }


    }
} // end of class
