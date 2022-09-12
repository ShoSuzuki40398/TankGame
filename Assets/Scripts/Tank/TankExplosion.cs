using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車の爆発演出制御
/// </summary>
public class TankExplosion : MonoBehaviour
{
    // 爆発パーティクル
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    /// <summary>
    /// 爆発演出再生
    /// </summary>
    public void Explosion()
    {
        if (m_ExplosionEffect == null)
            return;

        m_ExplosionEffect.Play();
    }
}
