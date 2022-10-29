using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �}�j���A�����͂ɂ���Ԑ���
/// �v���C���[�����삷���ԂɃA�^�b�`����
/// </summary>
public class PlayableTank : MonoBehaviour,IPlayerInput
{
    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    // �ړ��ʂ̓��͒l
    private Vector2 m_InputMoveVector = Vector2.zero;

    private void Awake()
    {
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
    /// �ړ����͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        m_InputMoveVector = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ������͎��C�x���g
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
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
}
