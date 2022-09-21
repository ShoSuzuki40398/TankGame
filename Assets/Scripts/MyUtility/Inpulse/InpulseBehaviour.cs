using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpulseBehaviour : MonoBehaviour
{
    /// <summary>
    /// �Ռ����
    /// </summary>
    [System.Serializable]
    public class InpulseProperty
    {
        // ��]�p�x
        [SerializeField]
        private Vector3 RotationAngle;
        // ��]���x
        [SerializeField]
        private float m_RotateSpeed = 10.0f;
        public Vector3 RatoteVelocity { get { return RotationAngle * m_RotateSpeed; } }

        // ������ԕ���
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityX;
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityY;
        [SerializeField]
        private MinMaxRange m_BetweenInpuluseVelocityZ;
        public Vector3 CurrentInpuluseVelocity { get; private set; }

        // ������ԗ�
        [SerializeField]
        private float m_InpulsePower = 5.0f;

        // ������ԗ͌�����
        [SerializeField]
        private AnimationCurve m_PowerCurveX;
        [SerializeField]
        private AnimationCurve m_PowerCurveY;
        [SerializeField]
        private AnimationCurve m_PowerCurveZ;

        // �����o�ߎ���
        private float m_ProgressExplodeTime = 0.0f;

        // �������[�V�����̍Đ����x(1.0�����{)
        [SerializeField, Range(0.1f, 2.0f)]
        public float m_SpeedRatio = 1.0f;

        /// <summary>
        /// �R���X�g���N�^
        /// �Ռ������͐������Ɍ��߂Ă���
        /// </summary>
        public void Initialize()
        {
            CurrentInpuluseVelocity = RandomInpulseVelocity();
        }

        /// <summary>
        /// �����_���ɏՌ����������߂�
        /// </summary>
        public Vector3 RandomInpulseVelocity()
        {
            float x = Random.Range(m_BetweenInpuluseVelocityX.min, m_BetweenInpuluseVelocityX.max);
            float y = Random.Range(m_BetweenInpuluseVelocityY.min, m_BetweenInpuluseVelocityY.max);
            float z = Random.Range(m_BetweenInpuluseVelocityZ.min, m_BetweenInpuluseVelocityZ.max);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// �Ռ��ɂ��ړ��ʂ̍X�V
        /// �o�ߎ��Ԃƌ��������猻�݂̏Ռ��X�s�[�h��Ԃ�
        /// </summary>
        /// <returns></returns>
        public Vector3 UpdateInpluse()
        {
            Vector3 res = Vector3.zero;
            float x = CurrentInpuluseVelocity.x * m_PowerCurveX.Evaluate(m_ProgressExplodeTime);
            float y = CurrentInpuluseVelocity.y * m_PowerCurveY.Evaluate(m_ProgressExplodeTime);
            float z = CurrentInpuluseVelocity.z * m_PowerCurveZ.Evaluate(m_ProgressExplodeTime);

            res = new Vector3(x, y, z);

            // �Ռ����Ԍo��
            m_ProgressExplodeTime += Time.deltaTime;

            return res;
        }

        /// <summary>
        /// ���Z�b�g
        /// </summary>
        public void Reset()
        {
            m_ProgressExplodeTime = 0.0f;
            CurrentInpuluseVelocity = RandomInpulseVelocity();
        }
    }

    // �Ռ����
    [SerializeField]
    protected InpulseProperty m_InpulseProperty;
}
