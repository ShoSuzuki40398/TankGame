using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ローディング制御
/// </summary>
[RequireComponent(typeof(LoadingText))]
public class LoadingNavigator : SingletonMonoBehaviour<LoadingNavigator>
{
    private LoadingText m_LoadingText;

    protected override void Init()
    {
        m_LoadingText = gameObject.GetComponent<LoadingText>();
        m_LoadingText.Init(transform);
    }

    public void StartLoading()
    {
        m_LoadingText.DisplayText();
    }

    public void StopLoading()
    {
        m_LoadingText.HideText();
    }
}
