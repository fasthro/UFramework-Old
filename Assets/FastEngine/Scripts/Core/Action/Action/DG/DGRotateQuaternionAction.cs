/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Rotate Quaternion Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGRotateQuaternionAction : DGActionTransformBase
    {
        private Quaternion m_startValue;
        private Quaternion m_endValue;

        public DGRotateQuaternionAction(Transform transform, Quaternion startValue, Quaternion endValue, float duration, Ease ease = Ease.Linear)
         : base(transform, duration, ease)
        {
            m_startValue = startValue;
            m_endValue = endValue;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DORotateQuaternion(m_endValue, m_duration).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnPlayRestore()
        {
            transform.rotation = m_startValue;
        }
    }
}
