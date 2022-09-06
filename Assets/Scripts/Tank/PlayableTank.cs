using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// マニュアル入力による戦車制御
/// プレイヤーが操作する戦車にアタッチする
/// </summary>
public class PlayableTank : MonoBehaviour,IPlayerInput
{
    [SerializeField]
    private TankMovement m_TankMovement;

    // 移動量の入力値
    private Vector2 m_InputMoveVector = Vector2.zero;

    private void Awake()
    {
    }

    private void Update()
    {
        // 移動と回転
        Movement();
    }

    /// <summary>
    /// 入力値をもとに移動と回転を行う
    /// </summary>
    private void Movement()
    {
        m_TankMovement.Move(m_InputMoveVector.y);
        m_TankMovement.Turn(m_InputMoveVector.x);
    }

    /// <summary>
    /// 移動入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        m_InputMoveVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 決定入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
    }
}
