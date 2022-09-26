using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// シーン状態の遷移制御を行う
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController>
{
    //　状態定義キー
    public enum Scene_State
    {
        Scene_Begin,    // シーン開始
        Battle_Start,   // 戦闘開始
        Battle,         // 戦闘中
        Battle_Finish,  // 戦闘終了
        Battle_Result,  // 戦闘結果
        Scene_End,      // シーン終了
    }

    // シーン状態ステートマシン
    [HideInInspector]
    public StateMachine<SceneController,Scene_State> m_StateMachine = new StateMachine<SceneController, Scene_State>();


    // Start is called before the first frame update
    void Start()
    {
        m_StateMachine.AddState(Scene_State.Scene_Begin, new SceneBegin(this));
        m_StateMachine.AddState(Scene_State.Battle_Start, new BattleStart(this));
        m_StateMachine.AddState(Scene_State.Battle, new Battle(this));
        m_StateMachine.AddState(Scene_State.Battle_Finish, new BattleFinish(this));
        m_StateMachine.AddState(Scene_State.Battle_Result, new BattleResult(this));
        m_StateMachine.AddState(Scene_State.Scene_End, new SceneEnd(this));

        m_StateMachine.ChangeState(Scene_State.Scene_Begin);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.Update();
    }
}

/// <summary>
/// シーン開始状態
/// </summary>
public class SceneBegin : State<SceneController>
{
    public SceneBegin(SceneController owner) : base(owner){}

    public override void Enter()
    {
        Debug.Log("SceneBegin");
        owner.m_StateMachine.ChangeState(SceneController.Scene_State.Battle_Start);
    }
}

/// <summary>
/// 戦闘開始状態
/// </summary>
public class BattleStart : State<SceneController>
{
    public BattleStart(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("BattleStart");
        owner.m_StateMachine.ChangeState(SceneController.Scene_State.Battle);
    }
}
/// <summary>
/// 戦闘中状態
/// </summary>
public class Battle : State<SceneController>
{
    public Battle(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("Battle");
        owner.m_StateMachine.ChangeState(SceneController.Scene_State.Battle_Finish);
    }
}
/// <summary>
/// 戦闘終了状態
/// </summary>
public class BattleFinish : State<SceneController>
{
    public BattleFinish(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("BattleFinish");
        owner.m_StateMachine.ChangeState(SceneController.Scene_State.Battle_Result);
    }
}
/// <summary>
/// 戦闘結果状態
/// </summary>
public class BattleResult : State<SceneController>
{
    public BattleResult(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("BattleResult");
        owner.m_StateMachine.ChangeState(SceneController.Scene_State.Scene_End);
    }
}
/// <summary>
/// シーン終了状態
/// </summary>
public class SceneEnd : State<SceneController>
{
    public SceneEnd(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("SceneEnd");
    }
}