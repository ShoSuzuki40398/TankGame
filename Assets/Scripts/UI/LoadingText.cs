using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// ���[�f�B���O�i�s�e�L�X�g�\��
/// </summary>
public class LoadingText : MonoBehaviour
{
    // �e�L�����o�X
    [SerializeField]
    private Canvas m_Canvas = null;
    private TextMeshProUGUI m_TextMesh;

    private string loadingStr = "Loading";
    private const float textUpdateInterval = 0.7f;
    private const int maxEllipseCount = 4;

    public bool IsUpdate = false;

    /// <summary>
    /// ������
    /// </summary>
    public void Init(Transform parent = null)
    {
        CreateCanvasIfNeeded();
        CreateTextIfNeeded();

        if (parent != null)
            m_Canvas.transform.SetParent(parent);
    }
    /// <summary>
    /// �t�F�[�h�p�L�����o�X�쐬
    /// </summary>
    private void CreateCanvasIfNeeded()
    {
        if (m_Canvas != null)
            return;

        // �����ō쐬����ꍇ�͍Œ���̏����ɂȂ�܂�
        var obj = new GameObject("LoadingCanvas");
        var canvas = obj.AddComponent<Canvas>();
        // �O�ʂɗ���悤�Ƀ\�[�g�I�[�_�[��傫�Ȓl�ɂ���
        canvas.sortingOrder = 1000;
        // �����_�[���[�h��ScreenSpaceOverlay�ɂ���
        // �t�F�[�h�����̓J�����ł͂Ȃ��Q�[���̃V�X�e���Ƃ��ĉ^�p���邽��
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;

        var scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // ���t�F�[�h�����Ƀ��[�����g���ꍇ��inspector����ύX�ł��������ǂ�����
        scaler.referenceResolution = new Vector2(800, 600);

        m_Canvas = canvas;
    }

    /// <summary>
    /// �t�F�[�h�p�p�l���쐬
    /// </summary>
    private void CreateTextIfNeeded()
    {
        if (m_TextMesh != null)
            return;

        // �C���X�^���X�쐬
        var obj = new GameObject("LodingText");
        var rect = obj.AddComponent<RectTransform>();
        m_TextMesh = obj.AddComponent<TextMeshProUGUI>();

        m_TextMesh.verticalAlignment = VerticalAlignmentOptions.Middle;

        // �^�b�`���o���Ȃ�
        m_TextMesh.raycastTarget = false;

        // �e�q�ݒ�
        // �p�l�������V�[���ɑ��݂��ăL�����o�X�͎��������̎��͑z�肵�Ă��Ȃ��̂Œ���
        rect.SetParent(m_Canvas.transform);

        // ���W
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = Vector2.zero;

        // �T�C�Y
        rect.sizeDelta = new Vector2(200, 50);
    }

    /// <summary>
    /// ���[�f�B���O�e�L�X�g�X�V
    /// </summary>
    /// <returns></returns>
    private async UniTask UpdateEllipse()
    {
        int count = 0;
        while (IsUpdate)
        {
            string elipseStr = new string('.', count);

            m_TextMesh.text = loadingStr + elipseStr;

            count++;
            count = Mathf.Clamp(count, 0, maxEllipseCount);
            if (count >= maxEllipseCount)
                count = 0;

            await UniTask.Delay(TimeSpan.FromSeconds(textUpdateInterval));
        }
    }

    /// <summary>
    /// �e�L�X�g�\��
    /// �\���Ɠ����Ƀe�L�X�g�̍X�V���J�n����
    /// �X�V���I�����鎞�͉���HideText���Ă�
    /// </summary>
    public async void DisplayText()
    {
        if (IsUpdate)
            return;

        IsUpdate = true;
        gameObject.Enable();
        await UpdateEllipse();
    }

    /// <summary>
    /// �e�L�X�g��\��
    /// </summary>
    public void HideText()
    {
        if (!IsUpdate)
            return;

        IsUpdate = false;
        gameObject.Disable();
    }
}
