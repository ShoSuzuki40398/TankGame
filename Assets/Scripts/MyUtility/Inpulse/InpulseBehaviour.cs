using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpulseBehaviour : MonoBehaviour
{
    /// <summary>
    /// 衝撃情報
    /// </summary>
    [System.Serializable]
    public class InpulseProperty
    {
        // 回転角度
        [SerializeField]
        private Vector3 RotationAngle;
        // 回転速度
        [SerializeField]
        private float m_RotateSpeed = 10.0f;
        public Vector3 RatoteVelocity { get { return RotationAngle * m_RotateSpeed; } }

        // 吹っ飛ぶ方向
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityX;
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityY;
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityZ;
        public Vector3 CurrentInpuluseVelocity { get; private set; }

        // 吹っ飛ぶ力
        [SerializeField]
        private float m_InpulsePower = 5.0f;

        // 吹っ飛ぶ力減衰率
        [SerializeField]
        private AnimationCurve m_PowerCurveX;
        [SerializeField]
        private AnimationCurve m_PowerCurveY;
        [SerializeField]
        private AnimationCurve m_PowerCurveZ;

        // 爆発経過時間
        private float m_ProgressExplodeTime = 0.0f;

        // 爆発モーションの再生速度(1.0が等倍)
        [SerializeField, Range(0.1f, 2.0f)]
        public float m_SpeedRatio = 1.0f;

        /// <summary>
        /// コンストラクタ
        /// 衝撃方向は生成時に決めておく
        /// </summary>
        public void Initialize()
        {
            CurrentInpuluseVelocity = RandomInpulseVelocity();
        }

        /// <summary>
        /// ランダムに衝撃方向を決める
        /// </summary>
        public Vector3 RandomInpulseVelocity()
        {
            float x = Random.Range(m_BetweenInpuluseVelocityX.min, m_BetweenInpuluseVelocityX.max);
            float y = Random.Range(m_BetweenInpuluseVelocityY.min, m_BetweenInpuluseVelocityY.max);
            float z = Random.Range(m_BetweenInpuluseVelocityZ.min, m_BetweenInpuluseVelocityZ.max);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 衝撃による移動量の更新
        /// 経過時間と減衰率から現在の衝撃スピードを返す
        /// </summary>
        /// <returns></returns>
        public Vector3 UpdateInpluse()
        {
            Vector3 res = Vector3.zero;
            float x = CurrentInpuluseVelocity.x * m_PowerCurveX.Evaluate(m_ProgressExplodeTime);
            float y = CurrentInpuluseVelocity.y * m_PowerCurveY.Evaluate(m_ProgressExplodeTime);
            float z = CurrentInpuluseVelocity.z * m_PowerCurveZ.Evaluate(m_ProgressExplodeTime);

            res = new Vector3(x, y, z);

            // 衝撃時間経過
            m_ProgressExplodeTime += Time.deltaTime;

            return res;
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset()
        {
            m_ProgressExplodeTime = 0.0f;
            CurrentInpuluseVelocity = RandomInpulseVelocity();
        }
    }

    // 衝撃情報
    [SerializeField]
    protected InpulseProperty m_InpulseProperty;
}
