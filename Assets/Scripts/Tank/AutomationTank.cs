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
    private Transform m_Player;

    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    [SerializeField]
    private NavMeshAgent m_NavMeshAgent;
    
    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }
    private PlayerInput m_PlayerInput;

    // �e�������鋗���܂ŋ߂Â��āA�K���ȋ�����ۂ悤�ɂ����� 
    // �ڋߋ���
    [SerializeField]
    private float m_CloseDistance = 4.0f;
    // �u�⋗��
    [SerializeField]
    private float m_FarDistance = 6.0f;

    // ���ςɂ�鎋�E�͈�
    [SerializeField,Range(-1,1)]
    private float m_DotRange = 0.9f;

    // �ړ���
    Vector3 moveVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
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

    // �G�s�\�[�h�J�n��
    public override void OnEpisodeBegin()
    {
    }

    //�@�ώ@�̎��W
    public override void CollectObservations(VectorSensor sensor)
    {
        if (m_Player == null)
            return;


        //�@�����ƃv���C���[�̈ʒu���ώ@�ɒǉ�����
        sensor.AddObservation(m_Player.localPosition);
        sensor.AddObservation(m_Tank.transform.localPosition);
        //�@�v���C���[�̕����𐳋K�����ώ@�ɒǉ�����
        var direction = (m_Player.localPosition - m_Tank.transform.localPosition).normalized;
        sensor.AddObservation(direction);

        // �����̐��ʕ������ώ@�ɒǉ�����
        sensor.AddObservation(m_Tank.transform.forward);

        var distance = Vector3.Distance(m_Player.position, m_Tank.transform.position);
        sensor.AddObservation(distance);
    }


    //�@�A�N�V�����̎󂯎��ƕ�V��^����
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousAction = actions.ContinuousActions;
        var discreateAction = actions.DiscreteActions;

        //�@MaxStep�𕪕�ɂ���1�X�e�b�v���Ƀ}�C�i�X��V��^����
        var distance = Vector3.Distance(m_Player.position, m_Tank.transform.position);

        // ���Ԍo�߂Ŕ���^����
        AddReward(-1f / MaxStep);

        // �߂Â�������ƌ���
        if (m_CloseDistance * 0.5f >= distance)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        // �v���C���[�̕��������Ă���ƕ�V��^����
        var vec = m_Player.transform.position - m_Tank.transform.position;
        var dot = Vector3.Dot(vec.normalized, m_Tank.transform.forward);

        // �K���ˌ�
        if (discreateAction[0] == 1 && m_CloseDistance <= distance && distance <= m_FarDistance && dot >= m_DotRange)
        {
            AddReward(1.0f / MaxStep);
        }

        // ��ރy�i���e�B
        if (continuousAction[0] < 0)
        {
            AddReward(-1.0f / MaxStep);
        }

        //�@�ړ��f�[�^�̍쐬
        moveVelocity = (m_Tank.transform.forward * continuousAction[0]).normalized;
        m_TankMovement.Turn(continuousAction[1]);

        if (discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
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

    /// <summary>
    /// �v���C���[�ʒu�ݒ�
    /// </summary>
    public void SetTarget(Transform player)
    {
        m_Player = player;
    }

    /// <summary>
    /// �s���J�n
    /// </summary>
    public void AwakeMovement()
    {
        //m_DecisionRequester.Enable();
    }

    /// <summary>
    /// �s����~
    /// </summary>
    public void StopMovement()
    {
        //m_DecisionRequester.Disable();
    }
}
