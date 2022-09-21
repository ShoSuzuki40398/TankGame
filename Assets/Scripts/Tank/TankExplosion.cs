using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransformExtension;

/// <summary>
/// 戦車の爆発演出制御
/// </summary>
public class TankExplosion : MonoBehaviour
{
    // 爆発パーティクル
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    // タンクパーツリスト
    [SerializeField]
    private TankPart[] m_TankParts = null;
    
    private void Awake()
    {
        // 戦車パーツ取得
        m_TankParts = transform.GetComponentsInChildren<TankPart>();
    }

    /// <summary>
    /// 爆発演出再生
    /// </summary>
    public void Explosion(Damager damager,Damageable damageable)
    {
        if (m_ExplosionEffect == null)
            return;

        // エフェクト再生
        m_ExplosionEffect.Play();

        // パーツ吹っ飛ぶ
        foreach(var part in m_TankParts)
        {
            part.StartInpulse();
        }
    }
}
