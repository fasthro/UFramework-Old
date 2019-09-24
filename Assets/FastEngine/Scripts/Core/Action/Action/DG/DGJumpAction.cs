/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Jump Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGJumpAction : DGActionTransformBase
    {
        private float m_jumpPower;
        private int m_jumpNum;

        /// <summary>
        /// 沿Y轴应用跳跃效果
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="jumpPower">跳跃的力量（跳跃的最大高度由其加上最终的Y偏移量表示）</param>
        /// <param name="jumpNum">跳跃的次数</param>
        /// <param name="duration"></param>
        /// <param name="snapping"></param>      
        public DGJumpAction(Transform transform, Vector3 startValue, Vector3 endValue, float jumpPower, int jumpNum, float duration, bool snapping = false)
         : base(transform, startValue, endValue, duration, snapping)
        {
            m_jumpPower = jumpPower;
            m_jumpNum = jumpNum;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOJump(m_eV3, m_jumpPower, m_jumpNum, m_duration, m_snapping).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            transform.position = m_sV3;
        }
    }
}
