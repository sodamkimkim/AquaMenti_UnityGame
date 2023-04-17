using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerMovement �������̽�
/// </summary>
public interface IPlayerMovement
{

    void Walk(Vector3 _direction); // �ȱ�
    void Run(Vector3 _direction); // �ٱ�
    void StandUp(); // ����
    void SitDown(); // �ɱ�
    void KneelDown(); // ���帮��
    void Jump();
    void SetPosition(Vector3 _direction, Quaternion _rotation);
}
