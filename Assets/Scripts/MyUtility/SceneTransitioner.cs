using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// �V�[���Ԃ̑J�ڐ���
/// </summary>
public class SceneTransitioner : SingletonMonoBehaviour<SceneTransitioner>
{
    /// <summary>
    /// �ʃV�[���ɑJ��
    /// �C���X�y�N�^�[����Ăяo���p�̊֐�
    /// </summary>
    public void TransitionToScene(string nextSceneName)
    {
        Transition(nextSceneName);
    }

    /// <summary>
    /// �ʃV�[���ɑJ��
    /// </summary>
    public void TransitionToScene(string nextSceneName, Action action = null)
    {
        Transition(nextSceneName);
    }

    /// <summary>
    /// �J�ڊJ�n
    /// </summary>
    /// <param name="nextSceneName"></param>
    /// <param name="action"></param>
    private void Transition(string nextSceneName, Action action = null)
    {
        //FadeController.Instance.FadeOut(()=>StartCoroutine(LoadSceneAsync(nextSceneName)));
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

        //FadeController.Instance.FadeIn();
    }
}
