using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavMeshTest : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    private NavMeshAgent agent;
    private new Rigidbody rigidbody;
    [SerializeField]
    private ShootTest shootTest;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float turnSpeed = 5f;

    Quaternion nextRotate;

    private void Awake()
    {
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        if(shootTest == null)shootTest = GetComponent<ShootTest>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var m = m_PlayerInput.actions["Move"].ReadValue<Vector2>();

        var f = m_PlayerInput.currentActionMap["Fire"].WasPressedThisFrame();

        Vector3 move = (transform.forward * m.y).normalized;
        agent.Move(move * Time.deltaTime * moveSpeed);

        float turn = m.x * turnSpeed;
        nextRotate = Quaternion.Euler(0, turn, 0).normalized;

        if(f)
        {
            shootTest.Fire();
        }
    }

    private void FixedUpdate()
    {
        Quaternion q = (rigidbody.rotation * nextRotate).normalized;
        rigidbody.MoveRotation(q);
        nextRotate = Quaternion.identity;
    }
}
