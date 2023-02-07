using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;

/// <summary>
/// �V�[����Ԃ̑J�ڐ�����s��
/// </summary>
public class MainSceneController : MonoBehaviour
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
    private StateMachine<MainSceneController, Scene_State> m_StateMachine = new StateMachine<MainSceneController, Scene_State>();
    public StateMachine<MainSceneController, Scene_State> StateMachine { get { return m_StateMachine; } }
    
    // �V�[�����o�p�^�C�����C������
    [SerializeField]
    private ScenePerformanceController m_ScenePerformanceController;
    public ScenePerformanceController ScenePerformanceController { get { return m_ScenePerformanceController; } }

    // ���͐���
    [SerializeField]
    private PlayerInput m_PlayerInput;

    // ���̓}�b�v�L�[
    private readonly string m_UIActionMapKey = "UI";
    private readonly string m_PlayerActionMapKey = "Player";

    // UI���̓A�N�V�����L�[
    private readonly string m_EnterActionKey = "Enter";
    private InputAction m_EnterAction { get { return m_PlayerInput.currentActionMap[m_EnterActionKey]; } }

    // �C���Q�[���L�����o�X
    [SerializeField]
    private Canvas m_IngameCanvas;

    // ���U���g�L�����o�X
    [SerializeField]
    private Canvas m_ResultCanvas;

    // ���U���g����
    [SerializeField]
    private ResultBehaviour m_ResultBehaviour;

    // ���U���g���j���[
    [SerializeField]
    private SelectableObjectGroup m_ResultManuGroup;

    // �v���C���[�^���N
    private PlayableTank m_PlayerTank;

    // �G�^���N
    private AutomationTank m_EnemyTank;

    // �C���Q�[�����̒Ǐ]�J����
    [SerializeField]
    private IngameCameras m_IngameCameras;

    // �J�����U��
    [SerializeField]
    private ShakeCamera m_ShakeCamera;


    // Start is called before the first frame update
    void Start()
    {
        // ��ԓo�^
        m_StateMachine.AddState(Scene_State.Scene_Begin, new SceneBegin(this));
        m_StateMachine.AddState(Scene_State.Battle_Start, new BattleStart(this));
        m_StateMachine.AddState(Scene_State.Battle, new Battle(this));
        m_StateMachine.AddState(Scene_State.Battle_Finish, new BattleFinish(this));
        m_StateMachine.AddState(Scene_State.Battle_Result, new BattleResult(this));
        m_StateMachine.AddState(Scene_State.Scene_End, new SceneEnd(this));
        
        // ���x���A�[�g�Ǎ����ɃV�[���J�n��Ԃ֑J�ڂ���
        m_StateMachine.ChangeState(Scene_State.Scene_Begin);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.Update();
    }

    /// <summary>
    /// �V�[���J�n�����o�̏I�����C�x���g
    /// </summary>
    public void FinishSceneBeginPerformance()
    {
        // �퓬�J�n��ԂɑJ�ڂ���
        m_StateMachine.ChangeState(Scene_State.Battle_Start);
    }

    /// <summary>
    /// �V�[���J�n�����o�̏I�����C�x���g
    /// </summary>
    public void FinishBattleEndPerformance()
    {
        // �퓬���ʏ�ԂɑJ�ڂ���
        m_StateMachine.ChangeState(Scene_State.Battle_Result);
    }

    /// <summary>
    /// �퓬�I��
    /// </summary>
    public void ToBattleEnd()
    {
        m_StateMachine.ChangeState(Scene_State.Battle_Finish);
    }

    /// <summary>
    /// �V�[��������
    /// </summary>
    private void SceneInitialize()
    {
        // ���͖���
        m_PlayerInput.Disable();

        // �e�L�����o�X��\��
        m_IngameCanvas.Disable();
        m_ResultCanvas.Disable();

        // �G�̒�~
        m_EnemyTank.Disable();
    }

    private void SceneFinalize()
    {
        // ���͖���
        m_PlayerInput.Disable();

        // �e�L�����o�X��\��
        m_IngameCanvas.Disable();
        m_ResultCanvas.Disable();

        // BGM��~
        BackgroundMusicPlayer.Instance.Stop();
    }


    /// <summary>
    /// �V�[�����I������
    /// </summary>
    public void ToSceneEnd()
    {
        StateMachine.ChangeState(Scene_State.Scene_End);
    }

    /// <summary>
    /// ���̓}�b�v�ؑ�
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
    /// �V�[���J�n���
    /// </summary>
    private class SceneBegin : State<MainSceneController>
    {
        public SceneBegin(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("SceneBegin");
            // ���x���A�[�g�쐬
            var property = IngameSetting.Instance.CurrentLevelArtProperty;
                LevelArtLoader.Instance.InstantiateFromProperty(property.LevelArtType, () =>
                {
                    owner.m_PlayerTank = LevelArtLoader.Instance.PlayableTank;
                    owner.m_EnemyTank = LevelArtLoader.Instance.EnemyTank;

                    // �v���C���[���^�[�Q�b�g�Ƃ��Đݒ�
                    owner.m_EnemyTank.SetTarget(owner.m_PlayerTank.Tank.transform);

                    // �C���Q�[���J�����ݒ�
                    owner.m_IngameCameras.SettingIngameCamera(owner.m_PlayerTank.GetComponentInChildren<TankMovement>().transform, owner.m_EnemyTank.GetComponentInChildren<TankMovement>().transform);

                    // �j�󎞃C�x���g�ݒ�
                    owner.m_PlayerTank.GetComponentInChildren<Damageable>().OnDie.AddListener((damager, damageable) => { owner.m_ShakeCamera.Shake(); });
                    owner.m_EnemyTank.GetComponentInChildren<Damageable>().OnDie.AddListener((damager, damageable) => { owner.m_ShakeCamera.Shake(); });

                    // �c�@�S����C�x���g�ݒ�
                    TankRemain playerRemain = owner.m_PlayerTank.GetComponentInChildren<TankRemain>();
                    TankRemain enemyRemain = owner.m_EnemyTank.GetComponentInChildren<TankRemain>();
                    playerRemain.OnLostAllRemain.AddListener(() => { owner.m_ResultBehaviour.SetResultType(ResultBehaviour.ResultType.PlayerLose); });
                    playerRemain.OnLostAllRemain.AddListener(owner.ToBattleEnd);

                    enemyRemain.OnLostAllRemain.AddListener(() => { owner.m_ResultBehaviour.SetResultType(ResultBehaviour.ResultType.PlayerWin); });
                    enemyRemain.OnLostAllRemain.AddListener(owner.ToBattleEnd);

                    // ���U���g�ݒ�
                    owner.m_ResultBehaviour.OnPrePlayerWin.AddListener(() => { owner.m_ResultBehaviour.SetResultCameraTraget(owner.m_PlayerTank.Tank.transform); });
                    owner.m_ResultBehaviour.OnPrePlayerLose.AddListener(() => { owner.m_ResultBehaviour.SetResultCameraTraget(owner.m_EnemyTank.Tank.transform); });
                    
                    // �V�[��������
                    owner.SceneInitialize();

                    // ���[�f�B���O�e�L�X�g��\��
                    //LoadingNavigator.Instance.StopLoading();

                    // �t�F�[�h�C���J�n
                    SceneNavigator.Instance.FadeIn(()=>{
                        // �V�[���J�n���o���Đ�����
                        // �I�����̃C�x���g�̓V�O�i���Őݒ肵�Ă���
                        // ���̏�ԁi�퓬�J�n��ԁj�ւ̑J�ڂ͏I�����C�x���g�ōs��(FinishSceneBeginPerformance)
                        owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Scene_Start);
                    });
                });
        }

        public override void Exit()
        {
            // ���͂�L���ɂ��A�v���C���[����p�̓��͂ɐ؂�ւ���
            owner.ChangeInputActionMap(owner.m_PlayerActionMapKey);
            owner.m_PlayerInput.Enable();
        }
    }

    /// <summary>
    /// �퓬�J�n���
    /// </summary>
    private class BattleStart : State<MainSceneController>
    {
        public BattleStart(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("BattleStart");
            // BGM�Đ�
            BackgroundMusicPlayer.Instance.Play();

            // �C���Q�[���L�����o�X�\��
            owner.m_IngameCanvas.Enable();

            // �G�̋N��
            owner.m_EnemyTank.Enable();
        }
    }
    /// <summary>
    /// �퓬�����
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
    /// �퓬�I�����
    /// </summary>
    private class BattleFinish : State<MainSceneController>
    {
        public BattleFinish(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("BattleFinish");
            // ���͖���
            owner.m_PlayerInput.Disable();

            // �C���Q�[���L�����o�X��\��
            owner.m_IngameCanvas.Disable();

            // ���U���g�O����
            // ���s���ʂ͎c�@���䂩�瑗��
            owner.m_ResultBehaviour.ResultSetUp();

            // �G��~
            owner.m_EnemyTank.Disable();

            // �퓬�I�����o���Đ�����
            // �I�����̃C�x���g�̓V�O�i���Őݒ肵�Ă���
            // ���̏�ԁi�퓬���ʏ�ԁj�ւ̑J�ڂ͏I�����C�x���g�ōs��(FinishBattleEndPerformance)
            owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Battle_End);
        }
    }
    /// <summary>
    /// �퓬���ʏ��
    /// </summary>
    private class BattleResult : State<MainSceneController>
    {
        public BattleResult(MainSceneController owner) : base(owner) { }


        public override void Enter()
        {
            Debug.Log("BattleResult");
            // ���U���g�L�����o�X�\��
            owner.m_ResultCanvas.Enable();

            // ���͂�L���ɂ��AUI����p�̓��͂ɐ؂�ւ���
            owner.ChangeInputActionMap(owner.m_UIActionMapKey);
            owner.m_PlayerInput.Enable();

            // ���U���g���o�Đ�
            owner.m_ResultBehaviour.ResultExecute();
        }

        public override void Execute()
        {
            UpdateResultManu();
        }

        /// <summary>
        /// ���U���g���j���[�X�V
        /// </summary>
        private void UpdateResultManu()
        {
            if (owner.m_EnterAction == null)
                return;

            // ����
            if (owner.m_EnterAction.WasPressedThisFrame())
            {
                owner.m_ResultManuGroup.DecideElement();
            }
        }
    }
    /// <summary>
    /// �V�[���I�����
    /// </summary>
    private class SceneEnd : State<MainSceneController>
    {
        public SceneEnd(MainSceneController owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("SceneEnd");

            // ���U���g�L�����o�X��\��
            owner.SceneFinalize();

            // �^�C�g���o�b�N
            SceneNavigator.Instance.Transition(CommonDefineData.SceneNameTitle);
        }
    }
}
