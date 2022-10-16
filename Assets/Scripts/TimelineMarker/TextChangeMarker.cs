using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// �J�X�^��TimelineMarker
/// Text�̕�����ύX����
/// </summary>
public class TextChangeMarker : Marker,INotification
{
    [SerializeField]
    private string text = "";
    public string Text => text;

    public PropertyName id => new PropertyName("TextChange");

}
