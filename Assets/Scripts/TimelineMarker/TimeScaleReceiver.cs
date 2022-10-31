using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// TimeScaleMarker�̃��V�[�o�[
/// </summary>
public class TimeScaleReceiver : MonoBehaviour, INotificationReceiver
{
    /// <summary>
    /// �e�L�X�g�ύX
    /// </summary>
    /// <param name="text"></param>
    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    /// <summary>
    /// �ʒm���C�x���g
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="notification"></param>
    /// <param name="context"></param>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as TimeScaleMarker;
        if (marker == null)
            return;

        // �e�L�X�g�ύX
        // �ύX����e�L�X�g��Timeline��ɔz�u����TextChangeMarker�C���X�y�N�^�[�ɂĐݒ�
        this.ChangeTimeScale(marker.TimeScale);
    }
}
