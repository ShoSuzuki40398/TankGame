using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーン開始時に子要素に存在する「RespawnPoint」タグが付いたオブジェクトを
/// リスポーン地点として保持する
/// リスポーン要求に応じて保持しているオブジェクトの情報を返す
/// </summary>
public class Respawner : MonoBehaviour
{
    // リスポーン位置リスト
    private GameObject[] m_RespawnPoints = null;

    // リスポーンタグ
    private const string m_RespawnTag = "RespawnPoint";

    // リスポーン種類
    public enum RespawnMode
    {
        RANDOM, // ランダムリスポーン
        FAR,    // 最遠距離リスポーン
        NEAR    // 最近距離リスポーン
    }
    [SerializeField]
    private RespawnMode m_RespawnMode = RespawnMode.RANDOM;


    private void Awake()
    {
        m_RespawnPoints = GameObject.FindGameObjectsWithTag(m_RespawnTag);
    }

    /// <summary>
    /// リスポーン位置を取得
    /// </summary>
    /// <returns></returns>
    public Transform GetRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        switch (m_RespawnMode)
        {
            case RespawnMode.RANDOM: res = GetRandomRespawnPoint(); break;
            case RespawnMode.FAR: res = GetFarRespawnPoint(target); break;
            case RespawnMode.NEAR: res = GetNearRespawnPoint(target); break;
        }

        return res;
    }

    /// <summary>
    /// リスポーン位置が存在するか
    /// リスポーン位置がないというパターンは基本的に想定していないので
    /// あらかじめ1つ以上はリスポーン用のオブジェクトを保持しておくこと。
    /// </summary>
    /// <returns></returns>
    private bool ExistRespawnPoint()
    {
        if (m_RespawnPoints == null) return false;
        if (m_RespawnPoints.Length == 0) return false;

        return true;
    }

    /// <summary>
    /// ランダムなリスポーン位置を取得する
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomRespawnPoint()
    {
        Transform res = transform;

        // リスポーン位置がない場合は自分の位置を返す。
        if (!ExistRespawnPoint())
            return res;

        // ランダムに配列のオブジェクトのインデックスを決定して返す
        int index = Random.Range(0, m_RespawnPoints.Length);
        
        return m_RespawnPoints[index].transform;
    }

    /// <summary>
    /// 指定した位置から一番遠いリスポーン位置を取得する
    /// </summary>
    /// <returns></returns>
    private Transform GetFarRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        // リスポーン位置がない場合は自分の位置を返す。
        if (!ExistRespawnPoint())
            return res;

        // 最大距離を測る
        // 最初に配列先頭のリスポーン位置への距離を最大距離として保持する。
        // その後先頭以外のリスポーン位置と比較して最大距離のリスポーン位置を返す
        // ※変更案：距離の閾値を決めて、一定の距離以上離れたリスポーン位置を返す
        // ※全てのリスポーン位置と比較する必要がないためスループットは上がる
        Vector3 respawnPos = m_RespawnPoints[0].transform.position;
        float maxDistance = Vector3.Distance(m_RespawnPoints[0].transform.position,target);
        int index = 0;
        for (int i = index + 1; i < m_RespawnPoints.Length; ++i)
        {
            respawnPos = m_RespawnPoints[i].transform.position;
            float distance = Vector3.Distance(respawnPos, target);
            
            if (distance > maxDistance)
            {
                maxDistance = distance;
                index = i;
            }
        }
        
        return m_RespawnPoints[index].transform;
    }

    /// <summary>
    /// 指定した位置から一番遠いリスポーン位置を取得する
    /// </summary>
    /// <returns></returns>
    private Transform GetNearRespawnPoint(Vector3 target)
    {
        Transform res = transform;

        // リスポーン位置がない場合は自分の位置を返す。
        if (!ExistRespawnPoint())
            return res;

        // 最小距離を測る
        // 最初に配列先頭のリスポーン位置への距離を最小距離として保持する。
        // その後先頭以外のリスポーン位置と比較して最小距離のリスポーン位置を返す
        Vector3 respawnPos = m_RespawnPoints[0].transform.position;
        float minDistance = Vector3.Distance(m_RespawnPoints[0].transform.position, target);
        int index = 0;

        for (int i = index + 1; i < m_RespawnPoints.Length; ++i)
        {
            float distance = Vector3.Distance(respawnPos, target);

            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }

        return m_RespawnPoints[index].transform;
    }
}
