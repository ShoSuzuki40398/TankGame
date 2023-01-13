using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ShootingAgentScript : Agent
{
    Rigidbody agentRigidbody;

    public Transform target;
    public GameObject bullet;

    public bool isDamage;

    // Start is called before the first frame update
    void Start()
    {
        agentRigidbody = GetComponent<Rigidbody>();
    }

    int count = 0;

    public void Hit()
    {
        isDamage = true;
        count++;
        Debug.Log("Hit : " + count);
    }

    public bool ReloadCheck()
    {
        if (GameObject.FindGameObjectWithTag("Bullet"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            // 床の下に落ちた場合エージェントを初期位置に戻す
            this.agentRigidbody.angularVelocity = Vector3.zero;
            this.agentRigidbody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // ターゲットを新たなランダムな場所に移動させる
        target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);
        isDamage = false;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector Observations > Space SizeをCollectObservations()で書き込むfloatの数と同じに設定する必要があります。
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.forward);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        SetReward(-0.001f);

        if (isDamage)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }


        MoveAgent(actions.DiscreteActions);
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];
        var shootAction = act[3];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = new Vector3(0f, 0f, 0.1f);
                break;
            case 2:
                dirToGo = new Vector3(0f, 0f, -0.1f);
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = new Vector3(0.1f, 0f, 0f);
                break;
            case 2:
                dirToGo = new Vector3(-0.1f, 0f, 0f);
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        if (shootAction == 1)
        {
            GameObject bullets = Instantiate(bullet) as GameObject;
            Vector3 force;
            force = this.gameObject.transform.forward * 300.0f;
            bullets.GetComponent<Rigidbody>().AddForce(force);
            bullets.transform.position = this.transform.position;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        transform.Translate(dirToGo);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
        discreteActionsOut[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
