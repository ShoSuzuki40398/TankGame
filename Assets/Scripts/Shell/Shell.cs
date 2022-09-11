using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    [HideInInspector]
    public ShellObject m_ShellObject;
    public Rigidbody m_Rigidbody { get; private set; }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// èâä˙à íuê›íË
    /// </summary>
    public void Initialize(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    private void Release()
    {
        m_ShellObject.ReturnToPool();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter : " + other.name);
        Release();
    }
}
