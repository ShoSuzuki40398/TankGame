using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    [SerializeField]
    private Transform m_BulletSpawnPoint;   // �e���ˈʒu

    // �C�e�v�[��
    [SerializeField]
    private ShellPool m_ShellPool;

    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    private void Awake()
    {
        if(m_ShellPool == null)m_ShellPool = GetComponent<ShellPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �C�e����
    /// �v���n�u���琶�����A�Փ˔���̂��߂Ƀv���C���[or�G�̂ǂ��炩�甭�˂��ꂽ���𔻕ʂ��邽��
    /// ���C���[��ݒ肷��
    /// </summary>
    /// <returns></returns>
    private ShellObject SpawnShell()
    {
        // �e�𐶐������W�ƕ�����ݒ�
        ShellObject shellObj = m_ShellPool.Pop();
        shellObj.shell.Initialize(m_BulletSpawnPoint.position, m_BulletSpawnPoint.rotation);
        return shellObj;
    }

    /// <summary>
    /// �ˌ�
    /// </summary>
    public void Fire()
    {
        ShellObject shellObj = SpawnShell();
        Rigidbody shellRigidbody = shellObj.shell.m_Rigidbody;
        shellRigidbody.velocity = m_CurrentLaunchForce * m_BulletSpawnPoint.forward;

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;
        
    }
}
