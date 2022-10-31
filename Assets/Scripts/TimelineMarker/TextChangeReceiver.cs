using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

/// <summary>
/// TextChangeMarker�̃��V�[�o�[
/// </summary>
public class TextChangeReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    private TextMeshProUGUI m_TextMeshProUGUI;

    /// <summary>
    /// �e�L�X�g�ύX
    /// </summary>
    /// <param name="text"></param>
    public void ChangeText(string text)
    {
        m_TextMeshProUGUI.text = text;
    }

    /// <summary>
    /// �ʒm���C�x���g
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="notification"></param>
    /// <param name="context"></param>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as TextChangeMarker;
        if (marker == null)
            return;

        // �e�L�X�g�ύX
        // �ύX����e�L�X�g��Timeline��ɔz�u����TextChangeMarker�C���X�y�N�^�[�ɂĐݒ�
        this.ChangeText(marker.Text);
    }
}
