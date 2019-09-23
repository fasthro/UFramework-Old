/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Local Rotate Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGLocalRotateAction : DGActionTransformBase
    {
        private Vector3 m_startValue;
        private Vector3 m_endValue;
        private RotateMode m_mode;

        public DGLocalRotateAction(Transform transform, Vector3 startValue, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast, Ease ease = Ease.Linear)
         : base(transform, duration, ease)
        {
            m_mode = mode;
            m_startValue = startValue;
            m_endValue = endValue;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOLocalRotate(m_endValue, m_duration, m_mode).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnPlayRestore()
        {
            transform.localEulerAngles = m_startValue;
        }
    }
}
