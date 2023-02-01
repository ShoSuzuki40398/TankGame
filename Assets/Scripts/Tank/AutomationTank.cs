using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;
using UnityEngine.AI;

/// <summary>
/// 自動操作戦車
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

    // 戦車本体
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    // 移動量
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
        // OnActionReceivedにてNavMeshのMove関数で戦車を移動すると、砲弾があらぬ方向へ飛んでいく不具合がある。
        // そのためMove関数だけUpdateでコールする。
        // OnActionReceivedではmoveVelocityだけ設定し、Update内でMavMeshによる移動をすることで上記不具合を回避します。
        m_NavMeshAgent.Move(moveVelocity * Time.deltaTime * m_TankMovement.Speed);
    }

    // TODO:リリース時に削除
    private void AgentInitialize()
    {
    }

    // エピソード開始時
    public override void OnEpisodeBegin()
    {
        // TODO:リリース時に削除
        AgentInitialize();
    }

    //　観察の収集
    public override void CollectObservations(VectorSensor sensor)
    {
        if (m_Target == null)
            return;

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
        // 時間経過で罰を与える
        if (tankEnvController != null)
        {
            AddReward(-1f / tankEnvController.MaxEnvironmentSteps);
        }

        //　移動データの作成
        moveVelocity = (m_Tank.transform.forward * Mathf.Abs(continuousAction[0])).normalized;
        m_TankMovement.Turn(continuousAction[1]);

        // 射撃データの作成
        if (discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //　なんらかの影響でFloorから転落し位置が-5より下になったら元に戻す
        if (m_Tank.transform.localPosition.y < -5f)
        {
            TankEnvController envController = GetComponentInParent<TankEnvController>();
            envController.ResetScene();
        }
    }

    //　自分で操作
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

    // TODO:リリース時に削除
    /// <summary>
    /// 相手に弾を当てたとき
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.5f);
    }

    // TODO:リリース時に削除
    /// <summary>
    /// 自分が破壊された時
    /// </summary>
    public void DestroyMyself()
    {
        if (tankEnvController == null)
            return;

        var layer = Tank.layer;
        tankEnvController.DestroyOpponent(layer);
    }

    // TODO:リリース時に削除
    /// <summary>
    /// プレイヤー位置設定
    /// </summary>
    public void SetTarget(Transform player)
    {
        m_Target = player;
    }
}
