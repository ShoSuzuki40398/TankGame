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

    // UI ---------------------------------------------------
    // ��ԍ��W
    // HP�Q�[�W��Ώۂ̍��W�ɒǏ]������
    private Transform tankTransform;
    [SerializeField]
    private Vector3 m_ViewOffset = Vector3.zero;

    // �\���L�����o�X
    [SerializeField]
    private Canvas m_TargetCanvas;

    // �c�@�\��
    [SerializeField]
    private GameObject m_RemainViewPerfab;
    private RemainView m_RemainView;

    private void Awake()
    {
        tankTransform = transform;
        Initialize();
    }

    private void Update()
    {
        UpdateViewPosition();
    }

    /// <summary>
    /// HP�o�[�\���ʒu�X�V
    /// </summary>
    private void UpdateViewPosition()
    {
        Vector3 pos = tankTransform.position + m_ViewOffset;
        m_RemainView.transform.position = WorldTo2DTranform.Transform(pos, m_TargetCanvas, Camera.main);
    }

    /// <summary>
    /// �c�@������
    /// </summary>
    public void Initialize()
    {
        // �����c�@��ݒ肷��
        SetRemain(m_InitRemainCount);

        // �c�@UI�𐶐�����RemainView�̎Q�Ƃ��擾
        m_RemainView = Instantiate(m_RemainViewPerfab, m_TargetCanvas.transform).GetComponent<RemainView>();

        // UI�Ɏc�@���𔽉f����
        m_RemainView.Initialize(m_InitRemainCount);
    }

    /// <summary>
    /// �w�肵�����̌��ݎc�@���𑝂₷
    /// </summary>
    /// <param name="count"></param>
    public void AddRemain(int count)
    {
        // ���ݎc�@������w�萔���₵���l��ݒ肷��
        SetRemain(m_CurrentRemainCount + count);
    }

    /// <summary>
    /// �w�肵�����̌��ݎc�@�������炷
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        // ���ݎc�@������w�萔���炵���l��ݒ肷��
        SetRemain(m_CurrentRemainCount - count);

        // �c�@UI�ɔ��f����
        m_RemainView.SubRemain(count);

        // �c�@����C�x���g����
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
    public void SetRemain(int count)
    {
        int clamp = Mathf.Clamp(count, 0, m_InitRemainCount);
        m_CurrentRemainCount = clamp;
    }

    /// <summary>
    /// �c�@UI�\��
    /// </summary>
    public void ShowRemainView()
    {
        m_RemainView.gameObject.Enable();
    }

    /// <summary>
    /// �c�@UI��\��
    /// </summary>
    public void HideRemainView()
    {
        m_RemainView.gameObject.Disable();
    }
}
