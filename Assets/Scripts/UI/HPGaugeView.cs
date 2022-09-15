using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP�Q�[�W��View����
/// �l�̕ϓ���0�`1�̊����ŕω�������
/// </summary>
public class HPGaugeView : MonoBehaviour
{
    // HP�o�[�����ڏ��
    [System.Serializable]
    public class HPColorProperty
    {
        // �ύX��̐F
        public Color32 color;
        // �F�ς�������臒l
        public float changeBorder;
    }
    
    // �����ڏ��̃��X�g
    [SerializeField]
    private List<HPColorProperty> colorProperties = new List<HPColorProperty>();
    
    // ���݂�HP�����ڏ��
    // Update���Ŗ��t���[���Ď�������AHP���ϓ��������ɎQ�Ƃł���΂悢�̂�
    // �ێ�����`�ɂ��܂���
    private HPColorProperty m_CurrentHPColorProperty;

    // HP�c�ʃC���[�W
    [SerializeField]
    private Image m_Fill;

    // HP�c�ʕ\��
    [SerializeField]
    private Slider m_Slider;

    // HP�ő�l
    public float MaxValue { get { return m_Slider.maxValue; } }
        
    /// <summary>
    /// ������
    /// </summary>
    public void Initialize()
    {
        // �ő�l�ɕύX
        HPColorProperty maxProperty = GetMaxHPColorProperty();
        ResetView(maxProperty);
    }

    /// <summary>
    /// �\�����Z�b�g
    /// </summary>
    private void ResetView(HPColorProperty hpProperty)
    {
        m_Slider.value = hpProperty.changeBorder;
        m_Fill.color = hpProperty.color;
        m_CurrentHPColorProperty = hpProperty;
        UpdateHPColorProperty();
    }

    /// <summary>
    /// 臒l���ő�̌����ڏ����擾
    /// (�܂�HP�ő�l�̏��)
    /// </summary>
    /// <returns></returns>
    private HPColorProperty GetMaxHPColorProperty()
    {
        return colorProperties.FindMax(p => p.changeBorder);
    }

    /// <summary>
    /// �c��HP�𒼐ڐݒ�
    /// </summary>
    public void SetHP(float value)
    {
        float ratioValue = value * 0.01f;
        float clampValue = Mathf.Clamp(ratioValue, 0, MaxValue);
        m_Slider.value = clampValue;
        UpdateHPColorProperty();
    }

    /// <summary>
    /// HP�o�[�̕ϓ�
    /// </summary>
    private void HPChange(float value)
    {
        m_Slider.value += value;
        m_Slider.value = Mathf.Clamp(m_Slider.value, 0, MaxValue);
        UpdateHPColorProperty();
    }

    /// <summary>
    /// HP����
    /// �S�����Ō��Z����
    /// </summary>
    public void SubHP(float value)
    {
        float ratioValue = value * 0.01f;
        HPChange(-ratioValue);
    }

    /// <summary>
    /// HP����
    /// �S�����ŉ��Z����
    /// </summary>
    public void AddHP(float value)
    {
        float ratioValue = value * 0.01f;
        HPChange(ratioValue);
    }

    /// <summary>
    /// ���݂̌����ڏ����X�V
    /// ���݂�HP�l���猩���ڂ̏����X�V����
    /// </summary>
    private void UpdateHPColorProperty()
    {
        float currentValue = m_Slider.value;

        // �O�̂���臒l�̏�����Ń\�[�g����
        var array = colorProperties.ToArray();
        array.Sort(pro => pro.changeBorder);

        foreach (var pro in array)
        {
            if(pro.changeBorder >= currentValue)
            {
                m_CurrentHPColorProperty = pro;
                break;
            }
        }

        if (m_CurrentHPColorProperty == null)
            return;

        // ���݂�HP���Ńo�[�̐F��ύX����
        m_Fill.color = m_CurrentHPColorProperty.color;
    }
}
