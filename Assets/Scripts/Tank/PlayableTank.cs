using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// マニュアル入力による戦車制御
/// プレイヤーが操作する戦車にアタッチする
/// </summary>
public class PlayableTank : MonoBehaviour, IPlayerInput
{
    private TankMovement m_TankMovement;
    private TankShooting m_TankShooting;
    private PlayerInput m_PlayerInput;

    // 移動量の入力値
    private Vector2 m_InputMoveVector = Vector2.zero;
    
    // 戦車本体
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    // 可動フラグ
    private bool m_IsMovement = true;

    private void Awake()
    {
        m_TankMovement = GetComponentInChildren<TankMovement>();
        m_TankShooting = GetComponentInChildren<TankShooting>();
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();

        // Move入力イベントにOnMoveを追加
        m_PlayerInput.actionEvents[0].AddListener(OnMove);
        // Fire入力イベントにOnFireを追加
        m_PlayerInput.actionEvents[1].AddListener(OnFire);
    }
    
    private void FixedUpdate()
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
    /// 可動フラグ設定
    /// </summary>
    public void SetIsMovement(bool flag)
    {
        m_IsMovement = flag;
    }

    /// <summary>
    /// 移動入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!m_IsMovement)
        {
            m_InputMoveVector = Vector2.zero;
            return;
        }

        m_InputMoveVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 決定入力時イベント
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!m_IsMovement)
            return;

        // 押した瞬間だけ有効
        // Was○○ThisFrameがないと他のphaseも拾って1度に複数回呼ばれるので注意する
        // started
        // performed ← ここで射撃入力とする
        // release
        if (context.action.WasPerformedThisFrame())
        {
            m_TankShooting.Fire();
        }
    }

    private void OnDestroy()
    {
        // Move入力イベントにOnMoveを追加
        m_PlayerInput.actionEvents[0].RemoveAllListeners();
        // Fire入力イベントにOnFireを追加
        m_PlayerInput.actionEvents[1].RemoveAllListeners();
    }
}
