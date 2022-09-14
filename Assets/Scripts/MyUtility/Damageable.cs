using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Damager����󂯂��_���[�W��S������
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

    public int startingHealth = 5;  // ����HP
    protected int m_CurrentHealth;  // ����HP
    public int CurrentHealth { get { return m_CurrentHealth; } }
    public float damageScale = 1.0f;    // �_���[�W�{��

    protected bool m_Invulnerable = false;  // �_���[�W�������
    protected float m_InulnerabilityTimer;  // ���݂̃_���[�W��������
    public float invulnerabilityDuration = 3f;  // �_���[�W��������

    protected Vector2 m_DamageDirection; // �_���[�W���󂯂�����
    public Vector2 DamageDirection { get { return m_DamageDirection; } }

    public bool disableOnDeath = false; // �f�X����GameObject���A�N�e�B�u�ɂ��邩

    // �_���[�W���󂯂��Ƃ��̃C�x���g
    public DamageEvent OnTakeDamage;

    // HP�S�����̃C�x���g
    public DamageEvent OnDie;

    void OnEnable()
    {
        // ����HP�ݒ�
        m_CurrentHealth = startingHealth;

        // ���G����
        DisableInvulnerability();
    }

    /// <summary>
    /// �_���[�W�������ԉ����^�C�}�[�X�V
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
    /// �_���[�W�������ON
    /// </summary>
    /// <param name="ignoreTimer"></param>
    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        //�Z�p�I�ɂ̓^�C�}�[�𖳎������A���ɑ傫�Ȑ��ɐݒ肷�邾���ł��B�e�X�g�Ɠ���ȃP�[�X��ǉ����Ȃ��悤�ɂ��Ă�������
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    /// <summary>
    /// �_���[�W�������OFF
    /// </summary>
    public void DisableInvulnerability()
    {
        m_Invulnerable = false;
    }

    /// <summary>
    /// �_���[�W���󂯂邱�Ƃ��\��
    /// </summary>
    /// <returns>true:�� false:�s��</returns>
    protected bool CanDamageble()
    {
        // ���G��Ԃ�HP���Ȃ����Ȃǂ͕s��

        if (m_Invulnerable)return false;
        if (m_CurrentHealth <= 0) return false;

        return true;
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="scale">�^�_���[�W�̔{��</param>
    protected void CalcDamage(int damage)
    {
        // ����HP�Ƀ_���[�W�l�ōX�V
        m_CurrentHealth -= (int)(damage * damageScale);
        // 0 �` Max�ŃN�����v
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, startingHealth);
    }

    // IDamageable--------------------------------------------------------------- //

    public void TakeDamage(Damager damager)
    {
        bool isd = CanDamageble();
        if (!CanDamageble())
            return;

        CalcDamage(damager.damage);
        
        // �_���[�W���󂯂��������v�Z
        m_DamageDirection = transform.position  - damager.transform.position;

        MyDebug.Log("�ɂ�");
        OnTakeDamage.Invoke(damager, this);

        // �f�X
        if (m_CurrentHealth <= 0)
        {
            MyDebug.Log("���ꂽ");
            OnDie.Invoke(damager, this);
            EnableInvulnerability();
            if (disableOnDeath)
                gameObject.SetActive(false);
        }
    }
}
