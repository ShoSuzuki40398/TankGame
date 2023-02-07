using UnityEngine;
using System;

/// <summary>
/// ���x���A�[�g���
/// </summary>
[Serializable]
public class LevelArtProperty
{
    // ���x���A�[�g���
    [SerializeField]
    private LevelArtLoader.LEVEL_ART_TYPE m_LevelArtType = LevelArtLoader.LEVEL_ART_TYPE.LEVEL_ART1;
    public LevelArtLoader.LEVEL_ART_TYPE LevelArtType { get { return m_LevelArtType; } }

    // ���x���A�[�g�v���n�u
    [SerializeField]
    private GameObject m_LevelArt;
    public GameObject LevelArt { get { return m_LevelArt; } }

    // �G�ړ����x
    [SerializeField]
    private float m_EnemySpeed = 3;
    public float EnemySpeed { get { return m_EnemySpeed; } }

    // �G�ړ����x
    [SerializeField]
    private float m_EnemyRotateSpeed = 440;
    public float EnemyRotateSpeed { get { return m_EnemyRotateSpeed; } }

    public void Reset()
    {
        m_LevelArtType = LevelArtLoader.LEVEL_ART_TYPE.LEVEL_ART1;
    }
}
