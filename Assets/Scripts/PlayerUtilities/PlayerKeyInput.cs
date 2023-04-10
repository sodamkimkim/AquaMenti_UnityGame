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
    private InventoryManager inventoryManager_ = null;
    [SerializeField]
    private WandRaySpawner wandRaySpawner_ = null;
    private bool useWand { get; set; }
    private void Awake()
    {
        inventoryManager_ = GetComponent<InventoryManager>();
        playerMovement_ = GetComponent<PlayerMovement>();
        wandRaySpawner_ = GetComponentInChildren<WandRaySpawner>();
    }
    private void Update()
    {
        // walk���� �϶� �޸� �� ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement_.isLeftShiftKeyInput_ = true;
        }
        else
        {
            playerMovement_.isLeftShiftKeyInput_ = false;
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
            playerFocusManager_.RotateWaterMagic();
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

            useWand = true;
            wandRaySpawner_.RaysTimingDraw();
            Debug.Log("���콺 ��Ŭ��");
        }
        else if (wandRaySpawner_.RaysIsPainting() == true)
        {
            wandRaySpawner_.RaysIsPainting(false);
            wandRaySpawner_.RaysStopCheckTargetProcess();
            wandRaySpawner_.RaysStopTimingDrow();
            Debug.Log("���콺 ��Ŭ�� ����");
        }
        else if (useWand == true)
        {
            useWand = false;
            wandRaySpawner_.RaysStopTimingDrow();
        }

        //// �ӽ� ���� ���� //
        //if (Input.GetKeyDown(KeyCode.Alpha1)) // 0�� ����
        //{
        //    meshPaintBrush_.stick.magicType = MeshPaintBrush.EMagicType.Zero;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2)) // 15�� ����
        //{
        //    meshPaintBrush_.stick.magicType = MeshPaintBrush.EMagicType.One;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3)) // 40�� ����
        //{
        //    meshPaintBrush_.stick.magicType = MeshPaintBrush.EMagicType.Two;
        //}
        //// End �ӽ� ���� ���� //

        // Utility
        // Dirty ��� ���� (���� %�� �����ϸ� ����� �κ�(������ ���ϴ��))
        //if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.E))
        //{
        //    meshPaintBrush_.GetTarget().ClearTexture();
        //}
        //// Dirty �ʱ�ȭ (�ʱ�ȭ ��ư�� �����ٸ� ������ �κ�(������ ���ϴ��))
        //if (meshPaintBrush_.GetTarget() != null && Input.GetKeyDown(KeyCode.R))
        //{
        //    meshPaintBrush_.GetTarget().ResetTexture();
        //}
        //if (meshPaintBrush_.GetTarget() != null && meshPaintBrush_.GetTarget().IsDrawable() && Input.GetKeyDown(KeyCode.T))
        //{
        //    meshPaintBrush_.GetTarget().CompleteTwinkle();
        //}


    }
} // end of class
