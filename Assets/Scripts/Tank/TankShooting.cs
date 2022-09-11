using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車の射撃制御を行う
/// </summary>
public class TankShooting : MonoBehaviour
{
    [SerializeField]
    private Transform m_BulletSpawnPoint;   // 弾発射位置

    [SerializeField]
    private AudioSource m_FireSound;        // 射撃音

    private bool m_IsFire = true;           // 発射可能フラグ
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
   private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    // 砲弾プール
    [SerializeField]
    private ShellPool m_ShellPool;

    // 自身の砲弾レイヤー
    [SerializeField]
    private string selfShellLayer;


    // Start is called before the first frame update
    void Start()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
    
    /// <summary>
    /// 弾を発射出来るか確認する
    /// </summary>
    /// <returns>true：可　false：不可</returns>
    private bool CheckSpawnShell()
    {
        return m_IsFire;
    }

    private ShellObject SpawnShell()
    {
        // 弾を生成し座標と方向を設定
        ShellObject shellObj = m_ShellPool.Pop();
        shellObj.shell.Initialize(m_BulletSpawnPoint.position,m_BulletSpawnPoint.rotation);
        shellObj.shell.SetLayer(selfShellLayer);
        return shellObj;
    }

    /// <summary>
    /// 射撃
    /// </summary>
    public void Fire()
    {
        if (!CheckSpawnShell())
            return;
        MyDebug.Log("発射");
        // Create an instance of the shell and store a reference to it's rigidbody.
        ShellObject shellObj = SpawnShell();
        Rigidbody shellRigidbody = shellObj.shell.m_Rigidbody;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellRigidbody.velocity = m_CurrentLaunchForce * m_BulletSpawnPoint.forward;

        // Change the clip to the firing clip and play it.
        //m_ShootingAudio.clip = m_FireClip;
        //m_ShootingAudio.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;

        m_IsFire = false;

        // 砲弾再装填
        StartCoroutine(RecavShell());
    }

    /// <summary>
    /// 砲弾再装填
    /// 時間指定で再射撃までのインターバルを行う
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecavShell()
    {
        yield return new WaitForSeconds(1.0f);

        m_IsFire = true;
    }
}
