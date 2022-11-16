using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 残機制御
/// </summary>
public class TankRemain : MonoBehaviour
{
    // 初期残機数
    [SerializeField]
    private int m_InitRemainCount = 3;
    
    // 現在残機数
    private int m_CurrentRemainCount = 0;

    // 残機消費時イベント
    [SerializeField]
    private UnityEvent OnSubRemain;

    // 残機全消費時イベント
    [SerializeField]
    private UnityEvent OnLostAllRemain;

    // UI ---------------------------------------------------
    // 戦車座標
    // HPゲージを対象の座標に追従させる
    private Transform tankTransform;
    [SerializeField]
    private Vector3 m_ViewOffset = Vector3.zero;

    // 表示キャンバス
    [SerializeField]
    private Canvas m_TargetCanvas;

    // 残機表示
    [SerializeField]
    private GameObject m_RemainViewPerfab;
    private RemainView m_RemainView;

    private void Awake()
    {
        tankTransform = transform;
        Initialize();
    }

    private void Update()
    {
        UpdateViewPosition();
    }

    /// <summary>
    /// HPバー表示位置更新
    /// </summary>
    private void UpdateViewPosition()
    {
        Vector3 pos = tankTransform.position + m_ViewOffset;
        m_RemainView.transform.position = WorldTo2DTranform.Transform(pos, m_TargetCanvas, Camera.main);
    }

    /// <summary>
    /// 残機初期化
    /// </summary>
    public void Initialize()
    {
        // 初期残機を設定する
        SetRemain(m_InitRemainCount);

        // 残機UIを生成してRemainViewの参照を取得
        m_RemainView = Instantiate(m_RemainViewPerfab, m_TargetCanvas.transform).GetComponent<RemainView>();

        // UIに残機数を反映する
        m_RemainView.Initialize(m_InitRemainCount);
    }

    /// <summary>
    /// 指定した数の現在残機数を増やす
    /// </summary>
    /// <param name="count"></param>
    public void AddRemain(int count)
    {
        // 現在残機数から指定数増やした値を設定する
        SetRemain(m_CurrentRemainCount + count);
    }

    /// <summary>
    /// 指定した数の現在残機数を減らす
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        // 現在残機数から指定数減らした値を設定する
        SetRemain(m_CurrentRemainCount - count);

        // 残機UIに反映する
        m_RemainView.SubRemain(count);

        // 残機消費時イベント発火
        OnSubRemain?.Invoke();

        // 減らした結果,残機が0になったら全消費イベント発火
        if (m_CurrentRemainCount == 0)
            OnLostAllRemain?.Invoke();
    }

    /// <summary>
    /// 現在残機数を設定
    /// 以下は設定できない
    /// ・初期残機数を超える数
    /// ・0未満
    /// </summary>
    public void SetRemain(int count)
    {
        int clamp = Mathf.Clamp(count, 0, m_InitRemainCount);
        m_CurrentRemainCount = clamp;
    }

    /// <summary>
    /// 残機UI表示
    /// </summary>
    public void ShowRemainView()
    {
        m_RemainView.gameObject.Enable();
    }

    /// <summary>
    /// 残機UI非表示
    /// </summary>
    public void HideRemainView()
    {
        m_RemainView.gameObject.Disable();
    }
}
