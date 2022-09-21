using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransformExtension;

/// <summary>
/// ��ԕ��i���
/// �������o�̂��߂ɐ�Ԃ��\������e�I�u�W�F�N�g�ɃA�^�b�`����B
/// 
/// </summary>
public class TankPart : InpulseBehaviour
{
    // �^���N�p�[�c�̃��[�J�����W�����l
    // �p�[�c���e���鉉�o���������̂�
    // �e�������Ƃ̃��Z�b�g�ŏ����ʒu�ɖ߂����߂ɏ����l��ێ�����
    private ExTransform m_InitLocalTransform;

    // �����o�ߎ��Ԍv��
    private Timer m_InpulseTimer = new Timer();

    // �������v����
    [SerializeField]
    private float m_InpulseTime = 3.0f;

    private void Awake()
    {
        // �N�����ɏ����l��ێ����Ă���
        m_InpulseProperty.Initialize();

        // �����ʒu��ۑ�����
        m_InitLocalTransform = new ExTransform(transform);
    }

    /// <summary>
    /// �p�[�c������ъJ�n
    /// </summary>
    public void StartInpulse()
    {
        StartCoroutine(UpdateInpulse());
    }

    /// <summary>
    /// �p�[�c������эX�V
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateInpulse()
    {
        // ���v���Ԑݒ�
        m_InpulseTimer.Awake(m_InpulseTime);

        // �ݒ肵�����ԕ���������
        while (!m_InpulseTimer.UpdateWithDeltaTime())
        {
            Vector3 inpulseVelocity = m_InpulseProperty.UpdateInpluse();

            transform.localPosition += inpulseVelocity;
            transform.Rotate(m_InpulseProperty.RatoteVelocity);

            yield return null;
        }

        // �����ʒu�ɖ߂�
        ResetPart();
    }

    /// <summary>
    /// Transfrom���Z�b�g
    /// </summary>
    public void ResetPart()
    {
        // �����ʒu�ɖ߂�
        transform.localPosition = m_InitLocalTransform.position;
        transform.localRotation = m_InitLocalTransform.rotation;

        // �����������Z�b�g
        m_InpulseTimer.Reset();
        m_InpulseProperty.Reset();
    }
}
