using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Target : MonoBehaviour
{
    [HideInInspector]
    public TargetObject m_TargetObject;

    [SerializeField]
    private float m_RotateSpeed = 5;

    public AutomationTank automationTank;

    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        transform.Rotate(new Vector3(0, m_RotateSpeed * Time.deltaTime, 0));
    }

    /// <summary>
    /// èâä˙à íuê›íË
    /// </summary>
    public void Initialize(Vector3 pos, Quaternion rot,AutomationTank automationTank)
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
        this.automationTank = automationTank;
    }

    public void Hit(Damager damager, Damageable damageable)
    {
        var layer = LayerMask.LayerToName(damager.gameObject.layer);

        if (layer == "EnemyShell")
        {
            automationTank.HitTarget(0.1f);
        }
        else if(layer == "PlayerShell")
        {
            automationTank.HitTarget(-0.1f);
        }
        m_TargetObject.ReturnToPool();
    }
}
