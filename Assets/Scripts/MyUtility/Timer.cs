using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ԍv���N���X
/// </summary>
public class Timer
{
    // �o�ߎ���
    private float m_CurrentTime = 0.0f;
    public float CurrentTime { get { return m_CurrentTime; } }

    // �ڕW����
    private float m_EndTime = 0.0f;

    // �^�C�}�[�N���t���O(true�œ���)
    private bool m_IsAwake = false;

    // �^�C�}�[���������s����
    public Action OnComplete = null;

    /// <summary>
    /// �^�C�}�[�N��
    /// </summary>
    /// <param name="end">�ڕW���Ԑݒ�</param>
    /// <param name="offset">�o�ߎ��ԃI�t�Z�b�g(��{��0�b�o�ߎw��)</param>
    public void Awake(float end,float offset = 0.0f)
    {
        m_CurrentTime = offset;
        m_EndTime = end;
        m_IsAwake = true;
    }

    /// <summary>
    /// Time.deltaTime�ɂ��o�ߎ��Ԃ��X�V
    /// </summary>
    /// <returns>�w�肵���o�ߎ��Ԃ𒴂����ꍇ��true��Ԃ�</returns>
    public bool UpdateWithDeltaTime()
    {
        bool res = false;

        // ���������N�����Ă��Ȃ��Ȃ�X�V���Ȃ�
        if (!m_IsAwake) return res;

        // deltaTime�Ōo�ߎ��Ԃ��v��
        m_CurrentTime += Time.deltaTime;
        if(m_CurrentTime > m_EndTime)
        {
            res = true;
            // �I�����̊֐������s
            OnComplete?.Invoke();
            // ��x�v�����I�������烊�Z�b�g
            // �����[�v�������ꍇ�͕ʓr�������C������
            Reset();
        }

        return res;
    }

    /// <summary>
    /// �^�C�}�[���Z�b�g
    /// </summary>
    public void Reset()
    {
        m_IsAwake = false;
        m_CurrentTime = 0.0f;
    }
}
