using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializeExtension;
using UnityEngine.AddressableAssets;
using System;

/// <summary>
/// LevelArt�A�Z�b�g�Ǎ��E����Ǘ�
/// </summary>
public class LevelArtLoader : SingletonMonoBehaviour<LevelArtLoader>, IScriptableObjectSerialize<LevelArtPropertyAsset, LevelArtLoader.LEVEL_ART_TYPE>
{
    // ���x���A�[�g���
    public enum LEVEL_ART_TYPE
    {
        LEVEL_ART1,
        LEVEL_ART2,
        LEVEL_ART3,
    }

    // �p�X�Ǘ�
    [SerializeField]
    private List<LevelArtPropertyAsset> m_LevelArtPropaties = new List<LevelArtPropertyAsset>();

    private AssetReferenceGameObject m_CurrentAseet = null;
    private LevelArtProperty m_CurrentLevelArtProperty = null;

    // �v���C���[�^���N
    [SerializeField]
    private GameObject m_PlayerPrefab;
    private PlayableTank m_PlayerTank;
    public PlayableTank PlayableTank { get { return m_PlayerTank; } }

    // �G�^���N
    [SerializeField]
    private GameObject m_EnemyPrefab;
    private AutomationTank m_EnemyTank;
    public AutomationTank EnemyTank { get { return m_EnemyTank; } }
    
    /// <summary>
    /// �w���ނ̃��x���A�[�g�A�Z�b�g���擾����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public LevelArtPropertyAsset GetPropertyAsset(LEVEL_ART_TYPE type)
    {
        return m_LevelArtPropaties.Find(ele => ele.levelArtProperty.LevelArtType == type);
    }

    /// <summary>
    /// �w���ނ̃��x���A�[�g�A�Z�b�g����v���n�u�𐶐�����
    /// TODO:�񓯊��ɂ��Ēl�i�v���n�u�C���X�^���X�j��Ԃ�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void InstantiateFromProperty(LEVEL_ART_TYPE type, Action completed = null)
    {
        LevelArtPropertyAsset levelArtPropertyAsset = GetPropertyAsset(type);

        if (levelArtPropertyAsset == null)
        {
            Debug.Log("levelArtPropertyAsset�Ȃ�");
            return;
        }
            

        Debug.Log(levelArtPropertyAsset.levelArtProperty.LevelArtPath);
        m_CurrentAseet = levelArtPropertyAsset.levelArtProperty.LevelArtPath;
        m_CurrentLevelArtProperty = levelArtPropertyAsset.levelArtProperty;
        var handle = m_CurrentAseet.InstantiateAsync(Vector3.zero, Quaternion.identity);

        handle.Completed += op =>
        {
            // ��Ԑ���
            Transform playerPos = op.Result.transform.Find(CommonDefineData.ObjectNamePlayerInitPos);
            Transform enemyPos = op.Result.transform.Find(CommonDefineData.ObjectNameEnemyInitPos);
            Respawner respawner = op.Result.GetComponentInChildren<Respawner>();
            CreateTanks(playerPos, enemyPos,respawner);
            completed?.Invoke();
        };
    }

    /// <summary>
    /// �v���C���[�ƓG�̐�Ԃ𐶐�
    /// ���łɏ����ʒu���w�肷��
    /// </summary>
    public void CreateTanks(Transform playerPos, Transform enemyPos,Respawner respawner)
    {
        m_PlayerTank = Instantiate(m_PlayerPrefab).GetComponent<PlayableTank>();
        m_EnemyTank = Instantiate(m_EnemyPrefab).GetComponent<AutomationTank>();

        // �v���C���[
        m_PlayerTank.transform.position = playerPos.position;
        m_PlayerTank.transform.rotation = playerPos.rotation;
        m_PlayerTank.GetComponentInChildren<TankRespawn>().SetRespawner(respawner);

        // �G
        m_EnemyTank.transform.position = enemyPos.position;
        m_EnemyTank.transform.rotation = enemyPos.rotation;
        m_EnemyTank.GetComponentInChildren<TankRespawn>().SetRespawner(respawner);
        m_EnemyTank.m_TankMovement.Speed = m_CurrentLevelArtProperty.EnemySpeed;
        m_EnemyTank.m_TankMovement.TurnSpeed = m_CurrentLevelArtProperty.EnemyRotateSpeed;
    }
}
