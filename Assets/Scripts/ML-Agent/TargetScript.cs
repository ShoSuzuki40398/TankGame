using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    GameObject agent;
    ShootingAgentScript script;

    void Start()
    {
        agent = GameObject.Find("ShootingAgent");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            script = agent.GetComponent<ShootingAgentScript>();
            script.Hit();
        }
    }
}