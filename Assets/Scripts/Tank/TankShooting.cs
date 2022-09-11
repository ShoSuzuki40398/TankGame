using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ̎ˌ�������s��
/// </summary>
public class TankShooting : MonoBehaviour
{
    [SerializeField]
    private Transform m_BulletSpawnPoint;   // �e���ˈʒu

    [SerializeField]
    private AudioSource m_FireSound;        // �ˌ���

    private bool m_IsFire = true;           // ���ˉ\�t���O
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
   private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.

    // �C�e�v�[��
    [SerializeField]
    private ShellPool m_ShellPool;

    // ���g�̖C�e���C���[
    [SerializeField]
    private string selfShellLayer;


    // Start is called before the first frame update
    void Start()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
    
    /// <summary>
    /// �e�𔭎ˏo���邩�m�F����
    /// </summary>
    /// <returns>true�F�@false�F�s��</returns>
    private bool CheckSpawnShell()
    {
        return m_IsFire;
    }

    private ShellObject SpawnShell()
    {
        // �e�𐶐������W�ƕ�����ݒ�
        ShellObject shellObj = m_ShellPool.Pop();
        shellObj.shell.Initialize(m_BulletSpawnPoint.position,m_BulletSpawnPoint.rotation);
        shellObj.shell.SetLayer(selfShellLayer);
        return shellObj;
    }

    /// <summary>
    /// �ˌ�
    /// </summary>
    public void Fire()
    {
        if (!CheckSpawnShell())
            return;
        MyDebug.Log("����");
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

        // �C�e�đ��U
        StartCoroutine(RecavShell());
    }

    /// <summary>
    /// �C�e�đ��U
    /// ���Ԏw��ōĎˌ��܂ł̃C���^�[�o�����s��
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecavShell()
    {
        yield return new WaitForSeconds(1.0f);

        m_IsFire = true;
    }
}
