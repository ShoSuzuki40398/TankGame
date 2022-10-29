using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

/// <summary>
/// シーン状態の遷移制御を行う
/// </summary>
public class MainSceneController : SingletonMonoBehaviour<MainSceneController>
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
    private StateMachine<MainSceneController,Scene_State> m_StateMachine = new StateMachine<MainSceneController, Scene_State>();
    public StateMachine<MainSceneController, Scene_State> StateMachine { get { return m_StateMachine; }}

    // BGMプレイヤー
    [SerializeField]
    private BackgroundMusicPlayer m_BgmPlayer;
    public BackgroundMusicPlayer BgmPlayer { get { return m_BgmPlayer; } }

    // シーン演出用タイムライン制御
    // シーン開始時演出
    [SerializeField]
    private ScenePerformanceController m_ScenePerformanceController;
    public ScenePerformanceController ScenePerformanceController { get { return m_ScenePerformanceController; } }

    // 戦闘終了時演出

    // 入力制御
    [SerializeField]
    private PlayerInput m_PlayerInput;


    // Start is called before the first frame update
    void Start()
    {
        // 
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

    /// <summary>
    /// シーン開始時演出の終了時イベント
    /// </summary>
    /// <param name="aDirector"></param>
    public void FinishSceneBeginPerformance()
    {
        // 戦闘開始状態に遷移する
        m_StateMachine.ChangeState(Scene_State.Battle_Start);
    }

    /// <summary>
    /// シーン開始状態
    /// </summary>
    private class SceneBegin : State<MainSceneController>
    {
        public SceneBegin(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            // 入力無効
            //owner.m_PlayerInput.Disable();

            Debug.Log("SceneBegin");
            // シーン開始演出を再生する
            // 終了時のイベントはシグナルで設定しておく
            // 次の状態（戦闘開始状態）への遷移は終了時イベントで行う
            //owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Scene_Start);
            owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Battle_End);
        }

        public override void Exit()
        {
            // 入力有効
            owner.m_PlayerInput.Enable();
        }
    }

    /// <summary>
    /// 戦闘開始状態
    /// </summary>
    private class BattleStart : State<MainSceneController>
    {
        public BattleStart(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("BattleStart");
            owner.BgmPlayer.Play();
        }
    }
    /// <summary>
    /// 戦闘中状態
    /// </summary>
    private class Battle : State<MainSceneController>
    {
        public Battle(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Battle");
        }
    }
    /// <summary>
    /// 戦闘終了状態
    /// </summary>
    private class BattleFinish : State<MainSceneController>
    {
        public BattleFinish(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("BattleFinish");
        }
    }
    /// <summary>
    /// 戦闘結果状態
    /// </summary>
    private class BattleResult : State<MainSceneController>
    {
        public BattleResult(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("BattleResult");
        }
    }
    /// <summary>
    /// シーン終了状態
    /// </summary>
    private class SceneEnd : State<MainSceneController>
    {
        public SceneEnd(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("SceneEnd");
        }
    }
}
