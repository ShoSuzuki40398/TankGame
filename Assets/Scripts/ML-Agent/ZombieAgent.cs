
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ZombieAgent : Agent
{
    private CharacterController characterController;
    private Animator animator;
    //　追いかけるターゲット
    [SerializeField]
    private Transform target;
    // 速さ
    [SerializeField]
    private float walkSpeed = 2f;
    //　速度
    private Vector3 velocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    //　データの初期化メソッド
    public void Reset()
    {
        velocity = Vector3.zero;
        characterController.enabled = false;
        transform.localPosition = new Vector3(-2f, 1.5f, 0f);
        characterController.enabled = true;

        var targetCharacterController = target.GetComponent<CharacterController>();
        targetCharacterController.enabled = false;
        target.localPosition = new Vector3(Random.Range(-13f, 13f), 1.5f, Random.Range(-13f, 13f));
        targetCharacterController.enabled = true;
    }


    public override void OnEpisodeBegin()
    {
        Reset();
    }


    //　観察の収集
    public override void CollectObservations(VectorSensor sensor)
    {
        //　ゲームの舞台サイズに合わせて正規化し観察に追加する
        sensor.AddObservation(target.localPosition / 15f);
        sensor.AddObservation(transform.localPosition / 15f);
        //　主人公の方向を正規化し観察に追加する
        var direction = (target.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(direction);
    }

    //　アクションの受け取りと報酬を与える
    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction  = actions.ContinuousActions;

        //　MaxStepを分母にして1ステップ毎にマイナス報酬を与える
        AddReward(-1f / MaxStep);
        //　移動データの作成
        var input = new Vector3(vectorAction[0], 0f, vectorAction[1]);
        //　キャラクターが接地している時だけ移動
        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;

            if (input.magnitude > 0f)
            {
                //　キャラクターの向きは徐々に変える
                transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(input.normalized, Vector3.up), 6f * Time.deltaTime);
                velocity = transform.forward * walkSpeed;
                animator.SetFloat("Speed", input.magnitude);
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        //　主人公と自身（敵）の距離が1.8mより短ければ1の報酬を与える
        if (Vector3.Distance(target.localPosition, transform.localPosition) < 1.8f)
        {
            AddReward(1f);
            EndEpisode();
        }
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
        var ActionsOut = actionsOut.ContinuousActions;
        ActionsOut.Clear();

        ActionsOut[0] = Input.GetAxis("Horizontal");
        ActionsOut[1] = Input.GetAxis("Vertical");
    }

}
