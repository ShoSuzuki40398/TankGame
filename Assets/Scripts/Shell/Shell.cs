using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砲弾クラス
/// 射出した砲弾の制御
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    [HideInInspector]
    public ShellObject m_ShellObject;
    public Rigidbody m_Rigidbody { get; private set; }

    // 解放までの時間
    // 長すぎず短すぎずの塩梅を指定してください
    [SerializeField]
    private float releaseTime = 5.0f;

    // 解放タイマー
    // 衝突判定を検知せず自己解放ができない可能性を考慮
    // 指定時間を超えても解放されない場合はタイマーで解放させる
    // 基本的に使用しないが、安全マージンとして実装する
    private Timer releaseTimer = new Timer();

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        releaseTimer.OnComplete = Release;
    }

    private void Update()
    {
        releaseTimer.UpdateWithDeltaTime();
    }

    /// <summary>
    /// 初期位置設定
    /// </summary>
    public void Initialize(Vector3 pos,Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        releaseTimer.Awake(releaseTime);
    }

    /// <summary>
    /// 砲弾のレイヤーを設定
    /// 自分が撃ったか敵が撃ったかで衝突判定の有無を変えたい
    /// </summary>
    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    private void Release()
    {
        // プールに戻す
        m_ShellObject.ReturnToPool();
    }


    private void OnTriggerEnter(Collider other)
    {
        MyDebug.Log("OnTriggerEnter: " + other.name);
        Release();
    }
}
