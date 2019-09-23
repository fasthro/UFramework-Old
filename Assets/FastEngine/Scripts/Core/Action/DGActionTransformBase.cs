/*
 * @Author: fasthro
 * @Date: 2019-09-23 14:05:03
 * @Description: DoTween Action Transform Base
 */
using DG.Tweening;
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class DGActionTransformBase : DGActionBase
    {
        #region param
        public Transform transform { get; protected set; }
        protected float m_duration;
        protected Ease m_ease;
        #endregion

        public DGActionTransformBase(Transform transform, float duration, Ease ease = Ease.Linear)
        {
            this.transform = transform;
            this.m_duration = duration;
            this.m_ease = ease;
        }
    }
}