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
        public DGScaleAction(Transform transform, Vector3 startValue, Vector3 endValue, float duration)
         : base(transform, startValue, endValue, duration, false)
        {
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DOScale(m_eV3, m_duration).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            transform.localScale = m_sV3;
        }
    }
}
