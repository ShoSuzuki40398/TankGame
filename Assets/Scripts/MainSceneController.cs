using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;

/// <summary>
/// シーン状態の遷移制御を行う
/// </summary>
public class MainSceneController : MonoBehaviour
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
    private StateMachine<MainSceneController, Scene_State> m_StateMachine = new StateMachine<MainSceneController, Scene_State>();
    public StateMachine<MainSceneController, Scene_State> StateMachine { get { return m_StateMachine; } }
    
    // シーン演出用タイムライン制御
    [SerializeField]
    private ScenePerformanceController m_ScenePerformanceController;
    public ScenePerformanceController ScenePerformanceController { get { return m_ScenePerformanceController; } }

    // 入力制御
    [SerializeField]
    private PlayerInput m_PlayerInput;

    // 入力マップキー
    private readonly string m_UIActionMapKey = "UI";
    private readonly string m_PlayerActionMapKey = "Player";

    // UI入力アクションキー
    private readonly string m_EnterActionKey = "Enter";
    private InputAction m_EnterAction { get { return m_PlayerInput.currentActionMap[m_EnterActionKey]; } }

    // インゲームキャンバス
    [SerializeField]
    private Canvas m_IngameCanvas;

    // リザルトキャンバス
    [SerializeField]
    private Canvas m_ResultCanvas;

    // リザルト制御
    [SerializeField]
    private ResultBehaviour m_ResultBehaviour;

    // リザルトメニュー
    [SerializeField]
    private SelectableObjectGroup m_ResultManuGroup;

    // プレイヤータンク
    private PlayableTank m_PlayerTank;

    // 敵タンク
    private AutomationTank m_EnemyTank;

    // インゲーム中の追従カメラ
    [SerializeField]
    private IngameCameras m_IngameCameras;

    // カメラ振動
    [SerializeField]
    private ShakeCamera m_ShakeCamera;


    // Start is called before the first frame update
    void Start()
    {
        // 状態登録
        m_StateMachine.AddState(Scene_State.Scene_Begin, new SceneBegin(this));
        m_StateMachine.AddState(Scene_State.Battle_Start, new BattleStart(this));
        m_StateMachine.AddState(Scene_State.Battle, new Battle(this));
        m_StateMachine.AddState(Scene_State.Battle_Finish, new BattleFinish(this));
        m_StateMachine.AddState(Scene_State.Battle_Result, new BattleResult(this));
        m_StateMachine.AddState(Scene_State.Scene_End, new SceneEnd(this));
        
        // レベルアート読込時にシーン開始状態へ遷移する
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
    public void FinishSceneBeginPerformance()
    {
        // 戦闘開始状態に遷移する
        m_StateMachine.ChangeState(Scene_State.Battle_Start);
    }

    /// <summary>
    /// シーン開始時演出の終了時イベント
    /// </summary>
    public void FinishBattleEndPerformance()
    {
        // 戦闘結果状態に遷移する
        m_StateMachine.ChangeState(Scene_State.Battle_Result);
    }

    /// <summary>
    /// 戦闘終了
    /// </summary>
    public void ToBattleEnd()
    {
        m_StateMachine.ChangeState(Scene_State.Battle_Finish);
    }

    /// <summary>
    /// シーン初期化
    /// </summary>
    private void SceneInitialize()
    {
        // 入力無効
        m_PlayerInput.Disable();

        // 各キャンバス非表示
        m_IngameCanvas.Disable();
        m_ResultCanvas.Disable();

        // 敵の停止
        m_EnemyTank.Disable();
    }

    private void SceneFinalize()
    {
        // 入力無効
        m_PlayerInput.Disable();

        // 各キャンバス非表示
        m_IngameCanvas.Disable();
        m_ResultCanvas.Disable();

        // BGM停止
        BackgroundMusicPlayer.Instance.Stop();
    }


    /// <summary>
    /// シーンを終了する
    /// </summary>
    public void ToSceneEnd()
    {
        StateMachine.ChangeState(Scene_State.Scene_End);
    }

    /// <summary>
    /// 入力マップ切替
    /// </summary>
    /// <param name="key"></param>
    private void ChangeInputActionMap(string key)
    {
        var actionMap = m_PlayerInput.actions.FindActionMap(key);

        if (actionMap != null)
        {
            m_PlayerInput.currentActionMap = actionMap;
        }
    }

    /// <summary>
    /// シーン開始状態
    /// </summary>
    private class SceneBegin : State<MainSceneController>
    {
        public SceneBegin(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("SceneBegin");
            // レベルアート作成
            var property = IngameSetting.Instance.CurrentLevelArtProperty;
                LevelArtLoader.Instance.InstantiateFromProperty(property.LevelArtType, () =>
                {
                    owner.m_PlayerTank = LevelArtLoader.Instance.PlayableTank;
                    owner.m_EnemyTank = LevelArtLoader.Instance.EnemyTank;

                    // プレイヤーをターゲットとして設定
                    owner.m_EnemyTank.SetTarget(owner.m_PlayerTank.Tank.transform);

                    // インゲームカメラ設定
                    owner.m_IngameCameras.SettingIngameCamera(owner.m_PlayerTank.GetComponentInChildren<TankMovement>().transform, owner.m_EnemyTank.GetComponentInChildren<TankMovement>().transform);

                    // 破壊時イベント設定
                    owner.m_PlayerTank.GetComponentInChildren<Damageable>().OnDie.AddListener((damager, damageable) => { owner.m_ShakeCamera.Shake(); });
                    owner.m_EnemyTank.GetComponentInChildren<Damageable>().OnDie.AddListener((damager, damageable) => { owner.m_ShakeCamera.Shake(); });

                    // 残機全消費時イベント設定
                    TankRemain playerRemain = owner.m_PlayerTank.GetComponentInChildren<TankRemain>();
                    TankRemain enemyRemain = owner.m_EnemyTank.GetComponentInChildren<TankRemain>();
                    playerRemain.OnLostAllRemain.AddListener(() => { owner.m_ResultBehaviour.SetResultType(ResultBehaviour.ResultType.PlayerLose); });
                    playerRemain.OnLostAllRemain.AddListener(owner.ToBattleEnd);

                    enemyRemain.OnLostAllRemain.AddListener(() => { owner.m_ResultBehaviour.SetResultType(ResultBehaviour.ResultType.PlayerWin); });
                    enemyRemain.OnLostAllRemain.AddListener(owner.ToBattleEnd);

                    // リザルト設定
                    owner.m_ResultBehaviour.OnPrePlayerWin.AddListener(() => { owner.m_ResultBehaviour.SetResultCameraTraget(owner.m_PlayerTank.Tank.transform); });
                    owner.m_ResultBehaviour.OnPrePlayerLose.AddListener(() => { owner.m_ResultBehaviour.SetResultCameraTraget(owner.m_EnemyTank.Tank.transform); });
                    
                    // シーン初期化
                    owner.SceneInitialize();

                    // ローディングテキスト非表示
                    //LoadingNavigator.Instance.StopLoading();

                    // フェードイン開始
                    SceneNavigator.Instance.FadeIn(()=>{
                        // シーン開始演出を再生する
                        // 終了時のイベントはシグナルで設定しておく
                        // 次の状態（戦闘開始状態）への遷移は終了時イベントで行う(FinishSceneBeginPerformance)
                        owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Scene_Start);
                    });
                });
        }

        public override void Exit()
        {
            // 入力を有効にし、プレイヤー操作用の入力に切り替える
            owner.ChangeInputActionMap(owner.m_PlayerActionMapKey);
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
            // BGM再生
            BackgroundMusicPlayer.Instance.Play();

            // インゲームキャンバス表示
            owner.m_IngameCanvas.Enable();

            // 敵の起動
            owner.m_EnemyTank.Enable();
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
            // 入力無効
            owner.m_PlayerInput.Disable();

            // インゲームキャンバス非表示
            owner.m_IngameCanvas.Disable();

            // リザルト前準備
            // 勝敗結果は残機制御から送る
            owner.m_ResultBehaviour.ResultSetUp();

            // 敵停止
            owner.m_EnemyTank.Disable();

            // 戦闘終了演出を再生する
            // 終了時のイベントはシグナルで設定しておく
            // 次の状態（戦闘結果状態）への遷移は終了時イベントで行う(FinishBattleEndPerformance)
            owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Battle_End);
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
            // リザルトキャンバス表示
            owner.m_ResultCanvas.Enable();

            // 入力を有効にし、UI操作用の入力に切り替える
            owner.ChangeInputActionMap(owner.m_UIActionMapKey);
            owner.m_PlayerInput.Enable();

            // リザルト演出再生
            owner.m_ResultBehaviour.ResultExecute();
        }

        public override void Execute()
        {
            UpdateResultManu();
        }

        /// <summary>
        /// リザルトメニュー更新
        /// </summary>
        private void UpdateResultManu()
        {
            if (owner.m_EnterAction == null)
                return;

            // 決定
            if (owner.m_EnterAction.WasPressedThisFrame())
            {
                owner.m_ResultManuGroup.DecideElement();
            }
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

            // リザルトキャンバス非表示
            owner.SceneFinalize();

            // タイトルバック
            SceneNavigator.Instance.Transition(CommonDefineData.SceneNameTitle);
        }
    }
}
