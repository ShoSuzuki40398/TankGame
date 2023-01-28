using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���C���V�[���Ŏg�p������̐ݒ��ێ�����N���X
/// </summary>
public class IngameSetting : SingletonMonoBehaviour<IngameSetting>
{
    public LevelArtProperty CurrentLevelArtProperty { get; private set; } = new LevelArtProperty();

    /// <summary>
    /// ���x���A�[�g��ނ��烌�x���A�[�g����ݒ肷��
    /// </summary>
    /// <param name="type"></param>
    public void SetCurrentLevelArtProperty(LevelArtLoader.LEVEL_ART_TYPE type)
    {
        LevelArtPropertyAsset asset = LevelArtLoader.Instance.GetPropertyAsset(type);
        if (asset == null)
            return;

        SetCurrentLevelArtProperty(asset);
    }

    /// <summary>
    /// ���x���A�[�g�A�Z�b�g���烌�x���A�[�g����ݒ肷��
    /// </summary>
    /// <param name="asset"></param>
    public void SetCurrentLevelArtProperty(LevelArtPropertyAsset asset)
    {
        CurrentLevelArtProperty = asset.levelArtProperty;
    }

    /// <summary>
    /// �ݒ胊�Z�b�g
    /// </summary>
    public void ResetSetting()
    {
    }
}
