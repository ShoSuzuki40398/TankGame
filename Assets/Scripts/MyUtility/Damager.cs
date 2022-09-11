using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

/// <summary>
/// Damageable�����I�u�W�F�N�g�ւ̗^�_���[�W��S������
/// �����蔻���Collider���g�p���Ă��邽�ߑΏ�(Damageable��)��RigidBody���K�v
/// </summary>
public class Damager : MonoBehaviour
{
    // �U���\�I�u�W�F�N�g(Damageable�������Ă���)�ɍU����^�����Ƃ��̃C�x���g
    [Serializable]
    public class DamageableEvent : UnityEvent<Damager, Damageable> { }

    // �U���s�I�u�W�F�N�g(Damageable�������Ă��Ȃ�)�ɍU����^�����Ƃ��̃C�x���g
    [Serializable]
    public class NonDamageableEvent : UnityEvent<Damager> { }

    public DamageableEvent OnDamageableHit;
    public NonDamageableEvent OnNonDamageableHit;

    protected bool m_CanDamage = true;
    public int damage = 1;  // �^����_���[�W��
    
    /// <summary>
    /// �^�_���[�W�A�N�e�B�u
    /// </summary>
    public void EnableDamage()
    {
        m_CanDamage = true;
    }

    /// <summary>
    /// �^�_���[�W��A�N�e�B�u
    /// </summary>
    public void DisableDamage()
    {
        m_CanDamage = false;
    }

    /// <summary>
    /// �_���[�W��^�����Ƃ��̃C�x���g
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
        // �_���[�W��^����G���擾
        Damageable damageable = other.GetComponent<Damageable>();

        // �_���[�W���������s
        if (damageable)
        {   // �_���[�W��^������I�u�W�F�N�g�̏ꍇ�̃C�x���g
            OnDamage(damageable);
        }
        else
        {   // �_���[�W���莩�̂��Ȃ��I�u�W�F�N�g�̏ꍇ�̃C�x���g
            OnNonDamageableHit?.Invoke(this);
        }
    }
}

/// <summary>
/// �_���[�W�󗝃C���^�[�t�F�[�X
/// Damager��Damageble�ɒʒm����p
/// Damageble�ȊO�̃I�u�W�F�N�g�ɒʒm����ꍇ��
/// On�`Event�֐���inspector����ݒ肷��
/// </summary>
public interface IDamageable
{
    void TakeDamage(Damager damager);
}

