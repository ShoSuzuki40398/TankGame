using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    [SerializeField]
    private Transform m_BulletSpawnPoint;   // 弾発射位置

    // 砲弾プール
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
    /// 砲弾生成
    /// プレハブから生成し、衝突判定のためにプレイヤーor敵のどちらから発射されたかを判別するため
    /// レイヤーを設定する
    /// </summary>
    /// <returns></returns>
    private ShellObject SpawnShell()
    {
        // 弾を生成し座標と方向を設定
        ShellObject shellObj = m_ShellPool.Pop();
        shellObj.shell.Initialize(m_BulletSpawnPoint.position, m_BulletSpawnPoint.rotation);
        return shellObj;
    }

    /// <summary>
    /// 射撃
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
