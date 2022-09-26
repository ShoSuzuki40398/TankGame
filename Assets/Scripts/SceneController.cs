using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �V�[����Ԃ̑J�ڐ�����s��
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController>
{
    //�@��Ԓ�`�L�[
    public enum Scene_State
    {
        Scene_Begin,    // �V�[���J�n
        Battle_Start,   // �퓬�J�n
        Battle,         // �퓬��
        Battle_Finish,  // �퓬�I��
        Battle_Result,  // �퓬����
        Scene_End,      // �V�[���I��
    }

    // �V�[����ԃX�e�[�g�}�V��
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
/// �V�[���J�n���
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
/// �퓬�J�n���
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
/// �퓬�����
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
/// �퓬�I�����
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
/// �퓬���ʏ��
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
/// �V�[���I�����
/// </summary>
public class SceneEnd : State<SceneController>
{
    public SceneEnd(SceneController owner) : base(owner) { }

    public override void Enter()
    {
        Debug.Log("SceneEnd");
    }
}