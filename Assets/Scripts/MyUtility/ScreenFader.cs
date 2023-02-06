using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 画面のフェードイン・アウトを制御する
/// フェード用のキャンバスとImageオブジェクトを子において操作する
/// </summary>
public class ScreenFader : MonoBehaviour
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
    protected Image m_FadePanel = null;

    // 親キャンバス
    [SerializeField]
    protected Canvas m_FadeCanvas = null;

    // フェードインにかける時間
    [SerializeField]
    protected float m_FadeInTime = 1.0f;

    // フェードアウトにかける時間
    [SerializeField]
    protected float m_FadeOutTime = 1.0f;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Transform parent = null)
    {
        CreateFadeCanvasIfNeeded();
        CreateFadePanelIfNeeded();

        if(parent != null)
            m_FadeCanvas.transform.SetParent(parent);
    }

    /// <summary>
    /// フェード用キャンバス作成
    /// </summary>
    private void CreateFadeCanvasIfNeeded()
    {
        if (m_FadeCanvas != null)
            return;

        // 自動で作成する場合は最低限の処理になります
        var obj = new GameObject("FadeCanvas");
        var canvas = obj.AddComponent<Canvas>();
        // 前面に来るようにソートオーダーを大きな値にする
        canvas.sortingOrder = 999;
        // レンダーモードをScreenSpaceOverlayにする
        // フェード処理はカメラではなくゲームのシステムとして運用するため
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

        var scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // ※フェード処理にルールを使う場合はinspectorから変更できた方が良いかも
        scaler.referenceResolution = new Vector2(800, 600);

        m_FadeCanvas = canvas;
    }

    /// <summary>
    /// フェード用パネル作成
    /// </summary>
    private void CreateFadePanelIfNeeded()
    {
        if (m_FadePanel != null)
            return;

        // インスタンス作成
        var obj = new GameObject("FadePanel");
        var rect = obj.AddComponent<RectTransform>();

        // 色
        m_FadePanel = obj.AddComponent<Image>();
        m_FadePanel.color = Color.black;

        // タッチ検出しない
        m_FadePanel.raycastTarget = false;

        // 親子設定
        // パネルだけシーンに存在してキャンバスは自動生成の時は想定していないので注意
        rect.SetParent(m_FadeCanvas.transform);

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
    public void SetAlpha(float a)
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
        
        state = FADE_STATE.FADEOUT;

        StartCoroutine(Fade(m_FadeOutTime, 0.0f, 1.0f, action));
    }
}
