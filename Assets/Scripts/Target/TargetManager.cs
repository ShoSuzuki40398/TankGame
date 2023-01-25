using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    private TargetPool m_TargetPool;

    // 2点の座標から四角形状のエリアを定義し、
    // エリア内のランダムな位置にターゲットを生成する
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

            // 10回試す
            for (int n = 0; n < 10; n++)
            {
                Vector3 halfExtents = new Vector3(2.0f, 0.05f, 2.0f);
                // 重ならないとき
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
    /// ターゲットをプールに戻す
    /// </summary>
    public void TargetReset()
    {
        for (int i = 0; i < m_TargetPool.poolingObjectList.Count; ++i)
        {
            m_TargetPool.poolingObjectList[i].ReturnToPool();
        }
    }

    /// <summary>
    /// ターゲットが全て非アクティブか
    /// </summary>
    /// <returns></returns>
    public bool IsAllTargetInPool()
    {
        return m_TargetPool.IsAllInPool();
    }
}
