using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    // ������ Raycast���� �ʵ�
    private WandRaySpawner wandRaySpawner_;

    // Player Rotate & Look ���� �ʵ�
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;

    // ScreenSideManager
    private ScreenSideManager screenSideManager_;

    // Flag
    public bool isFocusFixed_ { get; set; }

    private void Awake()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        wandRaySpawner_ = playerGo.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo.GetComponentInChildren<UpperBodyLook>();
        screenSideManager_ = GameObject.FindWithTag("Canvas_ScreenSide").GetComponent<ScreenSideManager>();

        isFocusFixed_ = true;
 
    }
    private void Update()
    {
        // # FocusFixed ��� or FocusMove ���
        if (isFocusFixed_)
        { // # FocusFixed ���
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY(true);
            upperBodyLook_.RotateUpperBodyAxisX(true);
        }
        else if (!isFocusFixed_)
        { // # FocusMove ���
            wandRaySpawner_.RayMoveFocusShot();
            // # ScreenSide MouseOver üũ
            PlayerLookControlWhenScreenSideMouseHover();
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

        // # Bottom ��ġ : x�� ȸ��
        if (screenSideManager_.isScreenSideBottom) upperBodyLook_.RotateUpperBodyDown(true);
        else upperBodyLook_.RotateUpperBodyDown(false);

        // # LeftSide ��ġ : -Y�� ȸ��
        if (screenSideManager_.isScreenSideLeft) playerYRotate_.RotateBodyAxisYLeft(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);

        // # RightSide ��ġ : Y�� ȸ��
        if (screenSideManager_.isScreenSideRight) playerYRotate_.RotateBodyAxisYRight(true);
        else playerYRotate_.RotateBodyAxisYLeft(false);
    }
   
} // end of class
