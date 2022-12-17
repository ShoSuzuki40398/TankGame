using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cinemachine;

/// <summary>
/// ���s���ʂɑ΂��鉉�o����
/// </summary>
public class ResultBehaviour : MonoBehaviour, IResult
{
    // ���ʒ�`
    [System.Serializable]
    public enum ResultType
    {
        None,
        PlayerWin,
        PlayerLose,
    }
    private ResultType m_ResultType;

    // �������ʕ���
    [SerializeField]
    private TextMeshProUGUI m_ResultText;

    // �������C�x���g���O
    public UnityEvent OnPrePlayerWin;

    // �������C�x���g
    [SerializeField]
    private UnityEvent OnPlayerWin;

    // �s�k���C�x���g���O
    public UnityEvent OnPrePlayerLose;

    // �s�k���C�x���g
    [SerializeField]
    private UnityEvent OnPlayerLose;

    // ��������SE
    [SerializeField]
    private AudioSource m_ResultSoundSource;

    // �������ʃJ����
    [SerializeField]
    private CinemachineVirtualCamera m_ResultCamera;


    /// <summary>
    /// ���U���g�������O����
    /// </summary>
    /// <param name="resultType"></param>
    public void ResultSetUp()
    {
        switch (m_ResultType)
        {
            case ResultType.None: break;
            case ResultType.PlayerWin:
                OnPrePlayerWin?.Invoke();
                break;
            case ResultType.PlayerLose:
                OnPrePlayerLose?.Invoke();
                break;
        }
    }

    /// <summary>
    /// ���U���g�������s
    /// </summary>
    public void ResultExecute()
    {
        switch (m_ResultType)
        {
            case ResultType.None: break;
            case ResultType.PlayerWin:
                OnPlayerWin?.Invoke();
                break;
            case ResultType.PlayerLose:
                OnPlayerLose?.Invoke();
                break;
        }
    }

    /// <summary>
    /// ���U���g�ݒ�
    /// </summary>
    public void SetResultType(ResultType type)
    {
        m_ResultType = type;
    }

    /// <summary>
    /// ���U���g�ݒ�
    /// </summary>
    public void SetResultType(int type)
    {
        m_ResultType = (ResultType)type;
    }

    /// <summary>
    /// ���U���g���ʂ�UI�e�L�X�g�ɔ��f����
    /// inspector����m_OnPlayerWin�Am_OnPlayerLose�ɐݒ肵��
    /// �\�����镶����inspector����ݒ肷��
    /// </summary>
    /// <param name="text"></param>
    public void SetResultText(string text)
    {
        m_ResultText.text = text;
    }

    /// <summary>
    /// ���U���g���ʂ��Đ�SE�ɔ��f����
    /// inspector����m_OnPlayerWin�Am_OnPlayerLose�ɐݒ肵��
    /// �Đ�����SE��inspector����ݒ肷��
    /// </summary>
    /// <param name="text"></param>
    public void SetResultSound(AudioClip audioClip)
    {
        m_ResultSoundSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// ���U���g���ʂ��J�����ɔ��f����
    /// inspector����m_OnPlayerWin�Am_OnPlayerLose�ɐݒ肵��
    /// �Ώۂɂ���^�[�Q�b�g��inspector����ݒ肷��
    /// </summary>
    public void SetResultCameraTraget(Transform target)
    {
        m_ResultCamera.LookAt = target;
        m_ResultCamera.Follow = target;
    }
}

// IResult--------------------------------------------------------------- //

public interface IResult
{
    void ResultSetUp();
    void ResultExecute();
}
