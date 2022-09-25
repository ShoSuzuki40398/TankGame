using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// νΤΜΛ§δπs€
/// </summary>
public class TankShooting : MonoBehaviour
{
    [SerializeField]
    private Transform m_BulletSpawnPoint;   // e­ΛΚu

    [SerializeField]
    private AudioSource m_FireSound;        // ΛΉ

    [SerializeField]
    private float m_FireInterval = 1.0f;    // ΔUΤ

    private bool m_IsFire = true;           // ­ΛΒ\tO
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
   private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    // Cev[
    [SerializeField]
    private ShellPool m_ShellPool;

    // ©gΜCeC[
    [SerializeField]
    private string m_SelfShellLayer;


    // Start is called before the first frame update
    void Start()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
    
    /// <summary>
    /// eπ­Λoι©mF·ι
    /// </summary>
    /// <returns>trueFΒ@falseFsΒ</returns>
    private bool CheckSpawnShell()
    {
        return m_IsFire;
    }

    private ShellObject SpawnShell()
    {
        // eπΆ¬΅ΐWΖϋόπέθ
        ShellObject shellObj = m_ShellPool.Pop();
        shellObj.shell.Initialize(m_BulletSpawnPoint.position,m_BulletSpawnPoint.rotation);
        shellObj.shell.SetLayer(m_SelfShellLayer);
        return shellObj;
    }

    /// <summary>
    /// Λ
    /// </summary>
    public void Fire()
    {
        if (!CheckSpawnShell())
            return;
        
        ShellObject shellObj = SpawnShell();
        Rigidbody shellRigidbody = shellObj.shell.m_Rigidbody;
        shellRigidbody.velocity = m_CurrentLaunchForce * m_BulletSpawnPoint.forward;

        // Change the clip to the firing clip and play it.
        m_FireSound.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;

        m_IsFire = false;

        // CeΔU
        StartCoroutine(RecavShell());
    }

    /// <summary>
    /// CeΔU
    /// ΤwθΕΔΛάΕΜC^[oπs€
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecavShell()
    {
        yield return new WaitForSeconds(m_FireInterval);

        m_IsFire = true;
    }
}
