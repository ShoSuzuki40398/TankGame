using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ̔������o����
/// </summary>
public class TankExplosion : MonoBehaviour
{
    // �����p�[�e�B�N��
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    /// <summary>
    /// �������o�Đ�
    /// </summary>
    public void Explosion()
    {
        if (m_ExplosionEffect == null)
            return;

        m_ExplosionEffect.Play();
    }
}
