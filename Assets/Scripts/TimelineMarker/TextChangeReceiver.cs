using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

/// <summary>
/// TextChangeMarkerのレシーバー
/// </summary>
public class TextChangeReceiver : MonoBehaviour, INotificationReceiver
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

    /// <summary>
    /// 通知時イベント
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="notification"></param>
    /// <param name="context"></param>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as TextChangeMarker;
        if (marker == null)
            return;

        // テキスト変更
        // 変更するテキストはTimeline上に配置したTextChangeMarkerインスペクターにて設定
        this.ChangeText(marker.Text);
    }
}
