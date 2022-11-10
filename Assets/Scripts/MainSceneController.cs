using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;

/// <summary>
/// �V�[����Ԃ̑J�ڐ�����s��
/// </summary>
public class MainSceneController : SingletonMonoBehaviour<MainSceneController>
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
    private StateMachine<MainSceneController,Scene_State> m_StateMachine = new StateMachine<MainSceneController, Scene_State>();
    public StateMachine<MainSceneController, Scene_State> StateMachine { get { return m_StateMachine; }}

    // BGM�v���C���[
    [SerializeField]
    private BackgroundMusicPlayer m_BgmPlayer;
    public BackgroundMusicPlayer BgmPlayer { get { return m_BgmPlayer; } }

    // �V�[�����o�p�^�C�����C������
    [SerializeField]
    private ScenePerformanceController m_ScenePerformanceController;
    public ScenePerformanceController ScenePerformanceController { get { return m_ScenePerformanceController; } }

    // ���͐���
    [SerializeField]
    private PlayerInput m_PlayerInput;

    // �C���Q�[���L�����o�X
    [SerializeField]
    private Canvas m_IngameCanvas;

    // ���U���g�L�����o�X
    [SerializeField]
    private Canvas m_ResultCanvas;
    
    // ���U���g����
    [SerializeField]
    private ResultBehaviour m_ResultBehaviour;

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
    /// <param name="aDirector"></param>
    public void FinishSceneBeginPerformance()
    {
        // �퓬�J�n��ԂɑJ�ڂ���
        m_StateMachine.ChangeState(Scene_State.Battle_Start);
    }

    /// <summary>
    /// �V�[���J�n�����o�̏I�����C�x���g
    /// </summary>
    /// <param name="aDirector"></param>
    public void FinishBattleEndPerformance()
    {
        // �퓬���ʏ�ԂɑJ�ڂ���
        m_StateMachine.ChangeState(Scene_State.Battle_Result);
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

            // �V�[��������
            owner.SceneInitialize();

            // �V�[���J�n���o���Đ�����
            // �I�����̃C�x���g�̓V�O�i���Őݒ肵�Ă���
            // ���̏�ԁi�퓬�J�n��ԁj�ւ̑J�ڂ͏I�����C�x���g�ōs��(FinishSceneBeginPerformance)
            owner.ScenePerformanceController.PlayOneShot(ScenePerformanceController.PerformanceType.Scene_Start);

            owner.Delay(7.0f, () => owner.StateMachine.ChangeState(Scene_State.Battle_Finish));
        }

        public override void Exit()
        {
            // ���͗L��
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
            owner.BgmPlayer.Play();

            // �C���Q�[���L�����o�X�\��
            owner.m_IngameCanvas.Enable();
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
            owner.m_ResultBehaviour.ResultSetUp();

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

            // ���U���g���o�Đ�
            owner.m_ResultBehaviour.ResultExecute();
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
            owner.m_ResultCanvas.Enable();
        }
    }
}
