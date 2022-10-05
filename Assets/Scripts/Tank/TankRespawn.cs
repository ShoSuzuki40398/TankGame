using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ԃ��X�|�[������
/// ���O�ɃV�[���ɔz�u�����uRespowner�v�ɑ΂��ă��X�|�[�����W�v�����s���܂�
/// ���X�|�[�����̏����͂�����̃N���X�ŒS�����܂��B�iHP�o�[�̃��Z�b�g���Ԃ̏��������j
/// </summary>
public class TankRespawn : MonoBehaviour
{
    // ���X�|�[������
    [SerializeField]
    private Respawner m_Respawner;

    // HP�\��
    [SerializeField]
    private TankHealth m_TankHealth;

    // ���X�|�[���J�n�܂ł̎���
    [SerializeField]
    private float m_StartRespawnCount = 2.5f;

    // ���X�|�[�����C�x���g�̒x������
    // ���X�|�[�����đ�UI�����ĕ\�������
    // ���X�|�[���O�̍��W�Ƀ`�����̂ň�u�����x�������܂�
    private const float k_EventDelayTime = 0.15f;

    // ���X�|�[���O�̃C�x���g
    public UnityEvent OnPreRepawnEvent;

    // ���X�|�[�����̃C�x���g
    // m_StartRespawnCount���x��������
    public UnityEvent OnRespawnEvent;
        
    /// <summary>
    /// ���X�|�[���J�n
    /// </summary>
    public void Respawn(Damager damager,Damageable damageable)
    {
        this.Delay(m_StartRespawnCount, ()=>Initialize(damager,damageable));
    }
    
    /// <summary>
    /// ���W�A�p�����[�^�[���̏�����
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="damageable"></param>
    private void Initialize(Damager damager, Damageable damageable)
    {
        // ���X�|�[���O�C�x���g
        PreExecuteRepawnEvent();

        // ���񂾏ꏊ�����X�|�[������ɑ���A���X�|�[���ʒu���擾����
        Vector3 pos = transform.position;
        Transform respawnPoint = m_Respawner.GetRespawnPoint(pos);

        // �ʒu�Ɖ�]�̏�����
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        // �̗͐ݒ�
        damageable.ResetHealth();
        m_TankHealth.HPChange(damageable.CurrentHealth);


        // �����X�|�[�����鎞�Ƀ��X�|�[���O�̍��W�Ɉ�uHP�o�[���\�������s�������
        // �@�������悭������Ȃ��̂Ŏb��I�ɏ��������x���������Ă܂�
        this.Delay(k_EventDelayTime, ()=> ExecuteRepawnEvent());
    }

    /// <summary>
    /// ���X�|�[���O�C�x���g���s
    /// </summary>
    private void PreExecuteRepawnEvent()
    {
        OnPreRepawnEvent?.Invoke();
    }

    /// <summary>
    /// ���X�|�[�����C�x���g���s
    /// </summary>
    private void ExecuteRepawnEvent()
    {
        m_TankHealth.ShowGauge();
        OnRespawnEvent?.Invoke();
    }
}
