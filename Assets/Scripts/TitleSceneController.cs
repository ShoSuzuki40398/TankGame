using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �^�C�g���V�[������
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
        // �C���Q�[����񃊃Z�b�g
        IngameSetting.Instance.ResetSetting();

        // ���C�����j���[�I���ɂ���
        m_MainManuGroup.Enable();
    }

    /// <summary>
    /// ���C���V�[���ɑJ�ڂ���
    /// </summary>
    /// <param name="type"></param>
    public void TransitionMainScene()
    {
        SceneNavigator.Instance.TransitionOnlyFadeOut(CommonDefineData.SceneNameMain);
        //FadeController.Instance.FadeOut(() => {
        //    // ���C���V�[���ɑJ�ڂ���
        //    SceneTransitioner.Instance.TransitionToScene(CommonDefineData.SceneNameMain);
        //});
    }

    /// <summary>
    /// �w�肵�����x���A�[�g�̏���IngameSetting�ɐݒ肷��
    /// </summary>
    /// <param name="type"></param>
    public void SetLevelArt(int type)
    {
        // ���x���A�[�g����ݒ�
        IngameSetting.Instance.SetCurrentLevelArtProperty((LevelArtLoader.LEVEL_ART_TYPE)type);
    }

    private void Update()
    {
        UpdateStartManu();
    }

    /// <summary>
    /// �X�^�[�g���j���[�X�V
    /// </summary>
    private void UpdateStartManu()
    {
        // �I��
        if (navigateAction.WasPressedThisFrame())
        {
            var navigateValue = navigateAction.ReadValue<Vector2>();
            m_MainManuGroup.SelectElementWithValue((int)Mathf.Round(navigateValue.x));
        }

        // ����
        if (enterAction.WasPressedThisFrame())
        {
            m_MainManuGroup.Disable();
            m_MainManuGroup.DecideElement();
        }
    }
}
