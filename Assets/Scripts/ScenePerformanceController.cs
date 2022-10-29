using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;

/// <summary>
/// Timeline�ɂ��V�[�����o����
/// Timeline�̊Ǘ����V�[�����o�^�C�v���L�[�ɂ��Ĕz��ŊǗ����A
/// �L�[�ɂ��w��ōĐ��Ȃǂ��s��
/// </summary>
public class ScenePerformanceController : MonoBehaviour
{
    // �V�[�����o�^�C�v
    public enum PerformanceType
    {
        Scene_Start,
        Battle_End,
        DEMO,
    }

    // �V�[�����o�e�[�u��
    [SerializeField]
    private ScenePerformanceTable m_ScenePerformanceTable;

    // Timeline�Đ�����
    [SerializeField]
    private PlayableDirector m_PlayableDirector;
    
    /// <summary>
    /// ���o�Đ�
    /// </summary>
    /// <param name="type">���o�^�C�v</param>
    /// <param name="complete">�I�����C�x���g</param>
    public void PlayOneShot(PerformanceType type,Action complete = null)
    {
        // �L�[�Ńe�[�u������擾���čĐ�����
        var performance = m_ScenePerformanceTable.GetPerformance(type);

        if (performance == null)
            return;
        
        m_PlayableDirector.Play(performance?.asset, DirectorWrapMode.Hold);

        // �I�����C�x���g����
        complete?.Invoke();
    }

}

/// <summary>
/// �V�[�����o�e�[�u��
/// ScenePerformanceProperty�̔z����Ǘ�����
/// �l�̎󂯓n�������邽�߂̃p�C�v���C���p�N���X
/// </summary>
[Serializable]
public class ScenePerformanceTable
{
    // �V�[�����o�z��
    // ���ꂪ�V�[�����o�̎��̂ɂȂ�
    public List<ScenePerformanceProperty> m_ScenePerformanceProperties = new List<ScenePerformanceProperty>();

    // �Ǘ����o���擾
    public int count { get { return m_ScenePerformanceProperties.Count; } }

    /// <summary>
    /// �V�[�����o�擾
    /// �z��ɂȂ��ꍇ��null�ŕԂ��̂ŌĂяo������null�`�F�b�N�͕K�����邱��
    /// </summary>
    public ScenePerformanceProperty? GetPerformance(ScenePerformanceController.PerformanceType type)
    {
        // �L�[�Ŏw�肵���^�C�v���������čŏ��Ɉ�v����value(TimelineAsset)���Đ�����
        var result = m_ScenePerformanceProperties.Find(item =>
        {
            if(type == item.type)
                return true;

            return false;
        });
                
        return result;
    }
}


/// <summary>
/// �V�[�����o�v���p�e�B
/// �ϐ��A�N�Z�T�[��public�ɂȂ��Ă���̂̓X�N���v�g����ύX����݌v�����z���Ă��܂�
/// �������A�C���X�y�N�^�[���玖�O�ɐݒ肷�邱�Ƃōς�ł�����͓���public�̗��_�͂���܂���B
/// �C���X�y�N�^�[���玖�O�ɐݒ肵�����Ȃ��ꍇ�ASerializeField������t����private�Ɏw�肷��
/// �݌v�ɕύX���Ă��悢
/// </summary>
[Serializable]
public struct ScenePerformanceProperty
{
    // �V�[�����o�L�[
    public ScenePerformanceController.PerformanceType type;

    // PlayableAsset�A�Z�b�g
    public PlayableAsset asset;
}