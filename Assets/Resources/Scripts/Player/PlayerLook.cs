using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float screenOffset_ = 100;
    private float offsetAngle_ = 10f;

    private void Update()
    {
        LookAtMouseCursor();
    }
    public void LookAtMouseCursor()
    {
        /*
         ���콺 �������� ��ũ���� �����¿������� ������ 
        �÷��̾� �ޱ� ���ݾ� ������
         */
        //float xLeft = screenOffset_;
        //float xRight = Screen.width - screenOffset_;
        //float yTop = Screen.height - screenOffset_;
        //float yBottom = screenOffset_;

        //Vector3 mousePos = Input.mousePosition;
        //Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);
        //Vector3 dir = mousePos - playerPos;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(angle + 90.0f, Vector3.up);
        transform.Rotate(0f, Input.GetAxis("Mouse X") * offsetAngle_, 0f, Space.World);
        transform.Rotate(-Input.GetAxis("Mouse Y") * offsetAngle_, 0f, 0f);

    }
} // end of class
