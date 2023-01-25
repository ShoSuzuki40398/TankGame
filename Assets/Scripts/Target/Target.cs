using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットオブジェクト制御
/// </summary>
public class Target : MonoBehaviour
{
    [HideInInspector]
    public TargetObject m_TargetObject;

    [SerializeField]
    private float m_RotateSpeed = 5;


    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        transform.Rotate(new Vector3(0, m_RotateSpeed * Time.deltaTime, 0));
    }

    /// <summary>
    /// 初期位置設定
    /// </summary>
    public void Initialize(Vector3 pos, Quaternion rot)
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
    }

    /// <summary>
    /// ターゲット被弾時
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="damageable"></param>
    public void Hit(Damager damager, Damageable damageable)
    {
        m_TargetObject.ReturnToPool();
    }
}
