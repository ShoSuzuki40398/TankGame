using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TextMeshPro�̃e�L�X�g�𓮓I�ɕύX����N���X
/// �ύX������TextMeshPro���A�^�b�`���ꂽ�I�u�W�F�N�g�ɃA�^�b�`����
/// </summary>
//[RequireComponent(typeof(TextMeshPro))]
public class TextChanger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;
    
    /// <summary>
    /// �e�L�X�g�ύX
    /// </summary>
    /// <param name="text"></param>
    public void ChangeText(string text)
    {
        m_TextMeshProUGUI.text = text;
    }
}
