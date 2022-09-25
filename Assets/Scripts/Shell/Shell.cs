using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C�e�N���X
/// �ˏo�����C�e�̐���
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    [HideInInspector]
    public ShellObject m_ShellObject;
    public Rigidbody m_Rigidbody { get; private set; }

    // ����܂ł̎���
    // ���������Z�������̉��~���w�肵�Ă�������
    [SerializeField]
    private float m_ReleaseTime = 5.0f;

    // ����^�C�}�[
    // �Փ˔�������m�������ȉ�����ł��Ȃ��\�����l��
    // �w�莞�Ԃ𒴂��Ă��������Ȃ��ꍇ�̓^�C�}�[�ŉ��������
    // ��{�I�Ɏg�p���Ȃ����A���S�}�[�W���Ƃ��Ď�������
    private Timer m_ReleaseTimer = new Timer();

    // �����v���n�u
    [SerializeField]
    private GameObject m_ExplosionPrefab = null;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_ReleaseTimer.OnComplete = Release;
    }

    private void Update()
    {
        m_ReleaseTimer.UpdateWithDeltaTime();
    }

    /// <summary>
    /// �����ʒu�ݒ�
    /// </summary>
    public void Initialize(Vector3 pos, Quaternion rot)
    {
        // ���W�𔭎ˈʒu�ɐݒ�
        transform.position = pos;
        transform.rotation = rot;

        // ����^�C�}�[�N��
        m_ReleaseTimer.Awake(m_ReleaseTime);
    }

    /// <summary>
    /// �C�e�̃��C���[��ݒ�
    /// ���������������G�����������ŏՓ˔���̗L����ς�����
    /// </summary>
    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// �������
    /// </summary>
    private void Release()
    {
        // �v�[���ɖ߂�
        m_ShellObject.ReturnToPool();
    }

    /// <summary>
    /// �q�b�g���G�t�F�N�g�Đ�
    /// Damager��Hit�C�x���g�ɐݒ肵�܂�
    /// </summary>
    /// <param name="hit"></param>
    public void Hit()
    {
        // �^�C�}�[���Z�b�g
        // �Փ˂ŏ�����̂Ń^�C�}�[�ŏ����Ȃ��Ă��悭�Ȃ�
        m_ReleaseTimer.Reset();

        // �����G�t�F�N�g����
        Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name);

        Hit();

        Release();
    }
}
