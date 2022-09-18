using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�[���J�n���Ɏq�v�f�ɑ��݂���uRespawnPoint�v�^�O���t�����I�u�W�F�N�g��
/// ���X�|�[���n�_�Ƃ��ĕێ�����
/// ���X�|�[���v���ɉ����ĕێ����Ă���I�u�W�F�N�g�̏���Ԃ�
/// </summary>
public class Respawner : MonoBehaviour
{
    // ���X�|�[���ʒu���X�g
    private GameObject[] m_RespawnPoints = null;

    // ���X�|�[���^�O
    private const string m_RespawnTag = "RespawnPoint";

    // ���X�|�[�����
    public enum RespawnMode
    {
        RANDOM, // �����_�����X�|�[��
        FAR,    // �ŉ��������X�|�[��
        NEAR    // �ŋߋ������X�|�[��
    }
    [SerializeField]
    private RespawnMode m_RespawnMode = RespawnMode.RANDOM;


    private void Awake()
    {
        m_RespawnPoints = GameObject.FindGameObjectsWithTag(m_RespawnTag);
    }

    /// <summary>
    /// ���X�|�[���ʒu���擾
    /// </summary>
    /// <returns></returns>
    public Transform GetRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        switch (m_RespawnMode)
        {
            case RespawnMode.RANDOM: res = GetRandomRespawnPoint(); break;
            case RespawnMode.FAR: res = GetFarRespawnPoint(target); break;
            case RespawnMode.NEAR: res = GetNearRespawnPoint(target); break;
        }

        return res;
    }

    /// <summary>
    /// ���X�|�[���ʒu�����݂��邩
    /// ���X�|�[���ʒu���Ȃ��Ƃ����p�^�[���͊�{�I�ɑz�肵�Ă��Ȃ��̂�
    /// ���炩����1�ȏ�̓��X�|�[���p�̃I�u�W�F�N�g��ێ����Ă������ƁB
    /// </summary>
    /// <returns></returns>
    private bool ExistRespawnPoint()
    {
        if (m_RespawnPoints == null) return false;
        if (m_RespawnPoints.Length == 0) return false;

        return true;
    }

    /// <summary>
    /// �����_���ȃ��X�|�[���ʒu���擾����
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomRespawnPoint()
    {
        Transform res = transform;

        // ���X�|�[���ʒu���Ȃ��ꍇ�͎����̈ʒu��Ԃ��B
        if (!ExistRespawnPoint())
            return res;

        // �����_���ɔz��̃I�u�W�F�N�g�̃C���f�b�N�X�����肵�ĕԂ�
        int index = Random.Range(0, m_RespawnPoints.Length);
        
        return m_RespawnPoints[index].transform;
    }

    /// <summary>
    /// �w�肵���ʒu�����ԉ������X�|�[���ʒu���擾����
    /// </summary>
    /// <returns></returns>
    private Transform GetFarRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        // ���X�|�[���ʒu���Ȃ��ꍇ�͎����̈ʒu��Ԃ��B
        if (!ExistRespawnPoint())
            return res;

        // �ő勗���𑪂�
        // �ŏ��ɔz��擪�̃��X�|�[���ʒu�ւ̋������ő勗���Ƃ��ĕێ�����B
        // ���̌�擪�ȊO�̃��X�|�[���ʒu�Ɣ�r���čő勗���̃��X�|�[���ʒu��Ԃ�
        // ���ύX�āF������臒l�����߂āA���̋����ȏ㗣�ꂽ���X�|�[���ʒu��Ԃ�
        // ���S�Ẵ��X�|�[���ʒu�Ɣ�r����K�v���Ȃ����߃X���[�v�b�g�͏オ��
        Vector3 respawnPos = m_RespawnPoints[0].transform.position;
        float maxDistance = Vector3.Distance(m_RespawnPoints[0].transform.position,target);
        int index = 0;
        for (int i = index + 1; i < m_RespawnPoints.Length; ++i)
        {
            respawnPos = m_RespawnPoints[i].transform.position;
            float distance = Vector3.Distance(respawnPos, target);
            
            if (distance > maxDistance)
            {
                maxDistance = distance;
                index = i;
            }
        }
        
        return m_RespawnPoints[index].transform;
    }

    /// <summary>
    /// �w�肵���ʒu�����ԉ������X�|�[���ʒu���擾����
    /// </summary>
    /// <returns></returns>
    private Transform GetNearRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        // ���X�|�[���ʒu���Ȃ��ꍇ�͎����̈ʒu��Ԃ��B
        if (!ExistRespawnPoint())
            return res;

        // �ŏ������𑪂�
        // �ŏ��ɔz��擪�̃��X�|�[���ʒu�ւ̋������ŏ������Ƃ��ĕێ�����B
        // ���̌�擪�ȊO�̃��X�|�[���ʒu�Ɣ�r���čŏ������̃��X�|�[���ʒu��Ԃ�
        Vector3 respawnPos = m_RespawnPoints[0].transform.position;
        float minDistance = Vector3.Distance(m_RespawnPoints[0].transform.position, target);
        int index = 0;

        for (int i = index + 1; i < m_RespawnPoints.Length; ++i)
        {
            float distance = Vector3.Distance(respawnPos, target);

            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }

        return m_RespawnPoints[index].transform;
    }
}
