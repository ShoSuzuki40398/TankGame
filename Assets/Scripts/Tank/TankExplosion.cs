using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransformExtension;

/// <summary>
/// ��Ԃ̔������o����
/// </summary>
public class TankExplosion : MonoBehaviour
{
    // �����p�[�e�B�N��
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    // �^���N�p�[�c���X�g
    private TankPart[] m_TankParts = null;

    // ����SE
    [SerializeField]
    private AudioSource m_ExplosionSound;

    private void Awake()
    {
        // ��ԃp�[�c�擾
        m_TankParts = transform.GetComponentsInChildren<TankPart>();
    }

    /// <summary>
    /// �������o�Đ�
    /// </summary>
    public void Explosion(Damager damager,Damageable damageable)
    {
        if (m_ExplosionEffect == null)
            return;

        if (m_ExplosionSound == null)
            return;

        // �G�t�F�N�g�Đ�
        m_ExplosionEffect.Play();

        // ����SE�Đ�
        m_ExplosionSound.Play();

        // �p�[�c�������
        foreach(var part in m_TankParts)
        {
            part.StartInpulse();
        }
    }
}
