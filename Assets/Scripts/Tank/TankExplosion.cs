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
    [SerializeField]
    private TankPart[] m_TankParts = null;
    
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

        // �G�t�F�N�g�Đ�
        m_ExplosionEffect.Play();

        // �p�[�c�������
        foreach(var part in m_TankParts)
        {
            part.StartInpulse();
        }
    }
}
