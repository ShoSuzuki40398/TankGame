using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砲弾爆発オブジェクト
/// Instantiateで動的に生成される想定です。
/// 生成した時点でパーティクル、サウンドを再生する。
/// 消滅はこのクラスで担当する。
/// </summary>
public class ShellExplosion : MonoBehaviour
{
    // 爆発パーティクル
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    // 爆発SE
    [SerializeField]
    private AudioSource m_ExplosionSound;

    // 生存時間(演出の総再生時間)
    [SerializeField]
    private float m_LifeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 消滅予約
        Destroy(gameObject, m_LifeTime);

        // 爆発
        Explosion();
    }
    
    /// <summary>
    /// 爆発演出開始
    /// </summary>
    public void Explosion()
    {
        if (m_ExplosionEffect == null)
            return;

        if (m_ExplosionSound == null)
            return;

        m_ExplosionEffect.Play();

        m_ExplosionSound.Play();
    }
}
