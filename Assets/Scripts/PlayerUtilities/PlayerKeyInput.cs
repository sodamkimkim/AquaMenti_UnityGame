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
    [SerializeField]
    private PlayerFocusManager playerFocusManager_ = null;
    [SerializeField]
    private MagicManager magicManager_=null;
    [SerializeField]
    private InventoryManager inventoryManager_ = null;
    private void Awake()
    {
        playerMovement_ = GetComponent<PlayerMovement>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement_.Jump();
        }
        // focus Center
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerFocusManager_.isFocusFixed_ == true) { playerFocusManager_.isFocusFixed_ = false; Debug.Log("isFocusFixed_ = false"); }
           else if (playerFocusManager_.isFocusFixed_ == false) { playerFocusManager_.isFocusFixed_ = true; Debug.Log("isFocusFixed_ = true"); }
        }
        // �������� Rotate
        if(Input.GetKeyDown(KeyCode.R))
        {
            magicManager_.RotateWaterMagic();
        }
        // inventory on / off
        if(Input.GetKeyDown(KeyCode.I))
        {

        }
    }
} // end of class
