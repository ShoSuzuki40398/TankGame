using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// インゲーム中のVirtualCamera
/// </summary>
public class IngameCameras : MonoBehaviour
{
    public CinemachineTargetGroup m_IngameTargetGroup;

    public CinemachineVirtualCamera m_PlayerVcam;
    [SerializeField]
    private float m_PlayerWeight;
    [SerializeField]
    private float m_PlayerRadius;

    public CinemachineVirtualCamera m_EnemyVcam;
    [SerializeField]
    private float m_EnemyWeight;
    [SerializeField]
    private float m_EnemyRadius;

    /// <summary>
    /// インゲームカメラ設定
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    public void SettingIngameCamera(Transform player, Transform enemy)
    {
        SetTargetToIngameGroup(player, enemy);
        SetTargetToPlayerVcam(player);
        SetTargetToEnemyVcam(enemy);
    }

    /// <summary>
    /// プレイヤーと敵をカメラグループに設定する
    /// </summary>
    public void SetTargetToIngameGroup(Transform player,Transform enemy)
    {
        m_IngameTargetGroup.AddMember(player, m_PlayerWeight, m_PlayerRadius);
        m_IngameTargetGroup.AddMember(enemy,m_EnemyWeight,m_EnemyRadius);
    }

    public void SetTargetToPlayerVcam(Transform target)
    {
        m_PlayerVcam.LookAt = target;
        m_PlayerVcam.Follow = target;
    }

    public void SetTargetToEnemyVcam(Transform target)
    {
        m_EnemyVcam.LookAt = target;
        m_EnemyVcam.Follow = target;
    }
}
