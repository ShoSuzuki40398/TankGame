using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

/// <summary>
/// 自動操作戦車
/// </summary>
public class AutomationTank : Agent
{
    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    // 戦車本体
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

    // エピソード開始時
    public override void OnEpisodeBegin()
    {
    }

    //　観察の収集
    public override void CollectObservations(VectorSensor sensor)
    {
        //　自分とプレイヤーの位置を観察に追加する
        sensor.AddObservation(m_Target.localPosition);
        sensor.AddObservation(m_Tank.transform.localPosition);
        //　プレイヤーの方向を正規化し観察に追加する
        var direction = (m_Target.localPosition - m_Tank.transform.localPosition).normalized;
        sensor.AddObservation(direction);
    }


    //　アクションの受け取りと報酬を与える
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousAction = actions.ContinuousActions;
        var discreateAction = actions.DiscreteActions;

        //　MaxStepを分母にして1ステップ毎にマイナス報酬を与える
        AddReward(-1f / MaxStep);
        //　移動データの作成

        m_TankMovement.Move(continuousAction[0]);
        m_TankMovement.Turn(continuousAction[1]);

        if(discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //　なんらかの影響でFloorから転落し位置が-5より下になったら-0.1の報酬を与える
        if (transform.localPosition.y < -5f)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    /// <summary>
    /// 敵に弾を当てたとき
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.1f);
    }

    /// <summary>
    /// 自分に弾が当たった時
    /// </summary>
    public void BulletHitToMyself()
    {
        AddReward(-0.1f);
    }

    /// <summary>
    /// 敵を破壊したとき
    /// </summary>
    public void DieToTarget()
    {
        AddReward(1.0f);
        EndEpisode();
    }

    /// <summary>
    /// 破壊されたとき
    /// </summary>
    public void DieToMyself()
    {
        AddReward(-0.5f);
        EndEpisode();
    }
    

    //　自分で操作
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
