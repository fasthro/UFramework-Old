/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Punch Position Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGPunchPositionAction : DGActionTransformBase
    {
        private Vector3 m_punch;
        private int m_vibrato;
        private float m_elasticity;

        /// <summary>
        /// localPosition朝着给定的方向穿刺变形，然后回到起始位置，就好像它是通过弹性线连接到起始位置一样。
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="punch">方向和强度</param>
        /// <param name="duration"></param>
        /// <param name="vibrato">振动次数</param>
        /// <param name="elasticity">表示向后弹跳时向量将超出起始位置多少（0到1）。1在冲头方向和相反方向之间产生完全振荡，而0仅在冲头和开始位置之间振荡。</param>
        /// <param name="snapping"></param>
        public DGPunchPositionAction(Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false)
         : base(transform, duration, snapping)
        {
            m_punch = punch;
            m_vibrato = vibrato;
            m_elasticity = elasticity;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOPunchPosition(m_punch, m_duration, m_vibrato, m_elasticity, m_snapping).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }
    }
}
