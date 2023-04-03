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
    private WandRaySpawner magicSpawner_;

    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
        magicSpawner_ = GetComponentInChildren<WandRaySpawner>();
    }
    private void FixedUpdate()
    {
        // walk���� �϶� �޸� �� ����
        if(Input.GetKey(KeyCode.LeftShift))
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
        // ��ٸ� ���
        if(Input.GetKey(KeyCode.F))
        {
            // ray�� ��ٸ��� �浹������ �ش� ��ٸ��� pos�� �ٲ���.

        }
    }
} // end of class
