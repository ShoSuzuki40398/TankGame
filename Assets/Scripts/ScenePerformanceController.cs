using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// Timelineによるシーン演出制御
/// Timelineの管理をシーン演出タイプをキーにして配列で管理し、
/// キーによる指定で再生などを行う
/// </summary>
public class ScenePerformanceController : MonoBehaviour
{
    // シーン演出タイプ
    public enum PerformanceType
    {
        Scene_Start,
        Battle_End,
        DEMO,
    }

    // シーン演出テーブル
    [SerializeField]
    private ScenePerformanceTable m_ScenePerformanceTable;

    // Timeline再生制御
    [SerializeField]
    private PlayableDirector m_PlayableDirector;
    
    /// <summary>
    /// 演出再生
    /// </summary>
    /// <param name="type">演出タイプ</param>
    /// <param name="complete">終了時イベント</param>
    public void PlayOneShot(PerformanceType type,Action complete = null)
    {
        // キーでテーブルから取得して再生する
        var performance = m_ScenePerformanceTable.GetPerformance(type);

        if (performance == null)
            return;
        
        m_PlayableDirector.Play(performance?.asset, DirectorWrapMode.Hold);

        // 終了時イベント発火
        complete?.Invoke();
    }

}

/// <summary>
/// シーン演出テーブル
/// ScenePerformancePropertyの配列を管理して
/// 値の受け渡しをするためのパイプライン用クラス
/// </summary>
[Serializable]
public class ScenePerformanceTable
{
    // シーン演出配列
    // これがシーン演出の実体になる
    public List<ScenePerformanceProperty> m_ScenePerformanceProperties = new List<ScenePerformanceProperty>();

    // 管理演出数取得
    public int count { get { return m_ScenePerformanceProperties.Count; } }

    /// <summary>
    /// シーン演出取得
    /// 配列にない場合はnullで返すので呼び出し元でnullチェックは必ずすること
    /// </summary>
    public ScenePerformanceProperty? GetPerformance(ScenePerformanceController.PerformanceType type)
    {
        // キーで指定したタイプを検索して最初に一致したvalue(TimelineAsset)を再生する
        var result = m_ScenePerformanceProperties.Find(item =>
        {
            if(type == item.type)
                return true;

            return false;
        });
                
        return result;
    }
}


/// <summary>
/// シーン演出プロパティ
/// 変数アクセサーはpublicになっているのはスクリプトから変更する設計を見越しています
/// しかし、インスペクターから事前に設定することで済んでいる内は特にpublicの利点はありません。
/// インスペクターから事前に設定しかしない場合、SerializeField属性を付けてprivateに指定する
/// 設計に変更してもよい
/// </summary>
[Serializable]
public struct ScenePerformanceProperty
{
    // シーン演出キー
    public ScenePerformanceController.PerformanceType type;

    // PlayableAssetアセット
    public PlayableAsset asset;
}