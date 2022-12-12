using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    // �A�Z�b�g�A�h���X
    [SerializeField]
    private AssetReferenceGameObject m_LevelArtPath = null;
    public AssetReferenceGameObject LevelArtPath { get { return m_LevelArtPath; } }
        
    public void Reset()
    {
        m_LevelArtType = LevelArtLoader.LEVEL_ART_TYPE.LEVEL_ART1;
        m_LevelArtPath = null;
    }

    public bool Exist()
    {
        if (m_LevelArtPath == null)
            return false;

        return true;
    }
}
