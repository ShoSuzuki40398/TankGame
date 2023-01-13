using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

/// <summary>
/// 自動操作戦車
/// </summary>
public class AutomationTank : Agent
{
    [SerializeField]
    private Transform m_Target;

    // 戦車本体
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

    // エピソード開始時
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    //　観察の収集
    public override void CollectObservations(VectorSensor sensor)
    {
        ////　ゲームの舞台サイズに合わせて正規化し観察に追加する
        //sensor.AddObservation(target.localPosition);
        //sensor.AddObservation(transform.localPosition);
        ////　主人公の方向を正規化し観察に追加する
        //var direction = (target.localPosition - transform.localPosition).normalized;
        //sensor.AddObservation(direction);
    }


    //　アクションの受け取りと報酬を与える
    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.ContinuousActions;

        //　MaxStepを分母にして1ステップ毎にマイナス報酬を与える
        AddReward(-1f / MaxStep);
        //　移動データの作成
        //var input = new Vector3(vectorAction[0], 0f, vectorAction[1]);

        // 移動処理←ここから　Playerの移動処理を真似ればいい

        //　なんらかの影響でFloorから転落し位置が-5より下になったら-0.1の報酬を与える
        if (transform.localPosition.y < -5f)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    //　自分で操作
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //var ActionsOut = actionsOut.ContinuousActions;
        //ActionsOut.Clear();

        //ActionsOut[0] = Input.GetAxis("Horizontal");
        //ActionsOut[1] = Input.GetAxis("Vertical");
    }
}
