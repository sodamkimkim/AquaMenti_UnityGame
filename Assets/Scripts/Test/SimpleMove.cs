using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    // Test�� ���� �⺻���� �̵�, ȸ���� ����
    private float moveSpeed = 3f;
    private float rotSpeed = 180f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rot = Vector3.zero;

    private bool isCaptured = false;


    private void Awake()
    {
        if (velocity == Vector3.zero)
            velocity = this.transform.position;
        if (rot == Vector3.zero)
            rot = this.transform.rotation.eulerAngles;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        MoveProcess();
        RotProcess();
        CapturedMouse();
    }


    private void Init()
    {
        isCaptured = true;
        if (isCaptured)
            // ���콺�� ������ ���߾� ���� �� ������ �ʰ� ��
            Cursor.lockState = CursorLockMode.Locked;
        else
        {
            // ���콺�� â ������ ����� ���ϰ� ��
            Cursor.lockState = CursorLockMode.Confined;
            VisibleMouse(false);
        }
    }


    private void MoveProcess()
    {
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(axisH, 0f, axisV);
        //transform.TransformDirection(rot.normalized);
        //velocity += move * moveSpeed * Time.deltaTime;

        //
        float angle = Mathf.Atan2(axisV * rotSpeed * Time.deltaTime, axisH * rotSpeed * Time.deltaTime);
        angle *= Mathf.Rad2Deg; // Radian�� Degree�� ��ȯ
        angle += 270f; // ������ 0�� �ǵ��� ����
        angle *= -1f; // �¿� ���� ����
        Vector3 angleVector = new Vector3(0f, angle, 0f);

        // �̵� ���� ����
        move.x *= -1f; // �̵� �¿� ���� ����
        move = Quaternion.Euler(angleVector) * move;

        // ī�޶� ���� �ν�
        angleVector += rot;
        transform.rotation = Quaternion.Euler(angleVector);

        // ĳ���Ͱ� �ٶ󺸴� ������ ������ �ǵ��� ����
        move = transform.TransformDirection(move.normalized);
        //

        velocity += move * moveSpeed * Time.deltaTime;

        transform.position = velocity;
    }

    private void RotProcess()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rot.x += -mouseY * rotSpeed * Time.deltaTime;

        // �� ���� ����
        if (rot.x > 80f)
            rot.x = 80f;
        else if (rot.x < -80f)
            rot.x = -80f;

        rot.y += mouseX * rotSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rot);
    }


    #region Mouse Capture in Screen
    private void CapturedMouse()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCaptured = !isCaptured;
            if(isCaptured)
                // ���콺�� ������ ���߾� ���� �� ������ �ʰ� ��
                Cursor.lockState = CursorLockMode.Locked;
            else
            {
                // ���콺�� â ������ ����� ���ϰ� ��
                Cursor.lockState = CursorLockMode.Confined;
                VisibleMouse(false);
            }
        }
    }

    // PowerWash Simulator ����
    // true: ����ȭ��, �ε���, �ΰ���-�º����, �ΰ���-�κ��丮���, �ΰ���-��񺯰���
    private void VisibleMouse(bool _visible)
    {
        Cursor.visible = _visible;
    }
    #endregion
}
