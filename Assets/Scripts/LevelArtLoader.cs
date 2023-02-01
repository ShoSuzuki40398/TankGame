using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializeExtension;
using UnityEngine.AddressableAssets;
using System;

/// <summary>
/// LevelArtアセット読込・解放管理
/// </summary>
public class LevelArtLoader : SingletonMonoBehaviour<LevelArtLoader>, IScriptableObjectSerialize<LevelArtPropertyAsset, LevelArtLoader.LEVEL_ART_TYPE>
{
    // レベルアート種類
    public enum LEVEL_ART_TYPE
    {
        LEVEL_ART1,
        LEVEL_ART2,
        LEVEL_ART3,
    }

    // パス管理
    [SerializeField]
    private List<LevelArtPropertyAsset> m_LevelArtPropaties = new List<LevelArtPropertyAsset>();

    private AssetReferenceGameObject m_CurrentAseet = null;
    private LevelArtProperty m_CurrentLevelArtProperty = null;

    // プレイヤータンク
    [SerializeField]
    private GameObject m_PlayerPrefab;
    private PlayableTank m_PlayerTank;
    public PlayableTank PlayableTank { get { return m_PlayerTank; } }

    // 敵タンク
    [SerializeField]
    private GameObject m_EnemyPrefab;
    private AutomationTank m_EnemyTank;
    public AutomationTank EnemyTank { get { return m_EnemyTank; } }
    
    /// <summary>
    /// 指定種類のレベルアートアセットを取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public LevelArtPropertyAsset GetPropertyAsset(LEVEL_ART_TYPE type)
    {
        return m_LevelArtPropaties.Find(ele => ele.levelArtProperty.LevelArtType == type);
    }

    /// <summary>
    /// 指定種類のレベルアートアセットからプレハブを生成する
    /// TODO:非同期にして値（プレハブインスタンス）を返す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void InstantiateFromProperty(LEVEL_ART_TYPE type, Action completed = null)
    {
        LevelArtPropertyAsset levelArtPropertyAsset = GetPropertyAsset(type);

        if (levelArtPropertyAsset == null)
        {
            Debug.Log("levelArtPropertyAssetなし");
            return;
        }
            

        Debug.Log(levelArtPropertyAsset.levelArtProperty.LevelArtPath);
        m_CurrentAseet = levelArtPropertyAsset.levelArtProperty.LevelArtPath;
        m_CurrentLevelArtProperty = levelArtPropertyAsset.levelArtProperty;
        var handle = m_CurrentAseet.InstantiateAsync(Vector3.zero, Quaternion.identity);

        handle.Completed += op =>
        {
            // 戦車生成
            Transform playerPos = op.Result.transform.Find(CommonDefineData.ObjectNamePlayerInitPos);
            Transform enemyPos = op.Result.transform.Find(CommonDefineData.ObjectNameEnemyInitPos);
            Respawner respawner = op.Result.GetComponentInChildren<Respawner>();
            CreateTanks(playerPos, enemyPos,respawner);
            completed?.Invoke();
        };
    }

    /// <summary>
    /// プレイヤーと敵の戦車を生成
    /// ついでに初期位置を指定する
    /// </summary>
    public void CreateTanks(Transform playerPos, Transform enemyPos,Respawner respawner)
    {
        m_PlayerTank = Instantiate(m_PlayerPrefab).GetComponent<PlayableTank>();
        m_EnemyTank = Instantiate(m_EnemyPrefab).GetComponent<AutomationTank>();

        // プレイヤー
        m_PlayerTank.transform.position = playerPos.position;
        m_PlayerTank.transform.rotation = playerPos.rotation;
        m_PlayerTank.GetComponentInChildren<TankRespawn>().SetRespawner(respawner);

        // 敵
        m_EnemyTank.transform.position = enemyPos.position;
        m_EnemyTank.transform.rotation = enemyPos.rotation;
        m_EnemyTank.GetComponentInChildren<TankRespawn>().SetRespawner(respawner);
        m_EnemyTank.m_TankMovement.Speed = m_CurrentLevelArtProperty.EnemySpeed;
        m_EnemyTank.m_TankMovement.TurnSpeed = m_CurrentLevelArtProperty.EnemyRotateSpeed;
    }
}
