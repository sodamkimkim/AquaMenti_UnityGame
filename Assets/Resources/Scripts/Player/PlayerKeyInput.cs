using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �׻� ��ȿ�� Ű�� �� Ŭ������ ����.
/// flag������ ��ȿ�� ���ΰ� �ٲ�� Ű�� ��Ȳ�� ���� ������ ��ġ�� ����
/// </summary>
public class PlayerKeyInput : MonoBehaviour
{
    private PlayerMovement playerMovement_;
    private WandRaySpawner wandRaySpawner_;

    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
        wandRaySpawner_ = GetComponentInChildren<WandRaySpawner>();
    }
    private void Update()
    {
        // walk���� �϶� �޸� �� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement_.isLeftShiftKeyInput = true;
        }
        else
        {
            playerMovement_.isLeftShiftKeyInput = false;
        }
        // move forward
        if (Input.GetKey(KeyCode.W))
        {
            playerMovement_.Walk(playerMovement_.GetPlayerTransform().forward);
        }
        // move backWard
        if (Input.GetKey(KeyCode.S))
        {
            playerMovement_.Walk(-playerMovement_.GetPlayerTransform().forward);
        }
        // move left
        if (Input.GetKey(KeyCode.A))
        {
            playerMovement_.Walk(-playerMovement_.GetPlayerTransform().right);
        }
        // move right
        if (Input.GetKey(KeyCode.D))
        {
            playerMovement_.Walk(playerMovement_.GetPlayerTransform().right);
        }
        // jump
        if (Input.GetKey(KeyCode.Space))
        {
            playerMovement_.Jump();
        }
        // focus Center
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (wandRaySpawner_.isCenterFocus_ == true) { wandRaySpawner_.isCenterFocus_ = false; Debug.Log("IsCenterFocus_ = false"); }
           else if (wandRaySpawner_.isCenterFocus_ == false) { wandRaySpawner_.isCenterFocus_ = true; Debug.Log("IsCenterFocus_ = true"); }
        }
    }
} // end of class
