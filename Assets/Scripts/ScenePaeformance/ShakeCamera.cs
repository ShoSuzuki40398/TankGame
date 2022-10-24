using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// �J�����U������
/// </summary>
public class ShakeCamera : MonoBehaviour
{
    // �J�����U���ʒm
    [SerializeField]
    private CinemachineImpulseSource m_CinemachineImpulseSource;

    private void Awake()
    {
        m_CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// �J�����U��
    /// </summary>
    public void Shake(float force = 1)
    {
        m_CinemachineImpulseSource.GenerateImpulse(force);
    }
}
