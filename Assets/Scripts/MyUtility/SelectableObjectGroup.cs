using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ISelectable���p�����Ă���v�f�𑀍삷��
/// ����Ώۂ̓C���X�^���X��UDID(GetInstanceID)�Ŕ��ʂ���
/// </summary>
public class SelectableObjectGroup : MonoBehaviour
{
    // ����Ώ�
    [SerializeField]
    protected List<BaseSelectableElement> m_Selectables;
    protected Dictionary<int, BaseSelectableElement> m_SelectablePairs = new Dictionary<int, BaseSelectableElement>();

    public BaseSelectableElement currentSelectedElement { get { return m_Selectables[m_CurrentSelectedElementIndex]; } }

    // ���ݑI�����Ă���I�����̃C���f�b�N�X
    // �����l�͍ŏ�ʂ̑I����
    protected int m_CurrentSelectedElementIndex = 0;

    private void Awake()
    {
        m_SelectablePairs = m_Selectables.ToDictionary(ele => ele.gameObject.GetInstanceID());
    }

    private void Start()
    {
        // �ŏ�ʂ̑I������I��������Ԃ���J�n
        SelectElementWithIndex(m_CurrentSelectedElementIndex);
    }

    /// <summary>
    /// �I������
    /// </summary>
    /// <param name="index"></param>
    private void SelectElement(int index)
    {
        // �͈̓`�F�b�N
        if (!index.IsRange(0, m_Selectables.Count - 1))
            return;

        // ���ݑI�����Ă���I�u�W�F�N�g��Normal��Ԃɖ߂��A�V�K�ɑI�����ꂽ�I�u�W�F�N�g��Selected��Ԃɂ���
        currentSelectedElement.Normal();
        m_CurrentSelectedElementIndex = index;
        currentSelectedElement.Selected();
    }

    /// <summary>
    /// �I�𒆂̑I�����Ō��肷��
    /// </summary>
    public void DecideElement()
    {
        currentSelectedElement.Decided();
    }

    /// <summary>
    /// ���݈ʒu����I�������w�肵�Ĉړ�����
    /// </summary>
    /// <param name="index"></param>
    public void SelectElementWithIndex(int index)
    {
        SelectElement(index);
    }

    /// <summary>
    /// ���݈ʒu���瑊�ΓI�Ɉړ�����
    /// ��ʑI�����Ɉړ�����Ƃ��͐��̒l(1)
    /// ���ʑI�����Ɉړ�����Ƃ��͕��̒l(-1)
    /// </summary>
    /// <param name="value"></param>
    public void SelectElementWithValue(int value = 0)
    {
        int newIndex = m_CurrentSelectedElementIndex + value;

        SelectElement(newIndex);
    }

    /// <summary>
    /// ���݈ʒu����ʂ̑I������I��
    /// </summary>
    /// <param name="value">�I���ړ���</param>
    public void SelectUpperElement(int value = 1)
    {
        SelectElementWithIndex(m_CurrentSelectedElementIndex + value);
    }

    /// <summary>
    /// ���݈ʒu��艺�ʂ̑I������I��
    /// </summary>
    /// <param name="value">�I���ړ���</param>
    public void SelectLowerElement(int value = 1)
    {
        SelectElementWithIndex(m_CurrentSelectedElementIndex - value);
    }
}