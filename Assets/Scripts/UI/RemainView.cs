using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 残機表示ビュー
/// 残機の表示状態はImageのenabledで行う
/// </summary>
public class RemainView : MonoBehaviour
{
    // 残機イメージプレハブ
    // 初期化時に初期残機数分このプレハブを生成する
    [SerializeField]
    private GameObject m_RemainImage;

    // 残機配列
    private List<GameObject> m_RemainList = new List<GameObject>();

    /// <summary>
    /// 指定数分、残機イメージを非アクティブにする
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        var matches = m_RemainList.FindAll(remain =>
        {
            if (remain.GetComponent<Image>().enabled)
                return true;

            return false;
        });

        // 指定数分の残機が残っていない場合は
        // 指定数を残り残機数に合わせる
        if (matches.Count < count)
            count = matches.Count;

        for(int i = 0;i < count;++i)
        {
            matches[i].GetComponent<Image>().Disable();
        }
    }

    /// <summary>
    /// 指定した数、残機イメージを生成し、自分の子に追加する
    /// </summary>
    public void CreateRemainObject(int count)
    {
        for (int i = 0;i < count;++i)
        {
            GameObject obj = Instantiate(m_RemainImage);
            obj.transform.SetParent(transform, false);
            m_RemainList.Add(obj);
        }
    }

    /// <summary>
    /// 残機初期化
    /// </summary>
    /// <param name="count"></param>
    public void Initialize(int count)
    {
        ClearRemain();

        CreateRemainObject(count);
    }

    /// <summary>
    /// 残機をゼロにする
    /// </summary>
    public void ClearRemain()
    {
        // 子オブジェクトの残機オブジェクトを全て削除する
        gameObject.DestroyChildren();

        // 配列からも解放する
        m_RemainList.Clear();
    }
}
