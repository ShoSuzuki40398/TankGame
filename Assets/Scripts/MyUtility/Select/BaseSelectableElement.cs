using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 選択肢オブジェクト基底クラス
/// </summary>
public abstract class BaseSelectableElement : MonoBehaviour, ISelectable,IPointerEnterHandler
{
    [HideInInspector]
    public SelectableObjectGroup m_Group;

    // 識別番号
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

    // カーソルフォーカス時イベント
    // inspectorから設定不可。
    // キーボードとマウスによる選択の両立のために内部的に実装になっている。
    // フォーカス時のイベントを設定したい場合は、m_SelectedActionを使うことで解消すること。
    [HideInInspector]
    public event Action<int> m_FocusAction;

    public abstract void Decided();
    public abstract void Disabled();
    public abstract void Highlighted();
    public abstract void Normal();
    public abstract void Selected();

    /// <summary>
    /// ポインター フォーカスイベント
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        m_FocusAction?.Invoke(index);
    }

    protected void Awake()
    {
        // Buttonコンポーネントがある場合はOnClickに決定イベントを設定する
        Button button = GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(()=> { m_Group.SelectElementWithIndex(index);m_Group.DecideElement(); });
        }
    }

}
