using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ISelectableを継承している要素を操作する
/// 操作対象はインスタンスのUDID(GetInstanceID)で判別する
/// </summary>
public class SelectableObjectGroup : MonoBehaviour
{
    // 操作対象
    [SerializeField]
    protected List<BaseSelectableElement> m_Selectables;
    protected Dictionary<int, BaseSelectableElement> m_SelectablePairs = new Dictionary<int, BaseSelectableElement>();

    public BaseSelectableElement currentSelectedElement { get { return m_Selectables[m_CurrentSelectedElementIndex]; } }

    // 現在選択している選択肢のインデックス
    // 初期値は最上位の選択肢
    protected int m_CurrentSelectedElementIndex = 0;

    private void Awake()
    {
        m_SelectablePairs = m_Selectables.ToDictionary(ele => ele.gameObject.GetInstanceID());
    }

    private void Start()
    {
        // 最上位の選択肢を選択した状態から開始
        SelectElementWithIndex(m_CurrentSelectedElementIndex);
    }

    /// <summary>
    /// 選択する
    /// </summary>
    /// <param name="index"></param>
    private void SelectElement(int index)
    {
        // 範囲チェック
        if (!index.IsRange(0, m_Selectables.Count - 1))
            return;

        // 現在選択しているオブジェクトをNormal状態に戻し、新規に選択されたオブジェクトをSelected状態にする
        currentSelectedElement.Normal();
        m_CurrentSelectedElementIndex = index;
        currentSelectedElement.Selected();
    }

    /// <summary>
    /// 選択中の選択肢で決定する
    /// </summary>
    public void DecideElement()
    {
        currentSelectedElement.Decided();
    }

    /// <summary>
    /// 現在位置から選択肢を指定して移動する
    /// </summary>
    /// <param name="index"></param>
    public void SelectElementWithIndex(int index)
    {
        SelectElement(index);
    }

    /// <summary>
    /// 現在位置から相対的に移動する
    /// 上位選択肢に移動するときは正の値(1)
    /// 下位選択肢に移動するときは負の値(-1)
    /// </summary>
    /// <param name="value"></param>
    public void SelectElementWithValue(int value = 0)
    {
        int newIndex = m_CurrentSelectedElementIndex + value;

        SelectElement(newIndex);
    }

    /// <summary>
    /// 現在位置より上位の選択肢を選択
    /// </summary>
    /// <param name="value">選択移動量</param>
    public void SelectUpperElement(int value = 1)
    {
        SelectElementWithIndex(m_CurrentSelectedElementIndex + value);
    }

    /// <summary>
    /// 現在位置より下位の選択肢を選択
    /// </summary>
    /// <param name="value">選択移動量</param>
    public void SelectLowerElement(int value = 1)
    {
        SelectElementWithIndex(m_CurrentSelectedElementIndex - value);
    }
}
