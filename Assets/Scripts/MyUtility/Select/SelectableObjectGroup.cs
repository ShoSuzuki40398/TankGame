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
        foreach(var ele in m_Selectables)
        {
            ele.m_Group = this;
        }
    }

    private void Start()
    {
        // 識別番号番号割り当て
        AssignmentIndex();

        // 最上位の選択肢を選択した状態から開始
        SelectElement(m_CurrentSelectedElementIndex);
    }
    
    /// <summary>
    /// 各選択肢に識別IDを設定する
    /// 最初の選択肢を0とし、昇順で割り当てる
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
    /// カーソルでフォーカスした時
    /// </summary>
    private void FocusElement(int index)
    {
        SelectElementWithIndex(index);
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
        // 同値チェック
        if (m_CurrentSelectedElementIndex == index)
            return;

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

    /// <summary>
    /// 選択していない選択肢の取得
    /// </summary>
    /// <returns></returns>
    public List<BaseSelectableElement> UnSelectElements()
    {
        List<BaseSelectableElement> res = new List<BaseSelectableElement>();

        return m_Selectables.FindAll(ele => ele.index != m_CurrentSelectedElementIndex);
    }


    /// <summary>
    /// 選択していない選択肢をノーマル状態にする
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
