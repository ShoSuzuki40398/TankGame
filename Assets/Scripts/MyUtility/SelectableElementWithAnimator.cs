using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animator経由でイベントを発火させる選択肢オブジェクト
/// </summary>
[RequireComponent(typeof(Animator))]
public class SelectableElementWithAnimator : BaseSelectableElement
{
    /* References */
    protected Animator m_Animator;

    // Animatorの各状態のキー
    // ※Animatorには以下の名前で事前にParameterを追加して下さい
    protected readonly string m_Normal      = "Normal";
    protected readonly string m_Highlighted = "Highlighted";
    protected readonly string m_Decided     = "Decided";
    protected readonly string m_Selected    = "Selected";
    protected readonly string m_Disabled    = "Disabled";

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    //----------- ISelectable ------------

    public override void Decided()
    {
        m_Animator.SetTrigger(m_Decided);
        m_DecidedAction?.Invoke();
    }

    public override void Disabled()
    {
        m_Animator.SetTrigger(m_Disabled);
        m_DisabledAction?.Invoke();
    }

    public override void Highlighted()
    {
        m_Animator.SetTrigger(m_Highlighted);
        m_HighlightedAction?.Invoke();
    }

    public override void Normal()
    {
        m_Animator.SetTrigger(m_Normal);
        m_NormalAction.Invoke();
    }

    public override void Selected()
    {
        m_Animator.SetTrigger(m_Selected);
        m_SelectedAction?.Invoke();
    }
}
