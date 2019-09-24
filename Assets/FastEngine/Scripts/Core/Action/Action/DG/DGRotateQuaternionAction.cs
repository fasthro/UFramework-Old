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
        public DGRotateQuaternionAction(Transform transform, Quaternion startValue, Quaternion endValue, float duration)
         : base(transform, startValue, endValue, duration, false)
        {
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DORotateQuaternion(m_eQuaternion, m_duration).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            transform.rotation = m_sQuaternion;
        }
    }
}
