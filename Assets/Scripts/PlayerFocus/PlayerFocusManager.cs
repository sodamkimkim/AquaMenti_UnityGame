using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager_ = null;
    [SerializeField]
    private GameObject playerGo_ = null;
    private Staff staff_ = null;
    // ������ Raycast���� �ʵ�
    private WandRaySpawner wandRaySpawner_;
    // Player Rotate & Look ���� �ʵ�
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;

    [SerializeField]
    private ScreenSideManager screenSideManager_;

    private MagicRotate magicRotate_;
    // Flag
    public bool isFocusFixed_ { get; set; }


    private void Awake()
    {
        wandRaySpawner_ = playerGo_.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo_.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo_.GetComponentInChildren<UpperBodyLook>();
        //screenSideManager_ = GameObject.FindWithTag("Canvas_ScreenSide").GetComponent<ScreenSideManager>();

        isFocusFixed_ = true;

    }
    public void SetStaff(Staff _staff)
    {
        staff_ = _staff;
        magicRotate_ = playerGo_.GetComponentInChildren<MagicRotate>();
    }
    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        // # FocusFixed ��� or FocusMove ���
        // # FocusFixed ��� or FocusMove ���
        if (isFocusFixed_)
        { // # FocusFixed ���
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY(true);
            upperBodyLook_.RotateUpperBodyAxisX(true);

            if (staff_ != null)
            {
                staff_.LookAtCenter();
            }
        }
        else if (!isFocusFixed_)
        { // # FocusMove ���
            wandRaySpawner_.RayMoveFocusShot();
            // # ScreenSide MouseOver üũ
            PlayerLookControlWhenScreenSideMouseHover();
          
            if (staff_ != null)
            {
                staff_.Move(wandRaySpawner_.GetCenterRayDir());
            }
        }

    }
    /// <summary>
    /// ScreenSide�� Mouse hover �Ǹ� RotateBodyAxisY of RotateUpperBodyAxisX
    /// </summary>
    private void PlayerLookControlWhenScreenSideMouseHover()
    {
        // # TopSide ��ġ : -x�� ȸ��
        if (screenSideManager_.isScreenSideTop) upperBodyLook_.RotateUpperBodyUP(true);
        else upperBodyLook_.RotateUpperBodyUP(false);

        // # BottomSide ��ġ : x�� ȸ��
        if (screenSideManager_.isScreenSideBottom) upperBodyLook_.RotateUpperBodyDown(true);
        else upperBodyLook_.RotateUpperBodyDown(false);

        // # LeftSide ��ġ : -Y�� ȸ��
        if (screenSideManager_.isScreenSideLeft) playerYRotate_.RotateBodyAxisYLeft(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);

        // # RightSide ��ġ : Y�� ȸ��
        if (screenSideManager_.isScreenSideRight) playerYRotate_.RotateBodyAxisYRight(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);
    }


    public void RotateWaterMagic()
    {
        magicRotate_.RotateWaterMagic();
        wandRaySpawner_.RotateFocusPointUI();
    }

} // end of class
