using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField]
    private MeshPaintBrush meshPaintBrush_ = null;
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

            if (inventoryManager_.isInventoryPanOpen_ ==false) { inventoryManager_.OpenInventoryPan(); Debug.Log("InventoryPan open"); }
            else if (inventoryManager_.isInventoryPanOpen_ ==true) { inventoryManager_.CloseInventoryPan(); Debug.Log("InventoryPan close"); }

        }
        // # -ȫ��-
        if (Input.GetMouseButton(0))
        {
            //IsPainting(true);
            //PaintToTarget();
            meshPaintBrush_.TimingDraw();
            Debug.Log("���콺 ��Ŭ��");
        }
        else if (meshPaintBrush_.IsPainting() == true)
        {
            meshPaintBrush_.IsPainting(false);
            meshPaintBrush_.StopCheckTargetProcess();
            meshPaintBrush_.StopTimingDraw();
            Debug.Log("���콺 ��Ŭ�� ����");
        }
        //else if (drawCoroutine == true)
        //{
        //    StopTimingDraw();
        //    Debug.Log("�� �� ȣ��ǳ���?");
        //}

        // �ӽ� ���� ���� //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.Zero;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.One;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            meshPaintBrush_.stick.magicType = MeshPaintBrush.MagicType.Two;
        }
        // End �ӽ� ���� ���� //

        // Utility
        // Dirty ��� ���� (���� %�� �����ϸ� ����� �κ�(������ ���ϴ��))
        if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.E))
        {
            meshPaintBrush_.GetTarget().ClearTexture();
        }
        // Dirty �ʱ�ȭ (�ʱ�ȭ ��ư�� �����ٸ� ������ �κ�(������ ���ϴ��))
        if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.R))
        {
            meshPaintBrush_.GetTarget().ResetTexture();
        }
        if (meshPaintBrush_.GetTarget() != null && meshPaintBrush_.GetTarget().IsDrawable() && Input.GetKeyDown(KeyCode.T))
        {
            meshPaintBrush_.GetTarget().CompleteTwinkle();
        }


    }
} // end of class
