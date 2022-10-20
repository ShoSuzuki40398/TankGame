using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// 戦車の爆発演出制御
/// </summary>
public class TankExplosion : MonoBehaviour
{
    // 爆発パーティクル
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    // タンクパーツリスト
    private TankPart[] m_TankParts = null;

    // 爆発SE
    [SerializeField]
    private AudioSource m_ExplosionSound;

    // カメラ振動通知元
    [SerializeField]
    private CinemachineImpulseSource m_CinemachineImpulseSource;

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

        if (m_ExplosionSound == null)
            return;

        // エフェクト再生
        m_ExplosionEffect.Play();

        // 爆発SE再生
        m_ExplosionSound.Play();

        // カメラを振動させる
        m_CinemachineImpulseSource.GenerateImpulse();

        // パーツ吹っ飛ぶ
        foreach(var part in m_TankParts)
        {
            part.StartInpulse();
        }
    }
}
