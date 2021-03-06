﻿/*
 * @Author: fasthro
 * @Date: 2019-09-21 16:25:16
 * @Description: Rotate Action (DoTween)
 */

using UnityEngine;
using DG.Tweening;

namespace FastEngine.Core
{
    public class DGRotateAction : DGActionTransformBase
    {
        private RotateMode m_mode;

        public DGRotateAction(Transform transform, Vector3 startValue, Vector3 endValue, float duration, RotateMode mode = RotateMode.Fast)
         : base(transform, startValue, endValue, duration, false)
        {
            m_mode = mode;
        }

        protected override void OnInitialize()
        {
            if (m_tween == null)
            {
                m_tween = transform.DORotate(m_eV3, m_duration, m_mode).SetEase(m_ease).SetAutoKill(false);
                m_tween.OnComplete(() =>
                {
                    isCompleted = true;
                });
            }
        }

        protected override void OnRestoreValue()
        {
            transform.eulerAngles = m_sV3;
        }
    }
}
