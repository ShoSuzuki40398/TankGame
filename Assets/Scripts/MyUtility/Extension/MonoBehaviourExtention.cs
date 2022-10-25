using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public static class MonoBehaviourExtention
{
    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="delayFrameCount"></param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private static IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }

    /// <summary>
    /// Actionの遅延実行
    /// </summary>
    /// <param name="self"></param>
    /// <param name="waitTime">遅延秒数</param>
    /// <param name="action">実行イベント</param>
    public static void Delay(this MonoBehaviour self, float waitTime, Action action)
    {
        self.StartCoroutine(DelayMethod(waitTime,action));
    }

    /// <summary>
    /// Actionの遅延実行
    /// </summary>
    /// <param name="self"></param>
    /// <param name="delayFrameCount">遅延フレーム数</param>
    /// <param name="action">実行イベント</param>
    public static void Delay(this MonoBehaviour self, int delayFrameCount, Action action)
    {
        self.StartCoroutine(DelayMethod(delayFrameCount, action));
    }

    /// <summary>
    /// 即時イベントをすべて実行する
    /// </summary>
    /// <param name="self"></param>
    public static void AllEventFire(this ICollection<UnityEngine.Events.UnityAction> self)
    {
        foreach (var act in self)
        {
            act();
        }
    }

    /// <summary>
    /// コンポーネントを有効にする
    /// </summary>
    /// <param name="self"></param>
    public static void Enable(this MonoBehaviour self)
    {
        self.enabled = true;
    }

    /// <summary>
    /// コンポーネントを無効にする
    /// </summary>
    /// <param name="self"></param>
    public static void Disable(this MonoBehaviour self)
    {
        self.enabled = false;
    }
}
