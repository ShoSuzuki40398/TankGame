using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// シーン間の遷移制御
/// </summary>
public class SceneTransitioner : SingletonMonoBehaviour<SceneTransitioner>
{
    // フェード処理ONOFF
    [SerializeField]
    private bool isFade = false;

    /// <summary>
    /// 別シーンに遷移
    /// インスペクターから呼び出す用の関数
    /// </summary>
    public void TransitionToScene(string nextSceneName)
    {
        Transition(nextSceneName);
    }

    /// <summary>
    /// 別シーンに遷移
    /// </summary>
    public void TransitionToScene(string nextSceneName, Action action = null)
    {
        Transition(nextSceneName,action);
    }

    /// <summary>
    /// 遷移開始
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <param name="action"></param>
    private void Transition(string nextSceneName, Action action = null)
    {
        Action transAction = () => StartCoroutine(LoadSceneAsync(nextSceneName));

        if (isFade)
            FadeController.Instance.FadeOut(transAction);
        else
            transAction();
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

        if(isFade)
            FadeController.Instance.FadeIn();
    }
}
