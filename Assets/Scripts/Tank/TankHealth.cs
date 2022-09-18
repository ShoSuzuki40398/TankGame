using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ̗͕̑\��
/// UI��ɐ�Ԃ̗̑͂�\������N���X�ł��B
/// ���ۂ�HP�Ǘ���Damageble�N���X���S�����Ă��܂��B
/// </summary>
public class TankHealth : MonoBehaviour
{
    // �̗̓o�[
    // �V�[���ɔz�u�����I�u�W�F�N�g���A�^�b�`���Ă��ǂ����A
    // ��Ԃ��v���n�u�ɂ���֌W�ŕt������HP�o�[�𓮓I�ɍ쐬��������
    // ��Ԃ��V�[���ɔz�u���₷���Ȃ�
    [SerializeField]
    private GameObject m_HPGaugeViewPrefab;
    private HPGaugeView m_HPGaugeView;

    // HP�Q�[�W��z�u����Canvas
    [SerializeField]
    private Canvas m_HPGaugeViewParent;

    // ��ԍ��W
    // HP�Q�[�W��Ώۂ̍��W�ɒǏ]������
    private Transform tankTransform;
    [SerializeField]
    private Vector3 m_ViewOffset = Vector3.zero;

    /// <summary>
    /// HP�Q�[�W������
    /// </summary>
    /// <param name="hPGaugeView"></param>
    private void InitializeHPView(HPGaugeView hPGaugeView)
    {
        hPGaugeView.Initialize();
        UpdateViewPosition();
    }

    private void Awake()
    {
        tankTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        // HP�Q�[�W�I�u�W�F�N�g�𐶐�����HPGaugeView�̎Q�Ƃ��擾
        m_HPGaugeView = Instantiate(m_HPGaugeViewPrefab, m_HPGaugeViewParent.transform).GetComponent<HPGaugeView>();
        InitializeHPView(m_HPGaugeView);
    }

    private void Update()
    {
        UpdateViewPosition();
    }

    private void UpdateViewPosition()
    {
        Vector3 pos = tankTransform.position + m_ViewOffset;
        m_HPGaugeView.transform.position = WorldTo2DTranform.Transform(pos, m_HPGaugeViewParent, Camera.main);
    }

    /// <summary>
    /// HP��ݒ�
    /// </summary>
    /// <param name="damageable"></param>
    public void HPChange(Damager damager, Damageable damageable)
    {
        m_HPGaugeView.SetHP(damageable.CurrentHealth);
    }

    /// <summary>
    /// HP��ݒ�
    /// </summary>
    /// <param name="damageable"></param>
    public void HPChange(float health)
    {
        m_HPGaugeView.SetHP(health);
    }
}
