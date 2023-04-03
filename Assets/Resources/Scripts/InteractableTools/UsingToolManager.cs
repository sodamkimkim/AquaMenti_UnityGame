using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingToolManager : MonoBehaviour
{
    private WandRaySpawner wandRaySpawner_;
    private Ladder ladder_;

    private void Awake()
    {
        wandRaySpawner_ = GameObject.FindWithTag("Player").GetComponentInChildren<WandRaySpawner>();
        ladder_ = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
    }
    private void Update()
    {
        Vector3 rayHitPos_ = wandRaySpawner_.GetHitPos();
        if (wandRaySpawner_.isLadder() && Input.GetKey(KeyCode.F)) ladder_.isMoveable_ = true;

        if (ladder_.isMoveable_) ladder_.SetLadderPos(rayHitPos_);
        // ray �� floor�� �浹 && MouseButtonDown(0) => floor�� ��ٸ� ���� �� ����.
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newLadderPos = rayHitPos_;
            ladder_.SetLadderPos(newLadderPos);
            ladder_.isMoveable_ = false;
        }


    }
} // end of class
