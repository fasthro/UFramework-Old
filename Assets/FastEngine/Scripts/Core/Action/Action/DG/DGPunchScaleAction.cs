/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Punch Scale Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGPunchScaleAction : DGActionTransformBase
    {
        private Vector3 m_punch;
        private int m_vibrato;
        private float m_elasticity;

        /// <summary>
        /// localScale朝着给定的大小穿刺变形，然后回到起始的那个，就好像它是通过弹性件连接到起始的大小一样。
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="punch">强度</param>
        /// <param name="duration"></param>
        /// <param name="vibrato">振动次数</param>
        /// <param name="elasticity">表示向后弹跳时向量将超出起始大小的量（0到1）。1在打孔刻度和相对刻度之间产生完全振荡，而0仅在打孔刻度和起始刻度之间振荡</param>
        /// <param name="snapping"></param>
        public DGPunchScaleAction(Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1)
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
                m_tween = transform.DOPunchScale(m_punch, m_duration, m_vibrato, m_elasticity).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }
    }
}
