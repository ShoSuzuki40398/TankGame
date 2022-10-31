using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// カスタムTimelineMarker
/// タイムスケールを変更する
/// </summary>
public class TimeScaleMarker : Marker,INotification
{
    [SerializeField]
    private float timeScale = 0f;
    public float TimeScale => timeScale;

    public PropertyName id => new PropertyName("TimeScaleChange");
}
