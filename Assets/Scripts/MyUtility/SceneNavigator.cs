using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScreenFader))]
public class SceneNavigator : SingletonMonoBehaviour<SceneNavigator>
{
    private ScreenFader m_ScreenFader;

    // 遷移時のイベント
    public class SceneTransEvent
    {
        public Action OnFadeInComplete = null;
        public Action OnFadeOutComplete = null;
    }

    protected override void Init()
    {
        m_ScreenFader = gameObject.GetComponent<ScreenFader>();
        m_ScreenFader.Init(transform);
    }

    /// <summary>
    /// 遷移開始
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void Transition(string nextSceneName)
    {
        Action transAction = () => StartCoroutine(LoadSceneAsync(nextSceneName));

        // フェードアウト開始
        // 完了時に遷移を開始
        m_ScreenFader.FadeOut(transAction);
    }

    /// <summary>
    /// フェードアウトとシーン遷移だけする
    /// フェードインのタイミングは遷移先のシーンに任せたいときに使う
    /// </summary>
    /// <param name="onComplete"></param>
    public void TransitionOnlyFadeOut(string nextSceneName, Action onComplete = null)
    {
        Action transAction = () => StartCoroutine(ExecuteTransitionOnlyFadeOut(nextSceneName,onComplete));

        // フェードアウト開始
        // 完了時に遷移を開始
        m_ScreenFader.FadeOut(transAction);
    }

    /// <summary>
    /// フェードアウトとシーン遷移だけする
    /// </summary>
    /// <param name="onComplete"></param>
    private IEnumerator ExecuteTransitionOnlyFadeOut(string nextSceneName,Action onComplete = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    /// <summary>
    /// フェードインだけする
    /// </summary>
    /// <param name="onComplete"></param>
    public void FadeIn(Action onComplete = null)
    {
        m_ScreenFader.FadeIn(onComplete);
    }

    /// <summary>
    /// シーンのロード
    /// 非同期でロードするけど現状意味はあまりない
    /// UniTaskに変更したい
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync(string nextSceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        m_ScreenFader.FadeIn();
    }
}
