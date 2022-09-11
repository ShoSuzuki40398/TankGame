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
    private float releaseTime = 5.0f;

    // ����^�C�}�[
    // �Փ˔�������m�������ȉ�����ł��Ȃ��\�����l��
    // �w�莞�Ԃ𒴂��Ă��������Ȃ��ꍇ�̓^�C�}�[�ŉ��������
    // ��{�I�Ɏg�p���Ȃ����A���S�}�[�W���Ƃ��Ď�������
    private Timer releaseTimer = new Timer();

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        releaseTimer.OnComplete = Release;
    }

    private void Update()
    {
        releaseTimer.UpdateWithDeltaTime();
    }

    /// <summary>
    /// �����ʒu�ݒ�
    /// </summary>
    public void Initialize(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        releaseTimer.Awake(releaseTime);
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


    private void OnTriggerEnter(Collider other)
    {
        MyDebug.Log("OnTriggerEnter: " + other.name);
        Release();
    }
}
