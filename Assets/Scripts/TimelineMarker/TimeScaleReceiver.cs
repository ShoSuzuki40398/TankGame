using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// TimeScaleMarkerのレシーバー
/// </summary>
public class TimeScaleReceiver : MonoBehaviour, INotificationReceiver
{
    /// <summary>
    /// テキスト変更
    /// </summary>
    /// <param name="text"></param>
    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    /// <summary>
    /// 通知時イベント
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="notification"></param>
    /// <param name="context"></param>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as TimeScaleMarker;
        if (marker == null)
            return;

        // テキスト変更
        // 変更するテキストはTimeline上に配置したTextChangeMarkerインスペクターにて設定
        this.ChangeTimeScale(marker.TimeScale);
    }
}
