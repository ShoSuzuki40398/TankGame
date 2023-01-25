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
    private Transform m_Target;
    [SerializeField]
    private Rigidbody m_TargetRigidBody;

    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    [SerializeField]
    private NavMeshAgent m_NavMeshAgent;

    // 戦車本体
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }
    private PlayerInput m_PlayerInput;

    // 弾が当たる距離まで近づいて、適正な距離を保つようにしたい 
    // 接近距離
    [SerializeField]
    private float m_CloseDistance = 4.0f;
    // 隔絶距離
    [SerializeField]
    private float m_FarDistance = 6.0f;

    // 内積による視界範囲
    [SerializeField,Range(-1,1)]
    private float m_DotRange = 0.9f;

    [Range(0, 15)]
    [SerializeField]
    private float respawnRange = 0;

    [SerializeField]
    private LayerMask LayerMask;

    Vector3 moveVelocity = Vector3.zero;

    [SerializeField]
    private TargetManager targetManager;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // OnActionReceivedにてNavMeshのMove関数で戦車を移動すると、砲弾があらぬ方向へ飛んでいく不具合がある
        // そのためMove関数だけUpdateでコールする。
        // OnActionReceivedではmoveVelocityだけ設定し、Update内でMavMeshによる移動をすることで上記不具合を回避します。
        m_NavMeshAgent.Move(moveVelocity * Time.deltaTime * m_TankMovement.Speed);
    }

    private void AgentInitialize()
    {
        Vector3 initPlayerPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
        Vector3 initEnemyPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));

        
        // 10回試す
        for (int n = 0; n < 100; n++)
        {
            Vector3 halfExtents = new Vector3(0.5f, 0.05f, 0.5f);
            // 重ならないとき
            if (!Physics.CheckBox(initEnemyPos, halfExtents, Quaternion.identity,LayerMask))
            {
                m_Tank.transform.localPosition = initEnemyPos;
                break;
            }
            else
            {
                initEnemyPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
            }
        }

        // 10回試す
        for (int n = 0; n < 100; n++)
        {
            Vector3 halfExtents = new Vector3(3.5f, 0.05f, 3.5f);
            // 重ならないとき
            if (!Physics.CheckBox(initPlayerPos, halfExtents, Quaternion.identity, LayerMask))
            {
                m_Target.transform.localPosition = initPlayerPos;
                break;
            }
            else
            {
                var distance = Vector3.Distance(m_Target.position, m_Tank.transform.position);

                initPlayerPos = new Vector3(Random.Range(-respawnRange, respawnRange), 0.25f, Random.Range(-respawnRange, respawnRange));
            }
        }

        // ランダムな方に向く
        var angle = Random.Range(0, 360);
        m_Tank.transform.Rotate(0, angle, 0);
        angle = Random.Range(0, 360);
        m_Target.Rotate(0, angle, 0);

        // プレイヤーのリジッドボディ初期化
        m_TargetRigidBody.velocity = Vector3.zero;
        m_TargetRigidBody.angularVelocity = Vector3.zero;

        // ターゲット初期化
        targetManager.TargetInitialize(this);
    }

    // エピソード開始時
    public override void OnEpisodeBegin()
    {
        AgentInitialize();
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

        // 自分の正面方向を観察に追加する
        sensor.AddObservation(m_Tank.transform.forward);

        var distance = Vector3.Distance(m_Target.position, m_Tank.transform.position);
        sensor.AddObservation(distance);
    }


    //　アクションの受け取りと報酬を与える
    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousAction = actions.ContinuousActions;
        var discreateAction = actions.DiscreteActions;

        //　MaxStepを分母にして1ステップ毎にマイナス報酬を与える
        var distance = Vector3.Distance(m_Target.position, m_Tank.transform.position);
        
        // 時間経過で罰を与える
        AddReward(-1f / MaxStep);

        // 適正な距離に要れば報酬を与える
        //if (m_CloseDistance <= distance && distance <= m_FarDistance)
        //{
        //    AddReward(0.1f / MaxStep);
        //}
        //else
        //{
        //    AddReward(-0.1f / MaxStep);

        //}

        // 近づきすぎると厳罰
        if (m_CloseDistance * 0.5f >= distance)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        // プレイヤーの方を向いていると報酬を与える
        var vec = m_Target.transform.position - m_Tank.transform.position;
        var dot = Vector3.Dot(vec.normalized, m_Tank.transform.forward);
        
        //if(dot >= m_DotRange)
        //{
        //    AddReward(0.1f / MaxStep);
        //}
        //else
        //{
        //    AddReward(-0.1f / MaxStep);
        //}

        // 適正射撃
        if (discreateAction[0] == 1 && m_CloseDistance <= distance && distance <= m_FarDistance && dot >= m_DotRange)
        {
            AddReward(1.0f/MaxStep);
        }
        //else if (discreateAction[0] == 1 && m_CloseDistance >= distance && distance >= m_FarDistance && dot <= m_DotRange)
        //{
        //    AddReward(-1.0f / MaxStep);
        //}

        // 後退ペナルティ
        if(continuousAction[0] < 0)
        {
            AddReward(-1.0f / MaxStep);
        }
        //　移動データの作成
        moveVelocity = (m_Tank.transform.forward * continuousAction[0]).normalized;
        m_TankMovement.Turn(continuousAction[1]);

        if (discreateAction[0] == 1)
        {
            m_TankShooting.Fire();
        }

        //　なんらかの影響でFloorから転落し位置が-5より下になったら元に戻す
        if (m_Tank.transform.localPosition.y < -5f)
        {
            AgentInitialize();
        }

        if (m_Target.localPosition.y < -5f)
        {
            AgentInitialize();
        }

        // Target 全消滅
        //if (targetManager.IsAllTargetInPool())
        //{
        //    EndEpisode();
        //}
    }

    /// <summary>
    /// プレイヤーに弾を当てたとき
    /// </summary>
    public void BulletHitToTarget()
    {
        AddReward(0.2f);
    }

    /// <summary>
    /// 自分に弾が当たった時
    /// </summary>
    public void BulletHitToMyself()
    {
        AddReward(-0.1f);
    }

    /// <summary>
    /// プレイヤーが破壊された時
    /// </summary>
    public void DieToTarget()
    {
        AddReward(0.3f);
        EndEpisode();
    }

    /// <summary>
    /// 自分が破壊された時
    /// </summary>
    public void DieToMyself()
    {
        AddReward(-1.0f);
        EndEpisode();
    }

    /// <summary>
    /// ターゲットを破壊した時
    /// </summary>
    /// <param name="reward"></param>
    public void HitTarget(float reward)
    {
        AddReward(reward);
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
}
