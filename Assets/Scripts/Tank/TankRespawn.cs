using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 戦車リスポーン制御
/// 事前にシーンに配置した「Respowner」に対してリスポーン座標要求を行います
/// リスポーン時の処理はこちらのクラスで担当します。（HPバーのリセットや戦車の初期化等）
/// </summary>
public class TankRespawn : MonoBehaviour
{
    // リスポーン制御
    [SerializeField]
    private Respawner m_Respawner;

    // HP表示
    [SerializeField]
    private TankHealth m_TankHealth;

    // リスポーン開始までの時間
    [SerializeField]
    private float m_StartRespawnCount = 2.5f;

    // リスポーン時イベントの遅延時間
    // リスポーンして即UI等を再表示すると
    // リスポーン前の座標にチラつくので一瞬だけ遅延かけます
    private const float k_EventDelayTime = 0.15f;

    // リスポーン前のイベント
    public UnityEvent OnPreRepawnEvent;

    // リスポーン時のイベント
    // m_StartRespawnCount分遅延させる
    public UnityEvent OnRespawnEvent;
        
    /// <summary>
    /// リスポーン開始
    /// </summary>
    public void Respawn(Damager damager,Damageable damageable)
    {
        this.Delay(m_StartRespawnCount, ()=>Initialize(damager,damageable));
    }
    
    /// <summary>
    /// 座標、パラメーター等の初期化
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="damageable"></param>
    private void Initialize(Damager damager, Damageable damageable)
    {
        // リスポーン前イベント
        PreExecuteRepawnEvent();

        // 死んだ場所をリスポーン制御に送り、リスポーン位置を取得する
        Vector3 pos = transform.position;
        Transform respawnPoint = m_Respawner.GetRespawnPoint(pos);

        // 位置と回転の初期化
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        // 体力設定
        damageable.ResetHealth();
        m_TankHealth.HPChange(damageable.CurrentHealth);


        // ※リスポーンする時にリスポーン前の座標に一瞬HPバーが表示される不具合がある
        // 　原因がよく分からないので暫定的に少しだけ遅延をかけてます
        this.Delay(k_EventDelayTime, ()=> ExecuteRepawnEvent());
    }

    /// <summary>
    /// リスポーン前イベント実行
    /// </summary>
    private void PreExecuteRepawnEvent()
    {
        OnPreRepawnEvent?.Invoke();
    }

    /// <summary>
    /// リスポーン時イベント実行
    /// </summary>
    private void ExecuteRepawnEvent()
    {
        m_TankHealth.ShowGauge();
        OnRespawnEvent?.Invoke();
    }
}
