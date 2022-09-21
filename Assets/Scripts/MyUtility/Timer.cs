using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間計測クラス
/// </summary>
public class Timer
{
    // 経過時間
    private float m_CurrentTime = 0.0f;
    public float CurrentTime { get { return m_CurrentTime; } }

    // 目標時間
    private float m_EndTime = 0.0f;

    // タイマー起動フラグ(trueで動く)
    private bool m_IsAwake = false;

    // タイマー完了時実行処理
    public Action OnComplete = null;

    /// <summary>
    /// タイマー起動
    /// </summary>
    /// <param name="end">目標時間設定</param>
    /// <param name="offset">経過時間オフセット(基本は0秒経過指定)</param>
    public void Awake(float end,float offset = 0.0f)
    {
        m_CurrentTime = offset;
        m_EndTime = end;
        m_IsAwake = true;
    }

    /// <summary>
    /// Time.deltaTimeにより経過時間を更新
    /// </summary>
    /// <returns>指定した経過時間を超えた場合はtrueを返す</returns>
    public bool UpdateWithDeltaTime()
    {
        bool res = false;

        // そもそも起動していないなら更新しない
        if (!m_IsAwake) return res;

        // deltaTimeで経過時間を計測
        m_CurrentTime += Time.deltaTime;
        if(m_CurrentTime > m_EndTime)
        {
            res = true;
            // 終了時の関数を実行
            OnComplete?.Invoke();
            // 一度計測を終了したらリセット
            // ※ループしたい場合は別途処理を修正する
            Reset();
        }

        return res;
    }

    /// <summary>
    /// タイマーリセット
    /// </summary>
    public void Reset()
    {
        m_IsAwake = false;
        m_CurrentTime = 0.0f;
    }
}
