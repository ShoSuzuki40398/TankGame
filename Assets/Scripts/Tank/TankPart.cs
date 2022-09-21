using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransformExtension;

/// <summary>
/// 戦車部品情報
/// 爆発演出のために戦車を構成する各オブジェクトにアタッチする。
/// 
/// </summary>
public class TankPart : InpulseBehaviour
{
    // タンクパーツのローカル座標初期値
    // パーツが弾ける演出をしたいので
    // 弾けたあとのリセットで初期位置に戻すために初期値を保持する
    private ExTransform m_InitLocalTransform;

    // 爆発経過時間計測
    private Timer m_InpulseTimer = new Timer();

    // 爆発所要時間
    [SerializeField]
    private float m_InpulseTime = 3.0f;

    private void Awake()
    {
        // 起動時に初期値を保持しておく
        m_InpulseProperty.Initialize();

        // 初期位置を保存する
        m_InitLocalTransform = new ExTransform(transform);
    }

    /// <summary>
    /// パーツ吹っ飛び開始
    /// </summary>
    public void StartInpulse()
    {
        StartCoroutine(UpdateInpulse());
    }

    /// <summary>
    /// パーツ吹っ飛び更新
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateInpulse()
    {
        // 所要時間設定
        m_InpulseTimer.Awake(m_InpulseTime);

        // 設定した時間分爆発処理
        while (!m_InpulseTimer.UpdateWithDeltaTime())
        {
            Vector3 inpulseVelocity = m_InpulseProperty.UpdateInpluse();

            transform.localPosition += inpulseVelocity;
            transform.Rotate(m_InpulseProperty.RatoteVelocity);

            yield return null;
        }

        // 初期位置に戻す
        ResetPart();
    }

    /// <summary>
    /// Transfromリセット
    /// </summary>
    public void ResetPart()
    {
        // 初期位置に戻す
        transform.localPosition = m_InitLocalTransform.position;
        transform.localRotation = m_InitLocalTransform.rotation;

        // 爆発処理リセット
        m_InpulseTimer.Reset();
        m_InpulseProperty.Reset();
    }
}
