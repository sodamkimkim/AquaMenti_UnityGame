using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// PlayerMovement �������̽�
/// </summary>
public interface IPlayerMovement
{
    /// <summary>
    /// walk vs run�̳Ŀ� ���� _direction�� ���� moveSpeed�޶���
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_direction"></param>
    void Walk(Vector3 _direction); // �ȱ�
    void Run(); // �ٱ�
    void StandUp(); // ����
    void SitDown(); // �ɱ�
    void KneelDown(); // ���帮��
    void Jump();
    void SetAnimation();
}
