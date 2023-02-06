using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ��ʂ̃t�F�[�h�C���E�A�E�g�𐧌䂷��
/// �t�F�[�h�p�̃L�����o�X��Image�I�u�W�F�N�g���q�ɂ����đ��삷��
/// </summary>
public class ScreenFader : MonoBehaviour
{
    enum FADE_STATE
    {
        IDLE,   // �ҋ@
        FADEIN, // �t�F�[�h�C��
        FADEOUT, // �t�F�[�h�A�E�g
    }
    FADE_STATE state = FADE_STATE.IDLE;

    // �t�F�[�h�p�p�l��
    [SerializeField]
    protected Image m_FadePanel = null;

    // �e�L�����o�X
    [SerializeField]
    protected Canvas m_FadeCanvas = null;

    // �t�F�[�h�C���ɂ����鎞��
    [SerializeField]
    protected float m_FadeInTime = 1.0f;

    // �t�F�[�h�A�E�g�ɂ����鎞��
    [SerializeField]
    protected float m_FadeOutTime = 1.0f;

    /// <summary>
    /// ������
    /// </summary>
    public void Init(Transform parent = null)
    {
        CreateFadeCanvasIfNeeded();
        CreateFadePanelIfNeeded();

        if(parent != null)
            m_FadeCanvas.transform.SetParent(parent);
    }

    /// <summary>
    /// �t�F�[�h�p�L�����o�X�쐬
    /// </summary>
    private void CreateFadeCanvasIfNeeded()
    {
        if (m_FadeCanvas != null)
            return;

        // �����ō쐬����ꍇ�͍Œ���̏����ɂȂ�܂�
        var obj = new GameObject("FadeCanvas");
        var canvas = obj.AddComponent<Canvas>();
        // �O�ʂɗ���悤�Ƀ\�[�g�I�[�_�[��傫�Ȓl�ɂ���
        canvas.sortingOrder = 999;
        // �����_�[���[�h��ScreenSpaceOverlay�ɂ���
        // �t�F�[�h�����̓J�����ł͂Ȃ��Q�[���̃V�X�e���Ƃ��ĉ^�p���邽��
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

        var scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // ���t�F�[�h�����Ƀ��[�����g���ꍇ��inspector����ύX�ł��������ǂ�����
        scaler.referenceResolution = new Vector2(800, 600);

        m_FadeCanvas = canvas;
    }

    /// <summary>
    /// �t�F�[�h�p�p�l���쐬
    /// </summary>
    private void CreateFadePanelIfNeeded()
    {
        if (m_FadePanel != null)
            return;

        // �C���X�^���X�쐬
        var obj = new GameObject("FadePanel");
        var rect = obj.AddComponent<RectTransform>();

        // �F
        m_FadePanel = obj.AddComponent<Image>();
        m_FadePanel.color = Color.black;

        // �^�b�`���o���Ȃ�
        m_FadePanel.raycastTarget = false;

        // �e�q�ݒ�
        // �p�l�������V�[���ɑ��݂��ăL�����o�X�͎��������̎��͑z�肵�Ă��Ȃ��̂Œ���
        rect.SetParent(m_FadeCanvas.transform);

        // �T�C�Y
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        // ���W
        rect.anchoredPosition = Vector2.zero;

        SetAlpha(0.0f);
    }

    /// <summary>
    /// �t�F�[�h
    /// </summary>
    /// <param name="fadeTime">�t�F�[�h�ɂ����鎞��</param>
    /// <param name="startAlpha">�J�n�A���t�@�l</param>
    /// <param name="endAlpha">�I���A���t�@�l</param>
    /// <param name="action">�t�F�[�h�I�����C�x���g</param>
    /// <returns></returns>
    private IEnumerator Fade(float fadeTime, float startAlpha, float endAlpha, Action action = null)
    {
        m_FadePanel.gameObject.SetActive(true);

        float elapsedTime = 0.0f;

        // �t�F�[�h�J�n
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            var currentAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            SetAlpha(currentAlpha);
            yield return new WaitForEndOfFrame();
        }

        // �t�F�[�h��̃C�x���g����
        action?.Invoke();

        // �t�F�[�h�C���̏ꍇ�͔�A�N�e�B�u�ɂ���
        // �}�E�X���W���Ń��C�L���X�g���s���ꍇ�A�t�F�[�h�p�l�����O�ʂɂ���Ǝז��ɂȂ邽��
        if (state == FADE_STATE.FADEIN)
        {
            m_FadePanel.gameObject.SetActive(false);
        }

        // �ҋ@��Ԃɖ߂��Ă���
        state = FADE_STATE.IDLE;
    }

    /// <summary>
    /// �A���t�@�l�ݒ�
    /// </summary>
    /// <param name="a"></param>
    public void SetAlpha(float a)
    {
        var color = m_FadePanel.color;
        color.a = a;
        m_FadePanel.color = color;
    }

    /// <summary>
    /// �t�F�[�h�C���J�n
    /// </summary>
    /// <param name="time"></param>
    public void FadeIn(Action action = null)
    {
        // �t�F�[�h���͖���
        if (state != FADE_STATE.IDLE)
        {
            return;
        }

        state = FADE_STATE.FADEIN;

        StartCoroutine(Fade(m_FadeInTime, 1.0f, 0.0f, action));
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�J�n
    /// </summary>
    /// <param name="time"></param>
    public void FadeOut(Action action = null)
    {
        // �t�F�[�h���͖���
        if (state != FADE_STATE.IDLE)
        {
            return;
        }
        
        state = FADE_STATE.FADEOUT;

        StartCoroutine(Fade(m_FadeOutTime, 0.0f, 1.0f, action));
    }
}
