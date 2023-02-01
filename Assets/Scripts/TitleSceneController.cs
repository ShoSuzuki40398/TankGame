using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// タイトルシーン制御
/// </summary>
public sealed class TitleSceneController : MonoBehaviour
{
    /* References */
    [SerializeField]
    private PlayerInput m_PlayerInput;
    [SerializeField]
    private SelectableObjectGroup m_MainManuGroup;

    /* InputAction */
    private readonly string m_NavigateActionKey = "Navigate";
    private InputAction navigateAction { get { return m_PlayerInput.currentActionMap[m_NavigateActionKey]; } }
    private readonly string m_EnterActionKey = "Enter";
    private InputAction enterAction { get { return m_PlayerInput.currentActionMap[m_EnterActionKey]; } }

    private void Start()
    {
        // インゲーム情報リセット
        IngameSetting.Instance.ResetSetting();

        // メインメニュー選択可にする
        m_MainManuGroup.Enable();
    }

    /// <summary>
    /// メインシーンに遷移する
    /// </summary>
    /// <param name="type"></param>
    public void TransitionMainScene()
    {
        SceneNavigator.Instance.TransitionOnlyFadeOut(CommonDefineData.SceneNameMain);
        //FadeController.Instance.FadeOut(() => {
        //    // メインシーンに遷移する
        //    SceneTransitioner.Instance.TransitionToScene(CommonDefineData.SceneNameMain);
        //});
    }

    /// <summary>
    /// 指定したレベルアートの情報をIngameSettingに設定する
    /// </summary>
    /// <param name="type"></param>
    public void SetLevelArt(int type)
    {
        // レベルアート情報を設定
        IngameSetting.Instance.SetCurrentLevelArtProperty((LevelArtLoader.LEVEL_ART_TYPE)type);
    }

    private void Update()
    {
        UpdateStartManu();
    }

    /// <summary>
    /// スタートメニュー更新
    /// </summary>
    private void UpdateStartManu()
    {
        // 選択
        if (navigateAction.WasPressedThisFrame())
        {
            var navigateValue = navigateAction.ReadValue<Vector2>();
            m_MainManuGroup.SelectElementWithValue((int)Mathf.Round(navigateValue.x));
        }

        // 決定
        if (enterAction.WasPressedThisFrame())
        {
            m_MainManuGroup.Disable();
            m_MainManuGroup.DecideElement();
        }
    }
}
