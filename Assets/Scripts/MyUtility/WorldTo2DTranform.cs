using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���[���h���W��2D�ϊ����郆�[�e�B���e�B�N���X
/// </summary>
public class WorldTo2DTranform : MonoBehaviour
{
    /// <summary>
    /// 3D���W��2D���W�ϊ�
    /// �L�����o�X�̃����_�[���[�h�ŕϊ����@�𕪊򂵂܂�
    /// RenderMode��WorldSpace�̎��͕ϊ����ʂ͕Ԃ��Ȃ����ߏ璷�ɂȂ��Ă��܂�
    /// </summary>
    /// <returns></returns>
    public static Vector2 Transform(Vector3 target,Canvas canvas,Camera camera)
    {
        Vector2 res = Vector2.zero;

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                res = TransformFromOverlay(target, camera);
                break;
            case RenderMode.ScreenSpaceCamera:
                res = TransformFromCamera(target, canvas, camera);
                break;
            case RenderMode.WorldSpace:
                TransformFromWorld(canvas, camera);
                break;
        }
        return res;
    }

    /// <summary>
    /// Canvas��RenderMode��ScreenSpaceOverlay�̎��̕ϊ��֐�
    /// �V���v���Ƀ��[���h���W����X�N���[�����W�ɕϊ�����
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static Vector2 TransformFromOverlay(Vector3 target,Camera camera)
    {
        Vector2 res = Vector2.zero;

        res = RectTransformUtility.WorldToScreenPoint(camera, target);

        return res;
    }

    /// <summary>
    /// Canvas��RenderMode��ScreenSpaceCamera�̎��̕ϊ��֐�
    /// �V���v���Ƀ��[���h���W����X�N���[�����W�ɕϊ����A
    /// ����ɃJ�����̕`��p����Ƃ������[�J�����W�ɕϊ�����
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static Vector2 TransformFromCamera(Vector3 target, Canvas canvas, Camera camera)
    {
        Vector2 res = Vector2.zero;
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, target);

        // �X�N���[�����W�����[�J�����W�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, camera, out res);
        
        return res;
    }

    /// <summary>
    /// Canvas��RenderMode��WorldSpace�̎��̕ϊ��֐�
    /// ���W�̕ϊ��͂����A��]�����J�����̕��Ɍ������邾��
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static void TransformFromWorld(Canvas canvas, Camera camera)
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // �L�����o�X���J�����̕��Ɍ������邾��
        canvasRectTransform.LookAt(camera.transform);
    }
}
