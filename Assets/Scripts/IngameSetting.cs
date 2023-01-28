using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインシーンで使用する情報の設定を保持するクラス
/// </summary>
public class IngameSetting : SingletonMonoBehaviour<IngameSetting>
{
    public LevelArtProperty CurrentLevelArtProperty { get; private set; } = new LevelArtProperty();

    /// <summary>
    /// レベルアート種類からレベルアート情報を設定する
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
    /// レベルアートアセットからレベルアート情報を設定する
    /// </summary>
    /// <param name="asset"></param>
    public void SetCurrentLevelArtProperty(LevelArtPropertyAsset asset)
    {
        CurrentLevelArtProperty = asset.levelArtProperty;
    }

    /// <summary>
    /// 設定リセット
    /// </summary>
    public void ResetSetting()
    {
    }
}
