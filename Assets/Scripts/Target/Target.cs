using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�I�u�W�F�N�g����
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
    /// �����ʒu�ݒ�
    /// </summary>
    public void Initialize(Vector3 pos, Quaternion rot)
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
    }

    /// <summary>
    /// �^�[�Q�b�g��e��
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="damageable"></param>
    public void Hit(Damager damager, Damageable damageable)
    {
        m_TargetObject.ReturnToPool();
    }
}
