using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Damagerから受けたダメージを担当する
/// </summary>
public class Damageable : MonoBehaviour, IDamageable
{
    //[Serializable]
    //public class HealthEvent : UnityEvent<Damageable>
    //{ }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable>
    { }

    //[Serializable]
    //public class HealEvent : UnityEvent<int, Damageable>
    //{ }

    public int startingHealth = 5;  // 初期HP
    protected int m_CurrentHealth;  // 現在HP
    public int CurrentHealth { get { return m_CurrentHealth; } }
    public float damageScale = 1.0f;    // ダメージ倍率

    protected bool m_Invulnerable = false;  // ダメージ無効状態
    protected float m_InulnerabilityTimer;  // 現在のダメージ無効時間
    public float invulnerabilityDuration = 3f;  // ダメージ無効時間

    protected Vector2 m_DamageDirection; // ダメージを受けた方向
    public Vector2 DamageDirection { get { return m_DamageDirection; } }

    public bool disableOnDeath = false; // デス時にGameObjectを非アクティブにするか

    // ダメージを受けたときのイベント
    public DamageEvent OnTakeDamage;

    // HP全損時のイベント
    public DamageEvent OnDie;

    void OnEnable()
    {
        // 初期HP設定
        m_CurrentHealth = startingHealth;

        // 無敵解除
        DisableInvulnerability();
    }

    /// <summary>
    /// ダメージ無効時間解除タイマー更新
    /// </summary>
    /// <returns></returns>
    protected IEnumerator UpdateInvulnerable()
    {
        if (!m_Invulnerable)
            yield break;


        while (m_InulnerabilityTimer >= 0f)
        {
            m_InulnerabilityTimer -= Time.deltaTime;
        }

        m_Invulnerable = false;

        yield return null;
    }

    /// <summary>
    /// ダメージ無効状態ON
    /// </summary>
    /// <param name="ignoreTimer"></param>
    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        //技術的にはタイマーを無視せず、非常に大きな数に設定するだけです。テストと特殊なケースを追加しないようにしてください
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    /// <summary>
    /// ダメージ無効状態OFF
    /// </summary>
    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    /// <summary>
    /// ダメージを受けることが可能か
    /// </summary>
    /// <returns>true:可 false:不可</returns>
    protected bool CanDamageble()
    {
        // 無敵状態やHPがない時などは不可

        if (m_Invulnerable)return false;
        if (m_CurrentHealth <= 0) return false;

        return true;
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="scale">与ダメージの倍率</param>
    protected void CalcDamage(int damage)
    {
        // 現在HPにダメージ値で更新
        m_CurrentHealth -= (int)(damage * damageScale);
        // 0 ～ Maxでクランプ
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, startingHealth);
    }

    // IDamageable--------------------------------------------------------------- //

    public void TakeDamage(Damager damager)
    {
        bool isd = CanDamageble();
        if (!CanDamageble())
            return;

        CalcDamage(damager.damage);
        
        // ダメージを受けた方向を計算
        m_DamageDirection = transform.position  - damager.transform.position;

        MyDebug.Log("痛い");
        OnTakeDamage.Invoke(damager, this);

        // デス
        if (m_CurrentHealth <= 0)
        {
            MyDebug.Log("やられた");
            OnDie.Invoke(damager, this);
            EnableInvulnerability();
            if (disableOnDeath)
                gameObject.SetActive(false);
        }
    }
}
