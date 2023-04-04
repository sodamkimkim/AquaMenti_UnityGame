using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFocusManager : MonoBehaviour
{
    // ������ Raycast���� �ʵ�
    private WandRaySpawner wandRaySpawner_;

    // Player Rotate & Look ���� �ʵ�
    private PlayerYRotate playerYRotate_;
    private UpperBodyLook upperBodyLook_;
    public bool isFocusFixed_ { get; set; }

    private void Awake()
    {
        GameObject playerGo = GameObject.FindWithTag("Player");
        wandRaySpawner_ = playerGo.GetComponentInChildren<WandRaySpawner>();
        playerYRotate_ = playerGo.GetComponentInChildren<PlayerYRotate>();
        upperBodyLook_ = playerGo.GetComponentInChildren<UpperBodyLook>();
        isFocusFixed_ = true;
    }
    private void Update()
    {
        // # FocusFixed ��� or FocusMove ���
        if (isFocusFixed_)
        { // # FocusFixed ���
            wandRaySpawner_.RayScreenCenterShot();
            playerYRotate_.RotateBodyAxisY();
            upperBodyLook_.RotateUpperBodyAxisX();
        }
        else if (!isFocusFixed_)
        { // # FocusMove ���
            wandRaySpawner_.RayMoveFocusShot();
        }
    }
} // end of class
