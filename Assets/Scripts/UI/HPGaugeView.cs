using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPゲージのView制御
/// 値の変動は0～1の割合で変化させる
/// </summary>
public class HPGaugeView : MonoBehaviour
{
    // HPバー見た目情報
    [System.Serializable]
    public class HPColorProperty
    {
        // 変更先の色
        public Color32 color;
        // 色変えをする閾値
        public float changeBorder;
    }
    
    // 見た目情報のリスト
    [SerializeField]
    private List<HPColorProperty> colorProperties = new List<HPColorProperty>();
    
    // 現在のHP見た目情報
    // Update等で毎フレーム監視するより、HPが変動した時に参照できればよいので
    // 保持する形にしました
    private HPColorProperty m_CurrentHPColorProperty;

    // HP残量イメージ
    [SerializeField]
    private Image m_Fill;

    // HP残量表示
    [SerializeField]
    private Slider m_Slider;

    // HP最大値
    public float MaxValue { get { return m_Slider.maxValue; } }
        
    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        // 最大値に変更
        HPColorProperty maxProperty = GetMaxHPColorProperty();
        ResetView(maxProperty);
    }

    /// <summary>
    /// 表示リセット
    /// </summary>
    private void ResetView(HPColorProperty hpProperty)
    {
        m_Slider.value = hpProperty.changeBorder;
        m_Fill.color = hpProperty.color;
        m_CurrentHPColorProperty = hpProperty;
        UpdateHPColorProperty();
    }

    /// <summary>
    /// 閾値が最大の見た目情報を取得
    /// (つまりHP最大値の情報)
    /// </summary>
    /// <returns></returns>
    private HPColorProperty GetMaxHPColorProperty()
    {
        return colorProperties.FindMax(p => p.changeBorder);
    }

    /// <summary>
    /// 残りHPを直接設定
    /// </summary>
    public void SetHP(float value)
    {
        float ratioValue = value * 0.01f;
        float clampValue = Mathf.Clamp(ratioValue, 0, MaxValue);
        m_Slider.value = clampValue;
        UpdateHPColorProperty();
    }

    /// <summary>
    /// HPバーの変動
    /// </summary>
    private void HPChange(float value)
    {
        m_Slider.value += value;
        m_Slider.value = Mathf.Clamp(m_Slider.value, 0, MaxValue);
        UpdateHPColorProperty();
    }

    /// <summary>
    /// HP減少
    /// 百分率で減算する
    /// </summary>
    public void SubHP(float value)
    {
        float ratioValue = value * 0.01f;
        HPChange(-ratioValue);
    }

    /// <summary>
    /// HP減少
    /// 百分率で加算する
    /// </summary>
    public void AddHP(float value)
    {
        float ratioValue = value * 0.01f;
        HPChange(ratioValue);
    }

    /// <summary>
    /// 現在の見た目情報を更新
    /// 現在のHP値から見た目の情報を更新する
    /// </summary>
    private void UpdateHPColorProperty()
    {
        float currentValue = m_Slider.value;

        // 念のため閾値の小→大でソートする
        var array = colorProperties.ToArray();
        array.Sort(pro => pro.changeBorder);

        foreach (var pro in array)
        {
            if(pro.changeBorder >= currentValue)
            {
                m_CurrentHPColorProperty = pro;
                break;
            }
        }

        if (m_CurrentHPColorProperty == null)
            return;

        // 現在のHP情報でバーの色を変更する
        m_Fill.color = m_CurrentHPColorProperty.color;
    }
}
