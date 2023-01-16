using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

/// <summary>
/// ����������
/// </summary>
public class AutomationTank : Agent
{
    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }
    private PlayerInput m_PlayerInput;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();
    }

    private void Update()
    {
    }

    private void TankInitialize()
    {

    }

    // �G�s�\�[�h�J�n��
    public override void OnEpisodeBegin()
    {
    }

    //�@�ώ@�̎��W
    public override void CollectObservations(VectorSensor sensor)
    {
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
        AddReward(-1f / MaxStep);
        //�@�ړ��f�[�^�̍쐬

        m_TankMovement.Move(continuousAction[0]);
        m_TankMovement.Turn(continuousAction[1]);

        if(discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //�@�Ȃ�炩�̉e����Floor����]�����ʒu��-5��艺�ɂȂ�����-0.1�̕�V��^����
        if (transform.localPosition.y < -5f)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    /// <summary>
    /// �G�ɒe�𓖂Ă��Ƃ�
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.1f);
    }

    /// <summary>
    /// �����ɒe������������
    /// </summary>
    public void BulletHitToMyself()
    {
        AddReward(-0.1f);
    }

    /// <summary>
    /// �G��j�󂵂��Ƃ�
    /// </summary>
    public void DieToTarget()
    {
        AddReward(1.0f);
        EndEpisode();
    }

    /// <summary>
    /// �j�󂳂ꂽ�Ƃ�
    /// </summary>
    public void DieToMyself()
    {
        AddReward(-0.5f);
        EndEpisode();
    }
    

    //�@�����ő���
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var ActionsOut = actionsOut.ContinuousActions;
        ActionsOut.Clear();

        var m = m_PlayerInput.actions["Move"].ReadValue<Vector2>();

        ActionsOut[1] = m.x;
        ActionsOut[0] = m.y;

        var f = Keyboard.current.spaceKey.wasPressedThisFrame;

        var discreateOut = actionsOut.DiscreteActions;
        discreateOut[0] = f ? 1 : 0;
    }
}
