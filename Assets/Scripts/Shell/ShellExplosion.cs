using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C�e�����I�u�W�F�N�g
/// Instantiate�œ��I�ɐ��������z��ł��B
/// �����������_�Ńp�[�e�B�N���A�T�E���h���Đ�����B
/// ���ł͂��̃N���X�ŒS������B
/// </summary>
public class ShellExplosion : MonoBehaviour
{
    // �����p�[�e�B�N��
    [SerializeField]
    private ParticleSystem m_ExplosionEffect;

    // ����SE
    [SerializeField]
    private AudioSource m_ExplosionSound;

    // ��������(���o�̑��Đ�����)
    [SerializeField]
    private float m_LifeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ���ŗ\��
        Destroy(gameObject, m_LifeTime);

        // ����
        Explosion();
    }
    
    /// <summary>
    /// �������o�J�n
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
