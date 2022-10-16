using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TextMeshProのテキストを動的に変更するクラス
/// 変更したいTextMeshProがアタッチされたオブジェクトにアタッチする
/// </summary>
//[RequireComponent(typeof(TextMeshPro))]
public class TextChanger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;
    
    /// <summary>
    /// テキスト変更
    /// </summary>
    /// <param name="text"></param>
    public void ChangeText(string text)
    {
        m_TextMeshProUGUI.text = text;
    }
}
