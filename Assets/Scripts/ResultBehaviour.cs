using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cinemachine;

/// <summary>
/// 勝敗結果に対する演出制御
/// </summary>
public class ResultBehaviour : MonoBehaviour, IResult
{
    // 結果定義
    [System.Serializable]
    public enum ResultType
    {
        None,
        PlayerWin,
        PlayerLose,
    }
    private ResultType m_ResultType;

    // 試合結果文言
    [SerializeField]
    private TextMeshProUGUI m_ResultText;

    // 勝利時イベント直前
    public UnityEvent OnPrePlayerWin;

    // 勝利時イベント
    [SerializeField]
    private UnityEvent OnPlayerWin;

    // 敗北時イベント直前
    public UnityEvent OnPrePlayerLose;

    // 敗北時イベント
    [SerializeField]
    private UnityEvent OnPlayerLose;

    // 試合結果SE
    [SerializeField]
    private AudioSource m_ResultSoundSource;

    // 試合結果カメラ
    [SerializeField]
    private CinemachineVirtualCamera m_ResultCamera;


    /// <summary>
    /// リザルト処理事前準備
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
    /// リザルト処理実行
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
    /// リザルト設定
    /// </summary>
    public void SetResultType(ResultType type)
    {
        m_ResultType = type;
    }

    /// <summary>
    /// リザルト設定
    /// </summary>
    public void SetResultType(int type)
    {
        m_ResultType = (ResultType)type;
    }

    /// <summary>
    /// リザルト結果をUIテキストに反映する
    /// inspectorからm_OnPlayerWin、m_OnPlayerLoseに設定して
    /// 表示する文言もinspectorから設定する
    /// </summary>
    /// <param name="text"></param>
    public void SetResultText(string text)
    {
        m_ResultText.text = text;
    }

    /// <summary>
    /// リザルト結果を再生SEに反映する
    /// inspectorからm_OnPlayerWin、m_OnPlayerLoseに設定して
    /// 再生するSEもinspectorから設定する
    /// </summary>
    /// <param name="text"></param>
    public void SetResultSound(AudioClip audioClip)
    {
        m_ResultSoundSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// リザルト結果をカメラに反映する
    /// inspectorからm_OnPlayerWin、m_OnPlayerLoseに設定して
    /// 対象にするターゲットもinspectorから設定する
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
