using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// �I�����I�u�W�F�N�g���N���X
/// </summary>
public abstract class BaseSelectableElement : MonoBehaviour, ISelectable,IPointerEnterHandler
{
    [HideInInspector]
    public SelectableObjectGroup m_Group;

    // ���ʔԍ�
    [HideInInspector]
    public int index = 0;

    [SerializeField]
    protected UnityEvent m_DecidedAction;
    [SerializeField]
    protected UnityEvent m_DisabledAction;
    [SerializeField]
    protected UnityEvent m_HighlightedAction;
    [SerializeField]
    protected UnityEvent m_NormalAction;
    [SerializeField]
    protected UnityEvent m_SelectedAction;

    // �J�[�\���t�H�[�J�X���C�x���g
    // inspector����ݒ�s�B
    // �L�[�{�[�h�ƃ}�E�X�ɂ��I���̗����̂��߂ɓ����I�Ɏ����ɂȂ��Ă���B
    // �t�H�[�J�X���̃C�x���g��ݒ肵�����ꍇ�́Am_SelectedAction���g�����Ƃŉ������邱�ƁB
    [HideInInspector]
    public event Action<int> m_FocusAction;

    public abstract void Decided();
    public abstract void Disabled();
    public abstract void Highlighted();
    public abstract void Normal();
    public abstract void Selected();

    /// <summary>
    /// �|�C���^�[ �t�H�[�J�X�C�x���g
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        m_FocusAction?.Invoke(index);
    }

    protected void Awake()
    {
        // Button�R���|�[�l���g������ꍇ��OnClick�Ɍ���C�x���g��ݒ肷��
        Button button = GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(()=> { m_Group.SelectElementWithIndex(index);m_Group.DecideElement(); });
        }
    }

}
