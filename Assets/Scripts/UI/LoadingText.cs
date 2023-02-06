using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// ローディング進行テキスト表示
/// </summary>
public class LoadingText : MonoBehaviour
{
    // 親キャンバス
    [SerializeField]
    private Canvas m_Canvas = null;
    private TextMeshProUGUI m_TextMesh;

    private string loadingStr = "Loading";
    private const float textUpdateInterval = 0.7f;
    private const int maxEllipseCount = 4;

    public bool IsUpdate = false;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Transform parent = null)
    {
        CreateCanvasIfNeeded();
        CreateTextIfNeeded();

        if (parent != null)
            m_Canvas.transform.SetParent(parent);
    }
    /// <summary>
    /// フェード用キャンバス作成
    /// </summary>
    private void CreateCanvasIfNeeded()
    {
        if (m_Canvas != null)
            return;

        // 自動で作成する場合は最低限の処理になります
        var obj = new GameObject("LoadingCanvas");
        var canvas = obj.AddComponent<Canvas>();
        // 前面に来るようにソートオーダーを大きな値にする
        canvas.sortingOrder = 1000;
        // レンダーモードをScreenSpaceOverlayにする
        // フェード処理はカメラではなくゲームのシステムとして運用するため
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

        var scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // ※フェード処理にルールを使う場合はinspectorから変更できた方が良いかも
        scaler.referenceResolution = new Vector2(800, 600);

        m_Canvas = canvas;
    }

    /// <summary>
    /// フェード用パネル作成
    /// </summary>
    private void CreateTextIfNeeded()
    {
        if (m_TextMesh != null)
            return;

        // インスタンス作成
        var obj = new GameObject("LodingText");
        var rect = obj.AddComponent<RectTransform>();
        m_TextMesh = obj.AddComponent<TextMeshProUGUI>();

        m_TextMesh.verticalAlignment = VerticalAlignmentOptions.Middle;

        // タッチ検出しない
        m_TextMesh.raycastTarget = false;

        // 親子設定
        // パネルだけシーンに存在してキャンバスは自動生成の時は想定していないので注意
        rect.SetParent(m_Canvas.transform);

        // 座標
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = Vector2.zero;

        // サイズ
        rect.sizeDelta = new Vector2(200, 50);
    }

    /// <summary>
    /// ローディングテキスト更新
    /// </summary>
    /// <returns></returns>
    private async UniTask UpdateEllipse()
    {
        int count = 0;
        while (IsUpdate)
        {
            string elipseStr = new string('.', count);

            m_TextMesh.text = loadingStr + elipseStr;

            count++;
            count = Mathf.Clamp(count, 0, maxEllipseCount);
            if (count >= maxEllipseCount)
                count = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(textUpdateInterval));
        }
    }

    /// <summary>
    /// テキスト表示
    /// 表示と同時にテキストの更新を開始する
    /// 更新を終了する時は下のHideTextを呼ぶ
    /// </summary>
    public async void DisplayText()
    {
        if (IsUpdate)
            return;

        IsUpdate = true;
        gameObject.Enable();
        await UpdateEllipse();
    }

    /// <summary>
    /// テキスト非表示
    /// </summary>
    public void HideText()
    {
        if (!IsUpdate)
            return;

        IsUpdate = false;
        gameObject.Disable();
    }
}
