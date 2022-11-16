using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �c�@�\���r���[
/// �c�@�̕\����Ԃ�Image��enabled�ōs��
/// </summary>
public class RemainView : MonoBehaviour
{
    // �c�@�C���[�W�v���n�u
    // ���������ɏ����c�@�������̃v���n�u�𐶐�����
    [SerializeField]
    private GameObject m_RemainImage;

    // �c�@�z��
    private List<GameObject> m_RemainList = new List<GameObject>();

    /// <summary>
    /// �w�萔���A�c�@�C���[�W���A�N�e�B�u�ɂ���
    /// </summary>
    /// <param name="count"></param>
    public void SubRemain(int count)
    {
        var matches = m_RemainList.FindAll(remain =>
        {
            if (remain.GetComponent<Image>().enabled)
                return true;

            return false;
        });

        // �w�萔���̎c�@���c���Ă��Ȃ��ꍇ��
        // �w�萔���c��c�@���ɍ��킹��
        if (matches.Count < count)
            count = matches.Count;

        for(int i = 0;i < count;++i)
        {
            matches[i].GetComponent<Image>().Disable();
        }
    }

    /// <summary>
    /// �w�肵�����A�c�@�C���[�W�𐶐����A�����̎q�ɒǉ�����
    /// </summary>
    public void CreateRemainObject(int count)
    {
        for (int i = 0;i < count;++i)
        {
            GameObject obj = Instantiate(m_RemainImage);
            obj.transform.SetParent(transform, false);
            m_RemainList.Add(obj);
        }
    }

    /// <summary>
    /// �c�@������
    /// </summary>
    /// <param name="count"></param>
    public void Initialize(int count)
    {
        ClearRemain();

        CreateRemainObject(count);
    }

    /// <summary>
    /// �c�@���[���ɂ���
    /// </summary>
    public void ClearRemain()
    {
        // �q�I�u�W�F�N�g�̎c�@�I�u�W�F�N�g��S�č폜����
        gameObject.DestroyChildren();

        // �z�񂩂���������
        m_RemainList.Clear();
    }
}
