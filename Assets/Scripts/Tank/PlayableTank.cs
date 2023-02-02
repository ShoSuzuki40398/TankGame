using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �}�j���A�����͂ɂ���Ԑ���
/// �v���C���[�����삷���ԂɃA�^�b�`����
/// </summary>
public class PlayableTank : MonoBehaviour, IPlayerInput
{
    private TankMovement m_TankMovement;
    private TankShooting m_TankShooting;
    private PlayerInput m_PlayerInput;

    // �ړ��ʂ̓��͒l
    private Vector2 m_InputMoveVector = Vector2.zero;
    
    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    // ���t���O
    private bool m_IsMovement = true;

    private void Awake()
    {
        m_TankMovement = GetComponentInChildren<TankMovement>();
        m_TankShooting = GetComponentInChildren<TankShooting>();
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();

        // Move���̓C�x���g��OnMove��ǉ�
        m_PlayerInput.actionEvents[0].AddListener(OnMove);
        // Fire���̓C�x���g��OnFire��ǉ�
        m_PlayerInput.actionEvents[1].AddListener(OnFire);
    }
    
    private void FixedUpdate()
    {
        // �ړ��Ɖ�]
        Movement();
    }

    /// <summary>
    /// ���͒l�����ƂɈړ��Ɖ�]���s��
    /// </summary>
    private void Movement()
    {
        m_TankMovement.Move(m_InputMoveVector.y);
        m_TankMovement.Turn(m_InputMoveVector.x);
    }

    /// <summary>
    /// ���t���O�ݒ�
    /// </summary>
    public void SetIsMovement(bool flag)
    {
        m_IsMovement = flag;
    }

    /// <summary>
    /// �ړ����͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!m_IsMovement)
        {
            m_InputMoveVector = Vector2.zero;
            return;
        }

        m_InputMoveVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ������͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!m_IsMovement)
            return;

        // �������u�Ԃ����L��
        // Was����ThisFrame���Ȃ��Ƒ���phase���E����1�x�ɕ�����Ă΂��̂Œ��ӂ���
        // started
        // performed �� �����Ŏˌ����͂Ƃ���
        // release
        if (context.action.WasPerformedThisFrame())
        {
            m_TankShooting.Fire();
        }
    }

    private void OnDestroy()
    {
        // Move���̓C�x���g��OnMove��ǉ�
        m_PlayerInput.actionEvents[0].RemoveAllListeners();
        // Fire���̓C�x���g��OnFire��ǉ�
        m_PlayerInput.actionEvents[1].RemoveAllListeners();
    }
}
