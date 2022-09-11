using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �I�����I�u�W�F�N�g���N���X
/// </summary>
public abstract class BaseSelectableElement : MonoBehaviour, ISelectable
{
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

    public abstract void Decided();
    public abstract void Disabled();
    public abstract void Highlighted();
    public abstract void Normal();
    public abstract void Selected();
}
