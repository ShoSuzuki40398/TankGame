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

    private bool m_IsFire = true;           // ­ΛΒ\tO
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
   private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    // Cev[
    [SerializeField]
    private ShellPool m_ShellPool;

    // ©gΜCeC[
    [SerializeField]
    private string selfShellLayer;


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
        shellObj.shell.SetLayer(selfShellLayer);
        return shellObj;
    }

    /// <summary>
    /// Λ
    /// </summary>
    public void Fire()
    {
        if (!CheckSpawnShell())
            return;
        MyDebug.Log("­Λ");
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
        yield return new WaitForSeconds(1.0f);

        m_IsFire = true;
    }
}
