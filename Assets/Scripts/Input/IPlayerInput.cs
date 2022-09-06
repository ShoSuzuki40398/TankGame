using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー入力イベント
/// </summary>
public interface IPlayerInput
{
    /// <summary>
    /// 移動入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    void OnMove(InputAction.CallbackContext context);

    /// <summary>
    /// 決定入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    void OnFire(InputAction.CallbackContext context);
}
