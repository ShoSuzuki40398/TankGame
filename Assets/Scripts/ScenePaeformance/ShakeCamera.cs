using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// カメラ振動制御
/// </summary>
public class ShakeCamera : MonoBehaviour
{
    // カメラ振動通知
    [SerializeField]
    private CinemachineImpulseSource m_CinemachineImpulseSource;

    private void Awake()
    {
        m_CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// カメラ振動
    /// </summary>
    public void Shake(float force = 1)
    {
        m_CinemachineImpulseSource.GenerateImpulse(force);
    }
}
