using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[���̓C�x���g
/// </summary>
public interface IPlayerInput
{
    /// <summary>
    /// �ړ����͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    void OnMove(InputAction.CallbackContext context);

    /// <summary>
    /// ������͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    void OnFire(InputAction.CallbackContext context);
}
