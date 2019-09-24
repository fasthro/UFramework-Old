/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Scale X Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGScaleXAction : DGActionTransformBase
    {
        public DGScaleXAction(Transform transform, float startValue, float endValue, float duration)
         : base(transform, startValue, endValue, duration, false)
        {
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOScaleX(m_eVF, m_duration).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            // TODO
        }
    }
}
