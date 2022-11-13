using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 画面のフェードイン・アウトを制御する
/// 
/// </summary>
public class FadeController : SingletonMonoBehaviour<FadeController>
{
    enum FADE_STATE
    {
        IDLE,   // 待機
        FADEIN, // フェードイン
        FADEOUT, // フェードアウト
    }
    FADE_STATE state = FADE_STATE.IDLE;

    // フェード用パネル
    [SerializeField]
    protected Image m_FadePanel;

    // 親キャンバス
    [SerializeField]
    protected GameObject m_ParentCanvas;

    // フェードインにかける時間
    [SerializeField]
    protected float m_FadeInTime = 1.0f;

    // フェードアウトにかける時間
    [SerializeField]
    protected float m_FadeOutTime = 1.0f;

    /// <summary>
    /// 起動時処理
    /// </summary>
    protected override void ActionInAwake()
    {
        // フェード用パネルが、起動時にインスペクターから設定していない場合は自動で作成
        CreateFadePanel();
    }

    /// <summary>
    /// フェード用パネル作成
    /// ※作成済みの場合は何もしないため、実行中に見た目が変わるなどは想定していない
    /// </summary>
    private void CreateFadePanel()
    {
        if (m_FadePanel != null)
            return;

        // 自動で作成する場合は最低限の処理になります
        // (親子設定、色、サイズ、座標)
        var canvas = m_ParentCanvas;
        if(canvas == null)
            canvas = GameObject.Find("FadeCanvas");
        
        // インスタンス作成
        var obj = new GameObject("FadePanel");
        var rect = obj.AddComponent<RectTransform>();

        // 色
        m_FadePanel = obj.AddComponent<Image>();
        m_FadePanel.color = Color.black;

        //親子設定
        rect.SetParent(canvas.transform);

        // サイズ
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        // 座標
        rect.anchoredPosition = Vector2.zero;

        SetAlpha(0.0f);
    }
       
    /// <summary>
    /// フェード
    /// </summary>
    /// <param name="fadeTime">フェードにかける時間</param>
    /// <param name="startAlpha">開始アルファ値</param>
    /// <param name="endAlpha">終了アルファ値</param>
    /// <param name="action">フェード終了時イベント</param>
    /// <returns></returns>
    private IEnumerator Fade(float fadeTime, float startAlpha, float endAlpha, Action action = null)
    {
        m_FadePanel.gameObject.SetActive(true);

        float elapsedTime = 0.0f;

        // フェード開始
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            var currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            SetAlpha(currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        // フェード後のイベント発火
        action?.Invoke();

        // フェードインの場合は非アクティブにする
        // マウス座標等でレイキャストを行う場合、フェードパネルが前面にあると邪魔になるため
        if (state == FADE_STATE.FADEIN)
        {
            m_FadePanel.gameObject.SetActive(false);
        }

        // 待機状態に戻しておく
        state = FADE_STATE.IDLE;
    }

    /// <summary>
    /// アルファ値設定
    /// </summary>
    /// <param name="a"></param>
    private void SetAlpha(float a)
    {
        var color = m_FadePanel.color;
        color.a = a;
        m_FadePanel.color = color;
    }

    /// <summary>
    /// フェードイン開始
    /// </summary>
    /// <param name="time"></param>
    public void FadeIn(Action action = null)
    {
        // フェード中は無効
        if (state != FADE_STATE.IDLE)
        {
            return;
        }

        if (m_FadePanel == null)
        {
            CreateFadePanel();
        }

        state = FADE_STATE.FADEIN;

        StartCoroutine(Fade(m_FadeInTime, 1.0f, 0.0f, action));
    }

    /// <summary>
    /// フェードアウト開始
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(Action action = null)
    {
        // フェード中は無効
        if (state != FADE_STATE.IDLE)
        {
            return;
        }

        if (m_FadePanel == null)
        {
            CreateFadePanel();
        }

        state = FADE_STATE.FADEOUT;

        StartCoroutine(Fade(m_FadeOutTime, 0.0f, 1.0f, action));
    }
}
