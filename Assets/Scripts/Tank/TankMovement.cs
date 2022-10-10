using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車移動
/// ↑↓で前進後退
/// ←→で自身のY軸を中心に回転をする
/// 移動方法にRigidbodyを使用する
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TankMovement : MonoBehaviour
{
    [SerializeField,Tooltip("移動速度")]
    private float m_Speed = 1f;
    [SerializeField, Tooltip("回転速度")]
    private float m_TurnSpeed = 180f;

    // 土埃エフェクト
    [SerializeField]
    private List<ParticleSystem> m_DustTrails = new List<ParticleSystem>();

    // エンジン音
    // 移動している時だけ再生するようにする
    [SerializeField]
    private AudioSource m_EngineSound;

    // 1フレームの速度
    public Vector3 Velocity { get; protected set; }

    // 移動用リジッドボディ
    private Rigidbody m_Rigidbody;
    private Vector3 m_PreviousPosition; // 前座標
    private Vector3 m_CurrentPosition;  // 現在座標
    private Vector3 m_NextMovement;     // 移動量

    private Vector3 m_PreviousRotation; // 前回転
    private Vector3 m_CurrentRotation;  // 現在回転
    private Quaternion m_NextRotate;       // 回転量


    private void Awake()
    {
        // アタッチ済みコンポーネント取得
        m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody != null)
            m_CurrentPosition = m_PreviousPosition = m_Rigidbody.position;
    }

    private void FixedUpdate()
    {
        EngineSoundUpdate();

        // 座標と回転更新
        PositionUpdate();
        RotationUpdate();
    }

    /// <summary>
    /// 座標更新
    /// </summary>
    private void PositionUpdate()
    {
        // 移動量から次の座標を設定する
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_NextMovement);
        m_NextMovement = Vector3.zero;
    }

    /// <summary>
    /// 回転更新
    /// </summary>
    private void RotationUpdate()
    {
        Quaternion q = (m_Rigidbody.rotation * m_NextRotate).normalized;
        m_Rigidbody.MoveRotation(q);
        m_NextRotate = Quaternion.identity;
    }

    /// <summary>
    /// シンプルに座標移動
    /// </summary>
    /// <param name="speedScale">移動倍率</param>
    public void Move( float speedScale = 1.0f)
    {
        m_NextMovement = transform.forward * speedScale * m_Speed * Time.deltaTime;
    }

    /// <summary>
    /// Y軸を中心に座標回転
    /// </summary>
    /// <param name="speedScale">回転倍率　兼　回転方向</param>
    public void Turn(float speedScale = 1.0f)
    {
        float turn = speedScale * m_TurnSpeed * Time.deltaTime;
        m_NextRotate = Quaternion.Euler(0, turn, 0).normalized;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EngineSoundUpdate()
    {
        if(Mathf.Approximately(m_NextMovement.magnitude,0))
        {
            m_EngineSound.Stop();
            return;
        }

        if(!m_EngineSound.isPlaying)
            m_EngineSound.Play();
        
    }

    /// <summary>
    /// 走行エフェクト表示
    /// </summary>
    public void EffectEnable()
    {
        foreach (var eff in m_DustTrails)
        {
            eff.gameObject.Enable();
        }
    }

    /// <summary>
    /// 走行エフェクト非表示
    /// </summary>
    public void EffectDisable()
    {
        m_EngineSound.Stop();
        foreach (var eff in m_DustTrails)
        {
            eff.gameObject.Disable();
        }
    }
}
