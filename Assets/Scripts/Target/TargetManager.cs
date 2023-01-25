using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    private TargetPool m_TargetPool;

    // 2�_�̍��W����l�p�`��̃G���A���`���A
    // �G���A���̃����_���Ȉʒu�Ƀ^�[�Q�b�g�𐶐�����
    [SerializeField]
    private Transform m_StartPoint;
    [SerializeField]
    private Transform m_EndPoint;

    [SerializeField]
    private LayerMask layerMask;

    public void TargetInitialize(AutomationTank automationTank)
    {
        TargetReset();

        for(int i = 0;i < m_TargetPool.poolingObjectList.Count;++i)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(m_StartPoint.localPosition.x, m_EndPoint.localPosition.x), UnityEngine.Random.Range(m_StartPoint.localPosition.y, m_EndPoint.localPosition.y), UnityEngine.Random.Range(m_StartPoint.localPosition.z, m_EndPoint.localPosition.z));

            // 10�񎎂�
            for (int n = 0; n < 10; n++)
            {
                Vector3 halfExtents = new Vector3(2.0f, 0.05f, 2.0f);
                // �d�Ȃ�Ȃ��Ƃ�
                if (!Physics.CheckBox(pos, halfExtents, Quaternion.identity, layerMask))
                {
                    var target = m_TargetPool.Pop();
                    target.target.Initialize(pos, Quaternion.identity);
                    break;
                }
                else
                {
                    pos = new Vector3(UnityEngine.Random.Range(m_StartPoint.localPosition.x, m_EndPoint.localPosition.x), UnityEngine.Random.Range(m_StartPoint.localPosition.y, m_EndPoint.localPosition.y), UnityEngine.Random.Range(m_StartPoint.localPosition.z, m_EndPoint.localPosition.z));
                }
            }
        }
    }

    /// <summary>
    /// �^�[�Q�b�g���v�[���ɖ߂�
    /// </summary>
    public void TargetReset()
    {
        for (int i = 0; i < m_TargetPool.poolingObjectList.Count; ++i)
        {
            m_TargetPool.poolingObjectList[i].ReturnToPool();
        }
    }

    /// <summary>
    /// �^�[�Q�b�g���S�Ĕ�A�N�e�B�u��
    /// </summary>
    /// <returns></returns>
    public bool IsAllTargetInPool()
    {
        return m_TargetPool.IsAllInPool();
    }
}
