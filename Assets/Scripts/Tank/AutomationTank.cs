using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;
using UnityEngine.AI;

/// <summary>
/// ����������
/// </summary>
public class AutomationTank : Agent
{
    [SerializeField]
    private TankEnvController tankEnvController;
    [SerializeField]
    private Transform m_Target;

    [HideInInspector]
    public TankMovement m_TankMovement;
    private TankShooting m_TankShooting;
    private NavMeshAgent m_NavMeshAgent;
    private PlayerInput m_PlayerInput;

    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    // �ړ���
    private Vector3 moveVelocity = Vector3.zero;

    private void Awake()
    {
        m_TankMovement = GetComponentInChildren<TankMovement>();
        m_TankShooting = GetComponentInChildren<TankShooting>();
        m_NavMeshAgent = GetComponentInChildren<NavMeshAgent>();

        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // OnActionReceived�ɂ�NavMesh��Move�֐��Ő�Ԃ��ړ�����ƁA�C�e������ʕ����֔��ł����s�������B
        // ���̂���Move�֐�����Update�ŃR�[������B
        // OnActionReceived�ł�moveVelocity�����ݒ肵�AUpdate����MavMesh�ɂ��ړ������邱�Ƃŏ�L�s���������܂��B
        m_NavMeshAgent.Move(moveVelocity * Time.deltaTime * m_TankMovement.Speed);
    }

    // TODO:�����[�X���ɍ폜
    private void AgentInitialize()
    {
    }

    // �G�s�\�[�h�J�n��
    public override void OnEpisodeBegin()
    {
        // TODO:�����[�X���ɍ폜
        AgentInitialize();
    }

    //�@�ώ@�̎��W
    public override void CollectObservations(VectorSensor sensor)
    {
        if (m_Target == null)
            return;

        //�@�����ƃv���C���[�̈ʒu���ώ@�ɒǉ�����
        sensor.AddObservation(m_Target.localPosition);
        sensor.AddObservation(m_Tank.transform.localPosition);
        //�@�v���C���[�̕����𐳋K�����ώ@�ɒǉ�����
        var direction = (m_Target.localPosition - m_Tank.transform.localPosition).normalized;
        sensor.AddObservation(direction);
    }


    //�@�A�N�V�����̎󂯎��ƕ�V��^����
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousAction = actions.ContinuousActions;
        var discreateAction = actions.DiscreteActions;

        //�@MaxStep�𕪕�ɂ���1�X�e�b�v���Ƀ}�C�i�X��V��^����
        // ���Ԍo�߂Ŕ���^����
        if (tankEnvController != null)
        {
            AddReward(-1f / tankEnvController.MaxEnvironmentSteps);
        }

        //�@�ړ��f�[�^�̍쐬
        moveVelocity = (m_Tank.transform.forward * Mathf.Abs(continuousAction[0])).normalized;
        m_TankMovement.Turn(continuousAction[1]);

        // �ˌ��f�[�^�̍쐬
        if (discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //�@�Ȃ�炩�̉e����Floor����]�����ʒu��-5��艺�ɂȂ����猳�ɖ߂�
        if (m_Tank.transform.localPosition.y < -5f)
        {
            TankEnvController envController = GetComponentInParent<TankEnvController>();
            envController.ResetScene();
        }
    }

    //�@�����ő���
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var ActionsOut = actionsOut.ContinuousActions;
        ActionsOut.Clear();

        var m = m_PlayerInput.actions["Move"].ReadValue<Vector2>();

        ActionsOut[0] = m.y;
        ActionsOut[1] = m.x;

        var f = m_PlayerInput.currentActionMap["Fire"].WasPressedThisFrame();

        var discreateOut = actionsOut.DiscreteActions;
        discreateOut[0] = f ? 1 : 0;
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// ����ɒe�𓖂Ă��Ƃ�
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.5f);
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �������j�󂳂ꂽ��
    /// </summary>
    public void DestroyMyself()
    {
        if (tankEnvController == null)
            return;

        var layer = Tank.layer;
        tankEnvController.DestroyOpponent(layer);
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �v���C���[�ʒu�ݒ�
    /// </summary>
    public void SetTarget(Transform player)
    {
        m_Target = player;
    }
}
