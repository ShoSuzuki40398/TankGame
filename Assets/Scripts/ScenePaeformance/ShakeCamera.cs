using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// ƒJƒƒ‰U“®§Œä
/// </summary>
public class ShakeCamera : MonoBehaviour
{
    // ƒJƒƒ‰U“®’Ê’m
    [SerializeField]
    private CinemachineImpulseSource m_CinemachineImpulseSource;

    private void Awake()
    {
        m_CinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// ƒJƒƒ‰U“®
    /// </summary>
    public void Shake(float force = 1)
    {
        m_CinemachineImpulseSource.GenerateImpulse(force);
    }
}
