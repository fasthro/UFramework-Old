/*
 * @Author: fasthro
 * @Date: 2019-09-24 15:36:30
 * @Description: Look At Action (DoTween)
 */
using DG.Tweening;
using UnityEngine;

namespace FastEngine.Core
{
    public class DGLookAtAction : DGActionTransformBase
    {
        private Vector3 m_towards;
        private AxisConstraint m_axisConstraint;
        private Vector3? m_up;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="towards">旋转朝向</param>
        /// <param name="duration">持续时间</param>
        /// <param name="axisConstraint">旋转轴约束</param>
        /// <param name="up">向上方向的向量</param>
        public DGLookAtAction(Transform transform, Vector3 towards, float duration, AxisConstraint axisConstraint = AxisConstraint.None, Vector3? up = null)
        : base(transform, duration, false)
        {
            m_towards = towards;
            m_axisConstraint = axisConstraint;
            m_up = up;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOLookAt(m_towards, m_duration, m_axisConstraint, m_up).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }
    }
}