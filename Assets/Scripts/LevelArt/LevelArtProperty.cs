using UnityEngine;
using System;

/// <summary>
/// レベルアート情報
/// </summary>
[Serializable]
public class LevelArtProperty
{
    // レベルアート種類
    [SerializeField]
    private LevelArtLoader.LEVEL_ART_TYPE m_LevelArtType = LevelArtLoader.LEVEL_ART_TYPE.LEVEL_ART1;
    public LevelArtLoader.LEVEL_ART_TYPE LevelArtType { get { return m_LevelArtType; } }

    // レベルアートプレハブ
    [SerializeField]
    private GameObject m_LevelArt;
    public GameObject LevelArt { get { return m_LevelArt; } }

    // 敵移動速度
    [SerializeField]
    private float m_EnemySpeed = 3;
    public float EnemySpeed { get { return m_EnemySpeed; } }

    // 敵移動速度
    [SerializeField]
    private float m_EnemyRotateSpeed = 440;
    public float EnemyRotateSpeed { get { return m_EnemyRotateSpeed; } }

    public void Reset()
    {
        m_LevelArtType = LevelArtLoader.LEVEL_ART_TYPE.LEVEL_ART1;
    }
}
