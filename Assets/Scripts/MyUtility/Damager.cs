using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

/// <summary>
/// Damageableを持つオブジェクトへの与ダメージを担当する
/// 当たり判定にColliderを使用しているため対象(Damageable側)にRigidBodyが必要
/// </summary>
public class Damager : MonoBehaviour
{
    // 攻撃可能オブジェクト(Damageableを持っている)に攻撃を与えたときのイベント
    [Serializable]
    public class DamageableEvent : UnityEvent<Damager, Damageable> { }

    // 攻撃不可オブジェクト(Damageableを持っていない)に攻撃を与えたときのイベント
    [Serializable]
    public class NonDamageableEvent : UnityEvent<Damager> { }

    public DamageableEvent OnDamageableHit;
    public NonDamageableEvent OnNonDamageableHit;

    protected bool m_CanDamage = true;
    public int damage = 1;  // 与えるダメージ量
    
    /// <summary>
    /// 与ダメージアクティブ
    /// </summary>
    public void EnableDamage()
    {
        m_CanDamage = true;
    }

    /// <summary>
    /// 与ダメージ非アクティブ
    /// </summary>
    public void DisableDamage()
    {
        m_CanDamage = false;
    }

    /// <summary>
    /// ダメージを与えたときのイベント
    /// </summary>
    /// <param name="damageable"></param>
    void OnDamage(Damageable damageable)
    {
        IDamageable target = damageable.GetComponent<IDamageable>();
        target.TakeDamage(this);
        OnDamageableHit?.Invoke(this, damageable);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ダメージを与える敵を取得
        Damageable damageable = other.GetComponent<Damageable>();

        // ダメージ処理を実行
        if (damageable)
        {   // ダメージを与えられるオブジェクトの場合のイベント
            OnDamage(damageable);
        }
        else
        {   // ダメージ判定自体がないオブジェクトの場合のイベント
            OnNonDamageableHit?.Invoke(this);
        }
    }
}

/// <summary>
/// ダメージ受理インターフェース
/// Damager→Damagebleに通知する用
/// Damageble以外のオブジェクトに通知する場合は
/// On～Event関数にinspectorから設定する
/// </summary>
public interface IDamageable
{
    void TakeDamage(Damager damager);
}

