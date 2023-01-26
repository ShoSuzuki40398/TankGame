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
    // TODO:�����[�X���ɍ폜
    [SerializeField]
    float speed = 0.005f;

    // TODO:�����[�X���ɍ폜
    [SerializeField]
    private Transform m_Target;


    [SerializeField]
    private TankMovement m_TankMovement;

    [SerializeField]
    private TankShooting m_TankShooting;

    // �ړ��ʂ̓��͒l
    private Vector2 m_InputMoveVector = Vector2.zero;

    private PlayerInput m_PlayerInput;

    // ��Ԗ{��
    [SerializeField]
    private GameObject m_Tank;
    public GameObject Tank { get { return m_Tank; } }

    private void Awake()
    {
        m_PlayerInput = GameObject.FindGameObjectWithTag(CommonDefineData.ObjectNamePlayerInput).GetComponent<PlayerInput>();

        // Move���̓C�x���g��OnMove��ǉ�
        //m_PlayerInput.actionEvents[0].AddListener(OnMove);
        // Fire���̓C�x���g��OnFire��ǉ�
        //m_PlayerInput.actionEvents[1].AddListener(OnFire);
    }

    // TODO:�����[�X���ɍ폜
    private void Start()
    {
        StartCoroutine(AutoFire());
    }

    // TODO:�����[�X���ɍ폜
    private IEnumerator AutoFire()
    {
        while (true)
        {
            m_TankShooting.Fire();
            yield return new WaitForSeconds(1.5f);
        }

    }

    private void FixedUpdate()
    {
        // �ړ��Ɖ�]
        //Movement();

        // TODO:�����[�X���ɍ폜
        m_Tank.transform.rotation = Quaternion.Slerp(m_Tank.transform.rotation, Quaternion.LookRotation((m_Target.position - m_Tank.transform.position).normalized), speed * Time.deltaTime);
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

    private void OnDestroy()
    {
        // Move���̓C�x���g��OnMove��ǉ�
        //m_PlayerInput.actionEvents[0].RemoveAllListeners();
        // Fire���̓C�x���g��OnFire��ǉ�
        //m_PlayerInput.actionEvents[1].RemoveAllListeners();
    }
}
