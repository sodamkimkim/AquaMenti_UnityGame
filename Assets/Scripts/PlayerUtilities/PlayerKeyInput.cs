using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private InventoryManager inventoryManager_ = null;
    [SerializeField]
    private WandRaySpawner wandRaySpawner_ = null;
    [SerializeField]
    private GameManager gameManager_ = null;
    [SerializeField]
    private UsingToolManager usingToolManager_ = null;
    [SerializeField]
    private UI_Manager uiManager_ = null;
    private WaterPumpActivator nowWaterPumpActivator_ = null;

/*    private PlayerYRotate playerYRotate_ = null;*/

    public bool useKey { get; set; }
    private bool useWand { get; set; }
    private bool isOutGameUIOpen { get; set; }
    private bool isInventoryUIOpen { get; set; }

    [SerializeField]
    private InGameAllItemInfo inGameAllItemInfo_ = null;
    private List<NowWearingInfo.NowWearingItem> spellList_ = new List<NowWearingInfo.NowWearingItem>();
    private PlayerAnimation playerAnimation_ = null;
    private int spellIdx_ = 0;
    private void Awake()
    {
        //  inventoryManager_ = GetComponent<InventoryManager>();
        playerMovement_ = GetComponent<PlayerMovement>();
        wandRaySpawner_ = GetComponentInChildren<WandRaySpawner>();

        useKey = true;
        isOutGameUIOpen = false;
        isInventoryUIOpen = false;

        spellList_.Clear();
        playerAnimation_ = GetComponent<PlayerAnimation>();

    }
    private void Start()
    {
        inGameAllItemInfo_.GetSpellItemList(out spellList_);
    }
    private void Update()
    {
        // Block Input Key
        if (useKey == false) return;

        playerAnimation_.IsWalk(false);
        // OutGameUI on / off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager_.isStartGame_ && gameManager_.isInGame_)
            {
                gameManager_.ActiveOutGameUi();
                isOutGameUIOpen = true;
            }
            else if (gameManager_.isStartGame_ && !gameManager_.isInGame_)
            {
                gameManager_.ActiveInGameUi();
                isOutGameUIOpen = false;
            }
            gameManager_.MouseLock(!isOutGameUIOpen, isOutGameUIOpen);
        }
        if (!gameManager_.isInGame_) return;
        // walk���� �϶� �޸� �� ����


        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMovement_.isLeftShiftKeyInput_ = true;
        }
        else
        {
            playerMovement_.isLeftShiftKeyInput_ = false;
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement_.Jump();
        }
        // move
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");
        if (axisH != 0 || axisV != 0)
        {
            playerAnimation_.IsWalk(true);
        }
        playerMovement_.Walk(new Vector3(axisH, 0f, axisV));
        // focus Center
        if (Input.GetKeyDown(KeyCode.C) && !playerFocusManager_.isInventoryOpen_)
        {
            if (playerFocusManager_.isFocusFixed_ == true) { playerFocusManager_.isFocusFixed_ = false; Debug.Log("isFocusFixed_ = false"); }
            else if (playerFocusManager_.isFocusFixed_ == false) { playerFocusManager_.isFocusFixed_ = true; Debug.Log("isFocusFixed_ = true"); }
            gameManager_.MouseLock(playerFocusManager_.isFocusFixed_, false);
        }
        // �������� Rotate
        if (Input.GetKeyDown(KeyCode.R) && !playerFocusManager_.isInventoryOpen_)
        {
            playerFocusManager_.RotateWaterMagic();
            nowWaterPumpActivator_.RotateParticle();
        }
        // inventory on / off
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryManager_.isInventoryPanOpen_ == false)
            {
                playerFocusManager_.isInventoryOpen_ = true;
                inventoryManager_.OpenInventoryPan();
                isInventoryUIOpen = true;
            }
            else if (inventoryManager_.isInventoryPanOpen_ == true)
            {
                gameManager_.isInGame_ = true;
                inventoryManager_.CloseInventoryPan();
                isInventoryUIOpen = false;
                playerFocusManager_.isInventoryOpen_ = false;
            }
            gameManager_.MouseLock(!isInventoryUIOpen, isInventoryUIOpen);
        }
        /*        if (!inventoryManager_.isInventoryPanOpen_)
                {
                    Cursor.visible = false;                     //���콺 Ŀ���� ������ �ʰ� ��
                }
                else
                {
                    Cursor.visible = true;                     //���콺 Ŀ���� ������ �ʰ� ��
                }*/
        if (!isInventoryUIOpen && !usingToolManager_.IsLadderMoveable())
        {
            nowWaterPumpActivator_ = inventoryManager_.GetWaterPumpActivator();
            if (Input.GetMouseButtonDown(0))
            {
                gameManager_.MouseLock(playerFocusManager_.isFocusFixed_ || isOutGameUIOpen, isInventoryUIOpen || isOutGameUIOpen);
                nowWaterPumpActivator_.PlayPump(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                nowWaterPumpActivator_.PlayPump(false);
            }

            // # -ȫ��-
            if (Input.GetMouseButton(0))
            {

                useWand = true;
                wandRaySpawner_.RaysTimingDraw();
                // Debug.Log(isInventoryUIOpen.ToString() + isOutGameUIOpen.ToString() + "���콺 ��Ŭ��");


            }
            else if (wandRaySpawner_.RaysIsPainting() == true)
            {

                wandRaySpawner_.RaysIsPainting(false);
                wandRaySpawner_.RaysStopCheckTargetProcess();
                wandRaySpawner_.RaysStopTimingDrow();
                //Debug.Log("���콺 ��Ŭ�� ����");
            }
            else if (useWand == true)
            {
                useWand = false;
                wandRaySpawner_.RaysStopTimingDrow();
            }
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Debug.Log(scroll);
            spellIdx_ += Mathf.RoundToInt(scroll * 10);

            if (spellIdx_ > spellList_.Count - 1)
            {
                spellIdx_ = 0;
            }
            else if (spellIdx_ < 0)
            {
                spellIdx_ = spellList_.Count - 1;
            }
            Debug.Log(spellIdx_);
            inventoryManager_.SelectItem(spellList_[spellIdx_]);
            if (spellIdx_ == 0)
            {
                nowWaterPumpActivator_.AngleParticle(0f);
            }
            else if (spellIdx_ == 1)
            {
                nowWaterPumpActivator_.AngleParticle(15f);
            }
            else if (spellIdx_ == 2)
            {
                nowWaterPumpActivator_.AngleParticle(25f);
            }
            else if (spellIdx_ == 3)
            {
                nowWaterPumpActivator_.AngleParticle(45f);
            }
            foreach (var spell in spellList_)
            {
                Debug.Log(spell.ToString());
            }
            //   wandRaySpawner_.rayAngle_ += scroll * angleIncrement;
            //  sprayAngle = Mathf.Clamp(sprayAngle, minAngle, maxAngle);
        }

        // KeyHelper
        if (Input.GetKeyDown(KeyCode.K))
        {
            uiManager_.KeyHelper();
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
