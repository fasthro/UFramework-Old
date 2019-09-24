/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Local Jump Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGLocalJumpAction : DGActionTransformBase
    {
        private float m_jumpPower;
        private int m_jumpNum;
        
        public DGLocalJumpAction(Transform transform, Vector3 startValue, Vector3 endValue, float jumpPower, int jumpNum, float duration, bool snapping = false)
         : base(transform, startValue, endValue, duration, snapping)
        {
            m_jumpPower = jumpPower;
            m_jumpNum = jumpNum;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOLocalJump(m_eV3, m_jumpPower, m_jumpNum, m_duration, m_snapping).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            transform.localPosition = m_sV3;
        }
    }
}
