using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAgent : MonoBehaviour
{
    AutomationTank AutomationTank;

    private void Awake()
    {
        AutomationTank = GetComponentInParent<AutomationTank>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AutomationTank.AddReward(-1.0f);
            AutomationTank.EndEpisode();
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            AutomationTank.AddReward(-1.0f);
            AutomationTank.EndEpisode();
        }
    }
}
