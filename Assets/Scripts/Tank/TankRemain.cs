using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 残機制御
/// </summary>
public class TankRemain : MonoBehaviour
{
    // 初期残機数
    [SerializeField]
    private int m_InitRemainCount = 3;
    
    // 現在残機数
    private int m_CurrentRemainCount = 0;

    // 残機消費時イベント
    [SerializeField]
    private UnityEvent OnSubRemain;

    // 残機全消費時イベント
    [SerializeField]
    private UnityEvent OnLostAllRemain;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 残機初期化
    /// </summary>
    public void Initialize()
    {
        SetRemain(m_InitRemainCount);
    }

    /// <summary>
    /// 指定した数の現在残機数を増やす
    /// </summary>
    /// <param name="count"></param>
    public void AddRemain(int count)
    {
        SetRemain(m_CurrentRemainCount + count);
    }

    /// <summary>
    /// 指定した数の現在残機数を減らす
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        SetRemain(m_CurrentRemainCount - count);
        OnSubRemain?.Invoke();

        // 減らした結果,残機が0になったら全消費イベント発火
        if (m_CurrentRemainCount == 0)
            OnLostAllRemain?.Invoke();
    }

    /// <summary>
    /// 現在残機数を設定
    /// 以下は設定できない
    /// ・初期残機数を超える数
    /// ・0未満
    /// </summary>
    public void SetRemain(int remain)
    {
        int clamp = Mathf.Clamp(remain, 0, m_InitRemainCount);
        m_CurrentRemainCount = remain;
    }


}
