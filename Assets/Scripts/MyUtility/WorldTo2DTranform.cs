using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワールド座標を2D変換するユーティリティクラス
/// </summary>
public class WorldTo2DTranform : MonoBehaviour
{
    /// <summary>
    /// 3D座標→2D座標変換
    /// キャンバスのレンダーモードで変換方法を分岐します
    /// RenderModeがWorldSpaceの時は変換結果は返さないため冗長になっています
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
    /// CanvasのRenderModeがScreenSpaceOverlayの時の変換関数
    /// シンプルにワールド座標からスクリーン座標に変換する
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
    /// CanvasのRenderModeがScreenSpaceCameraの時の変換関数
    /// シンプルにワールド座標からスクリーン座標に変換し、
    /// さらにカメラの描画角を基準としたローカル座標に変換する
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static Vector2 TransformFromCamera(Vector3 target, Canvas canvas, Camera camera)
    {
        Vector2 res = Vector2.zero;
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // ワールド座標をスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, target);

        // スクリーン座標をローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, camera, out res);
        
        return res;
    }

    /// <summary>
    /// CanvasのRenderModeがWorldSpaceの時の変換関数
    /// 座標の変換はせず、回転だけカメラの方に向かせるだけ
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static void TransformFromWorld(Canvas canvas, Camera camera)
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // キャンバスをカメラの方に向かせるだけ
        canvasRectTransform.LookAt(camera.transform);
    }
}
