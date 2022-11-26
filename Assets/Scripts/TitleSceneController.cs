using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
    }
    
    public void test()
    {
        Debug.Log("Select : " + m_MainManuGroup.currentSelectedElement.index);
    }

    public void decide()
    {
        Debug.Log("Decide : " + m_MainManuGroup.currentSelectedElement.index);
    }

    public void normal()
    {
        Debug.Log("normal : " + m_MainManuGroup.currentSelectedElement.index);
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
            m_MainManuGroup.DecideElement();
        }
    }
}
