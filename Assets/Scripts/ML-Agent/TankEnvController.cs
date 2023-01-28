using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

/// <summary>
/// タンクゲームAIの学習環境制御
/// </summary>
public class TankEnvController : MonoBehaviour
{
    [System.Serializable]
    public class TankAgentInfo
    {
        public AutomationTank agent;
        [HideInInspector]
        public Rigidbody rigidbody;
    }

    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    public List<TankAgentInfo> m_AgentList = new List<TankAgentInfo>();

    private SimpleMultiAgentGroup m_EnemyAgentGroup;
    private SimpleMultiAgentGroup m_PlayerAgentGroup;

    public Respawner respawner;

    private int m_ResetTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_EnemyAgentGroup = new SimpleMultiAgentGroup();
        m_PlayerAgentGroup = new SimpleMultiAgentGroup();

        foreach (var item in m_AgentList)
        {
            item.rigidbody = item.agent.GetComponentInChildren<Rigidbody>();
            if (item.agent.Tank.layer == LayerMask.NameToLayer("Enemy"))
            {
                m_EnemyAgentGroup.RegisterAgent(item.agent);
            }
            else if(item.agent.Tank.layer == LayerMask.NameToLayer("Player"))
            {
                m_PlayerAgentGroup.RegisterAgent(item.agent);
            }
        }

        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_EnemyAgentGroup.GroupEpisodeInterrupted();
            m_PlayerAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    public void ResetScene()
    {
        m_ResetTimer = 0;

        foreach (var item in m_AgentList)
        {
            var resPoint = respawner.GetRespawnPoint(item.agent.transform.localPosition);
            item.agent.Tank.transform.SetPositionAndRotation(resPoint.position, resPoint.localRotation);

            item.rigidbody.velocity = Vector3.zero;
            item.rigidbody.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 相手を破壊した時にリワードを計算しエピソードをリセットする
    /// </summary>
    /// <param name="layerName">破壊された側</param>
    public void DestroyOpponent(int layerName)
    {
        
        if (layerName == LayerMask.NameToLayer("Player"))
        {   // プレイヤー側を破壊した時
            //Debug.Log("Playerやられた");
            m_EnemyAgentGroup.AddGroupReward(1);
            m_EnemyAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_PlayerAgentGroup.AddGroupReward(-1);
        }
        else if(layerName == LayerMask.NameToLayer("Enemy"))
        {   // 敵側を破壊した時
            //Debug.Log("敵やられた");
            m_PlayerAgentGroup.AddGroupReward(1);
            m_PlayerAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_EnemyAgentGroup.AddGroupReward(-1);
        }
        m_EnemyAgentGroup.EndGroupEpisode();
        m_PlayerAgentGroup.EndGroupEpisode();
        ResetScene();

    }
}
