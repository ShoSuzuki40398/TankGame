using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԉړ�
/// �����őO�i���
/// �����Ŏ��g��Y���𒆐S�ɉ�]������
/// �ړ����@��Rigidbody���g�p����
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    [SerializeField,Tooltip("�ړ����x")]
    private float m_Speed = 1f;
    [SerializeField, Tooltip("��]���x")]
    private float m_TurnSpeed = 180f;

    // �y���G�t�F�N�g
    [SerializeField]
    private List<ParticleSystem> m_DustTrails = new List<ParticleSystem>();

    // �G���W����
    // �ړ����Ă��鎞�����Đ�����悤�ɂ���
    [SerializeField]
    private AudioSource m_EngineSound;

    // 1�t���[���̑��x
    public Vector3 Velocity { get; protected set; }

    // �ړ��p���W�b�h�{�f�B
    private Rigidbody m_Rigidbody;
    private Vector3 m_PreviousPosition; // �O���W
    private Vector3 m_CurrentPosition;  // ���ݍ��W
    private Vector3 m_NextMovement;     // �ړ���

    private Vector3 m_PreviousRotation; // �O��]
    private Vector3 m_CurrentRotation;  // ���݉�]
    private Quaternion m_NextRotate;       // ��]��


    private void Awake()
    {
        // �A�^�b�`�ς݃R���|�[�l���g�擾
        m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody != null)
            m_CurrentPosition = m_PreviousPosition = m_Rigidbody.position;
    }

    private void FixedUpdate()
    {
        EngineSoundUpdate();

        // ���W�Ɖ�]�X�V
        PositionUpdate();
        RotationUpdate();
    }

    /// <summary>
    /// ���W�X�V
    /// </summary>
    private void PositionUpdate()
    {
        // �ړ��ʂ��玟�̍��W��ݒ肷��
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_NextMovement);
        m_NextMovement = Vector3.zero;
    }

    /// <summary>
    /// ��]�X�V
    /// </summary>
    private void RotationUpdate()
    {
        Quaternion q = (m_Rigidbody.rotation * m_NextRotate).normalized;
        m_Rigidbody.MoveRotation(q);
        m_NextRotate = Quaternion.identity;
    }

    /// <summary>
    /// �V���v���ɍ��W�ړ�
    /// </summary>
    /// <param name="speedScale">�ړ��{��</param>
    public void Move( float speedScale = 1.0f)
    {
        m_NextMovement = transform.forward * speedScale * m_Speed * Time.deltaTime;
    }

    /// <summary>
    /// Y���𒆐S�ɍ��W��]
    /// </summary>
    /// <param name="speedScale">��]�{���@���@��]����</param>
    public void Turn(float speedScale = 1.0f)
    {
        float turn = speedScale * m_TurnSpeed * Time.deltaTime;
        m_NextRotate = Quaternion.Euler(0, turn, 0).normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EngineSoundUpdate()
    {
        if(Mathf.Approximately(m_NextMovement.magnitude,0))
        {
            m_EngineSound.Stop();
            return;
        }

        if(!m_EngineSound.isPlaying)
            m_EngineSound.Play();
        
    }

    /// <summary>
    /// ���s�G�t�F�N�g�\��
    /// </summary>
    public void EffectEnable()
    {
        foreach (var eff in m_DustTrails)
        {
            eff.gameObject.Enable();
        }
    }

    /// <summary>
    /// ���s�G�t�F�N�g��\��
    /// </summary>
    public void EffectDisable()
    {
        m_EngineSound.Stop();
        foreach (var eff in m_DustTrails)
        {
            eff.gameObject.Disable();
        }
    }
}
