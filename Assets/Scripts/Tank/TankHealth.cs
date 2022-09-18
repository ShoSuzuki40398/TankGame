using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車の体力表示
/// UI上に戦車の体力を表示するクラスです。
/// 実際のHP管理はDamagebleクラスが担当しています。
/// </summary>
public class TankHealth : MonoBehaviour
{
    // 体力バー
    // シーンに配置したオブジェクトをアタッチしても良いが、
    // 戦車をプレハブにする関係で付属するHPバーを動的に作成した方が
    // 戦車をシーンに配置しやすくなる
    [SerializeField]
    private GameObject m_HPGaugeViewPrefab;
    private HPGaugeView m_HPGaugeView;

    // HPゲージを配置するCanvas
    [SerializeField]
    private Canvas m_HPGaugeViewParent;

    // 戦車座標
    // HPゲージを対象の座標に追従させる
    private Transform tankTransform;
    [SerializeField]
    private Vector3 m_ViewOffset = Vector3.zero;

    /// <summary>
    /// HPゲージ初期化
    /// </summary>
    /// <param name="hPGaugeView"></param>
    private void InitializeHPView(HPGaugeView hPGaugeView)
    {
        hPGaugeView.Initialize();
        UpdateViewPosition();
    }

    private void Awake()
    {
        tankTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        // HPゲージオブジェクトを生成してHPGaugeViewの参照を取得
        m_HPGaugeView = Instantiate(m_HPGaugeViewPrefab, m_HPGaugeViewParent.transform).GetComponent<HPGaugeView>();
        InitializeHPView(m_HPGaugeView);
    }

    private void Update()
    {
        UpdateViewPosition();
    }

    private void UpdateViewPosition()
    {
        Vector3 pos = tankTransform.position + m_ViewOffset;
        m_HPGaugeView.transform.position = WorldTo2DTranform.Transform(pos, m_HPGaugeViewParent, Camera.main);
    }

    /// <summary>
    /// HPを設定
    /// </summary>
    /// <param name="damageable"></param>
    public void HPChange(Damager damager, Damageable damageable)
    {
        m_HPGaugeView.SetHP(damageable.CurrentHealth);
    }

    /// <summary>
    /// HPを設定
    /// </summary>
    /// <param name="damageable"></param>
    public void HPChange(float health)
    {
        m_HPGaugeView.SetHP(health);
    }
}
