using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingToolManager : MonoBehaviour
{
    private WandRaySpawner wandRaySpawner_;
    private Ladder ladder_;
    private CheckLadderOnGround checkLadderOnGround_;

    private void Awake()
    {
        wandRaySpawner_ = GameObject.FindWithTag("Player").GetComponentInChildren<WandRaySpawner>();
        ladder_ = GameObject.FindWithTag("Ladder").GetComponent<Ladder>();
        checkLadderOnGround_ = ladder_.gameObject.GetComponentInChildren<CheckLadderOnGround>();
    }
    private void Update()
    {
        Vector3 rayHitPos_ = wandRaySpawner_.GetHitPos();
        if (wandRaySpawner_.isLadder() && Input.GetKey(KeyCode.F)) ladder_.isMoveable_ = true;

        if (ladder_.isMoveable_)
        { 
            ladder_.SetLadderPos(new Vector3(rayHitPos_.x, ladder_.GetPos().y, rayHitPos_.z));
            // ray �� floor�� �浹 && MouseButtonDown(0) => floor�� ��ٸ� ���� �� ����.
            if (Input.GetMouseButtonDown(0))
            {
                ladder_.isMoveable_ = false;
            }
            if(Input.GetKeyDown(KeyCode.Z))
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
