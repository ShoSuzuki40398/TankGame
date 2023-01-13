using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// <summary>
/// ����������
/// </summary>
public class AutomationTank : Agent
{
    [SerializeField]
    private Transform m_Target;

    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void TankInitialize()
    {

    }

    // �G�s�\�[�h�J�n��
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    //�@�ώ@�̎��W
    public override void CollectObservations(VectorSensor sensor)
    {
        ////�@�Q�[���̕���T�C�Y�ɍ��킹�Đ��K�����ώ@�ɒǉ�����
        //sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(transform.localPosition);
        ////�@��l���̕����𐳋K�����ώ@�ɒǉ�����
        //var direction = (target.localPosition - transform.localPosition).normalized;
        //sensor.AddObservation(direction);
    }


    //�@�A�N�V�����̎󂯎��ƕ�V��^����
    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.ContinuousActions;

        //�@MaxStep�𕪕�ɂ���1�X�e�b�v���Ƀ}�C�i�X��V��^����
        AddReward(-1f / MaxStep);
        //�@�ړ��f�[�^�̍쐬
        //var input = new Vector3(vectorAction[0], 0f, vectorAction[1]);

        // �ړ���������������@Player�̈ړ�������^����΂���

        //�@�Ȃ�炩�̉e����Floor����]�����ʒu��-5��艺�ɂȂ�����-0.1�̕�V��^����
        if (transform.localPosition.y < -5f)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    //�@�����ő���
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //var ActionsOut = actionsOut.ContinuousActions;
        //ActionsOut.Clear();

        //ActionsOut[0] = Input.GetAxis("Horizontal");
        //ActionsOut[1] = Input.GetAxis("Vertical");
    }
}
