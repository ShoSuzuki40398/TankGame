using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// カスタムTimelineMarker
/// Textの文字を変更する
/// </summary>
public class TextChangeMarker : Marker,INotification
{
    [SerializeField]
    private string text = "";
    public string Text => text;

    public PropertyName id => new PropertyName("TextChange");

}
