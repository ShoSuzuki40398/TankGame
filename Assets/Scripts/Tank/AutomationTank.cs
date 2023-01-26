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

    // TODO:�����[�X���ɍ폜
    [SerializeField]
    private Rigidbody m_TargetRigidBody;

    // TODO:�����[�X���ɍ폜
    [Range(0, 15)]
    [SerializeField]
    private float respawnRange = 0;

    // TODO:�����[�X���ɍ폜
    [SerializeField]
    private LayerMask LayerMask;

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

    // TODO:�����[�X���ɍ폜
    private void AgentInitialize()
    {
        Vector3 initPlayerPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
        Vector3 initEnemyPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));


        // 10�񎎂�
        for (int n = 0; n < 10; n++)
        {
            Vector3 halfExtents = new Vector3(0.5f, 0.05f, 0.5f);
            // �d�Ȃ�Ȃ��Ƃ�
            if (!Physics.CheckBox(initEnemyPos, halfExtents, Quaternion.identity, LayerMask))
            {
                m_Tank.transform.localPosition = initEnemyPos;
                break;
            }
            else
            {
                initEnemyPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
            }
        }

        // 10�񎎂�
        for (int n = 0; n < 10; n++)
        {
            Vector3 halfExtents = new Vector3(3.5f, 0.05f, 3.5f);
            // �d�Ȃ�Ȃ��Ƃ�
            if (!Physics.CheckBox(initPlayerPos, halfExtents, Quaternion.identity, LayerMask))
            {
                m_Player.transform.localPosition = initPlayerPos;
                break;
            }
            else
            {
                var distance = Vector3.Distance(m_Player.position, m_Tank.transform.position);

                initPlayerPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
            }
        }

        // �����_���ȕ��Ɍ���
        var angle = Random.Range(0, 360);
        m_Tank.transform.Rotate(0, angle, 0);
        angle = Random.Range(0, 360);
        m_Player.Rotate(0, angle, 0);

        // �v���C���[�̃��W�b�h�{�f�B������
        m_TargetRigidBody.velocity = Vector3.zero;
        m_TargetRigidBody.angularVelocity = Vector3.zero;
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

        // �K���ȋ����ɗv��Ε�V��^����
        if (m_CloseDistance <= distance && distance <= m_FarDistance)
        {
            AddReward(0.1f / MaxStep);
        }
        //else
        //{
        //    AddReward(-1f / MaxStep);
        //}

        // �߂Â�������ƌ���
        if (2 >= distance)
        {
            SetReward(-1.0f);
            // TODO:�����[�X���ɍ폜
            EndEpisode();
        }

        // �v���C���[�̕��������Ă���ƕ�V��^����
        var vec = m_Player.transform.position - m_Tank.transform.position;
        var dot = Vector3.Dot(vec.normalized, m_Tank.transform.forward);
        if (dot >= m_DotRange)
        {
            AddReward(0.1f / MaxStep);
        }
        else
        {
            AddReward(-0.1f / MaxStep);
        }

        // �K���ˌ�
        if (discreateAction[0] == 1 && m_CloseDistance <= distance && distance <= m_FarDistance && dot >= m_DotRange)
        {
            AddReward(1f / MaxStep);
        }
        else if (discreateAction[0] == 1 && m_CloseDistance >= distance && distance >= m_FarDistance && dot <= m_DotRange)
        {
            AddReward(-1f / MaxStep);
        }

        // ��ރy�i���e�B(�\�Ȍ���O�i�ŉ������Ăق���)
        //if (continuousAction[0] < 0)
        //{
        //    AddReward(-1f / MaxStep);
        //}

        //�@�ړ��f�[�^�̍쐬
        moveVelocity = (m_Tank.transform.forward * continuousAction[0]).normalized;
        m_TankMovement.Turn(continuousAction[1]);

        // �ˌ��f�[�^�̍쐬
        if (discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //�@�Ȃ�炩�̉e����Floor����]�����ʒu��-5��艺�ɂȂ����猳�ɖ߂�
        if (m_Tank.transform.localPosition.y < -5f)
        {
            AgentInitialize();
        }

        if (m_Player.localPosition.y < -5f)
        {
            AgentInitialize();
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
    /// �v���C���[�ɒe�𓖂Ă��Ƃ�
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.5f);
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �����ɒe������������
    /// </summary>
    public void BulletHitToMyself()
    {
        AddReward(-0.5f);
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �v���C���[���j�󂳂ꂽ��
    /// </summary>
    public void DieToTarget()
    {
        //AddReward(1.0f);
        EndEpisode();
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �������j�󂳂ꂽ��
    /// </summary>
    public void DieToMyself()
    {
        //AddReward(-1.0f);
        EndEpisode();
    }

    // TODO:�����[�X���ɍ폜
    /// <summary>
    /// �v���C���[�ʒu�ݒ�
    /// </summary>
    public void SetTarget(Transform player)
    {
        m_Player = player;
    }
}
