using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �c�@����
/// </summary>
public class TankRemain : MonoBehaviour
{
    // �����c�@��
    [SerializeField]
    private int m_InitRemainCount = 3;
    
    // ���ݎc�@��
    private int m_CurrentRemainCount = 0;

    // �c�@����C�x���g
    [SerializeField]
    private UnityEvent OnSubRemain;

    // �c�@�S����C�x���g
    [SerializeField]
    private UnityEvent OnLostAllRemain;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// �c�@������
    /// </summary>
    public void Initialize()
    {
        SetRemain(m_InitRemainCount);
    }

    /// <summary>
    /// �w�肵�����̌��ݎc�@���𑝂₷
    /// </summary>
    /// <param name="count"></param>
    public void AddRemain(int count)
    {
        SetRemain(m_CurrentRemainCount + count);
    }

    /// <summary>
    /// �w�肵�����̌��ݎc�@�������炷
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        SetRemain(m_CurrentRemainCount - count);
        OnSubRemain?.Invoke();

        // ���炵������,�c�@��0�ɂȂ�����S����C�x���g����
        if (m_CurrentRemainCount == 0)
            OnLostAllRemain?.Invoke();
    }

    /// <summary>
    /// ���ݎc�@����ݒ�
    /// �ȉ��͐ݒ�ł��Ȃ�
    /// �E�����c�@���𒴂��鐔
    /// �E0����
    /// </summary>
    public void SetRemain(int remain)
    {
        int clamp = Mathf.Clamp(remain, 0, m_InitRemainCount);
        m_CurrentRemainCount = remain;
    }


}
