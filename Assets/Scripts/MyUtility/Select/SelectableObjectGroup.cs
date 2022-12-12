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
        foreach(var ele in m_Selectables)
        {
            ele.m_Group = this;
        }
    }

    private void Start()
    {
        // ���ʔԍ��ԍ����蓖��
        AssignmentIndex();

        // �ŏ�ʂ̑I������I��������Ԃ���J�n
        SelectElement(m_CurrentSelectedElementIndex);
    }
    
    /// <summary>
    /// �e�I�����Ɏ���ID��ݒ肷��
    /// �ŏ��̑I������0�Ƃ��A�����Ŋ��蓖�Ă�
    /// </summary>
    private void AssignmentIndex()
    {
        foreach (var ele in m_Selectables.Select((value, index) => (value, index)))
        {
            ele.value.index = ele.index;
            ele.value.m_FocusAction += FocusElement;
        }
    }

    /// <summary>
    /// �J�[�\���Ńt�H�[�J�X������
    /// </summary>
    private void FocusElement(int index)
    {
        SelectElementWithIndex(index);
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
        // ���l�`�F�b�N
        if (m_CurrentSelectedElementIndex == index)
            return;

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

    /// <summary>
    /// �I�����Ă��Ȃ��I�����̎擾
    /// </summary>
    /// <returns></returns>
    public List<BaseSelectableElement> UnSelectElements()
    {
        List<BaseSelectableElement> res = new List<BaseSelectableElement>();

        return m_Selectables.FindAll(ele => ele.index != m_CurrentSelectedElementIndex);
    }


    /// <summary>
    /// �I�����Ă��Ȃ��I�������m�[�}����Ԃɂ���
    /// </summary>
    /// <returns></returns>
    public void UnSelectElementsToNormal()
    {
        var elements = UnSelectElements();

        foreach(var ele in elements)
        {
            ele.Normal();
        }
    }
}
