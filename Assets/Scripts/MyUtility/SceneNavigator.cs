using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ScreenFader))]
public class SceneNavigator : SingletonMonoBehaviour<SceneNavigator>
{
    private ScreenFader m_ScreenFader;

    // �J�ڎ��̃C�x���g
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
    /// �J�ڊJ�n
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void Transition(string nextSceneName)
    {
        Action transAction = () => StartCoroutine(LoadSceneAsync(nextSceneName));

        // �t�F�[�h�A�E�g�J�n
        // �������ɑJ�ڂ��J�n
        m_ScreenFader.FadeOut(transAction);
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�ƃV�[���J�ڂ�������
    /// �t�F�[�h�C���̃^�C�~���O�͑J�ڐ�̃V�[���ɔC�������Ƃ��Ɏg��
    /// </summary>
    /// <param name="onComplete"></param>
    public void TransitionOnlyFadeOut(string nextSceneName, Action onComplete = null)
    {
        Action transAction = () => StartCoroutine(ExecuteTransitionOnlyFadeOut(nextSceneName,onComplete));

        // �t�F�[�h�A�E�g�J�n
        // �������ɑJ�ڂ��J�n
        m_ScreenFader.FadeOut(transAction);
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�ƃV�[���J�ڂ�������
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
    /// �t�F�[�h�C����������
    /// </summary>
    /// <param name="onComplete"></param>
    public void FadeIn(Action onComplete = null)
    {
        m_ScreenFader.FadeIn(onComplete);
    }

    /// <summary>
    /// �V�[���̃��[�h
    /// �񓯊��Ń��[�h���邯�ǌ���Ӗ��͂��܂�Ȃ�
    /// UniTask�ɕύX������
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
