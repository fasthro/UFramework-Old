/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Punch Rotate Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGPunchRotateAction : DGActionTransformBase
    {
        private Vector3 m_punch;
        private int m_vibrato;
        private float m_elasticity;

        /// <summary>
        /// localRotation朝着给定值打孔，然后返回到起始值，就好像它已通过弹性线连接到起始旋转一样。
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="punch">强度</param>
        /// <param name="duration"></param>
        /// <param name="vibrato">振动次数</param>
        /// <param name="elasticity">表示向后弹跳时向量将超出起始旋转量的范围（0到1）。1在打孔器旋转和反向旋转之间产生完全振荡，而0仅在打孔器和开始旋转之间振荡。</param>
        public DGPunchRotateAction(Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
         : base(transform, duration, false)
        {
            m_punch = punch;
            m_vibrato = vibrato;
            m_elasticity = elasticity;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOPunchRotation(m_punch, m_duration, m_vibrato, m_elasticity).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }
    }
}
