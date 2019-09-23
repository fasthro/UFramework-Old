/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Scale Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGScaleAction : DGActionTransformBase
    {
        private Vector3 m_startValue;
        private Vector3 m_endValue;
        
        public DGScaleAction(Transform transform, Vector3 startValue, Vector3 endValue, float duration, Ease ease = Ease.Linear)
         : base(transform, duration, ease)
        {
            m_startValue = startValue;
            m_endValue = endValue;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOScale(m_endValue, m_duration).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnPlayRestore()
        {
            transform.localScale = m_startValue;
        }
    }
}
