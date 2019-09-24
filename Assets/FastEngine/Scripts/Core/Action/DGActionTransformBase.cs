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
        protected bool m_snapping;

        protected Vector3 m_sV3;
        protected Vector3 m_eV3;

        protected Quaternion m_sQuaternion;
        protected Quaternion m_eQuaternion;

        protected float m_sVF;
        protected float m_eVF;

        #endregion
        
        public DGActionTransformBase(Transform transform, float duration, bool snapping = false)
        {
            InitValue(transform, duration, snapping);
        }

        public DGActionTransformBase(Transform transform, Vector3 startValue, Vector3 endValue, float duration, bool snapping = false)
        {
            InitValue(transform, duration, snapping);
            this.m_sV3 = startValue;
            this.m_eV3 = endValue;
        }

        public DGActionTransformBase(Transform transform, Quaternion startValue, Quaternion endValue, float duration, bool snapping = false)
        {
            InitValue(transform, duration, snapping);
            this.m_sQuaternion = startValue;
            this.m_eQuaternion = endValue;
        }

        public DGActionTransformBase(Transform transform, float startValue, float endValue, float duration, bool snapping = false)
        {
            InitValue(transform, duration, snapping);
            this.m_sVF = startValue;
            this.m_eVF = endValue;
        }

        private void InitValue(Transform transform, float duration, bool snapping)
        {
            this.transform = transform;
            this.m_duration = duration;
            this.m_snapping = snapping;
            this.m_ease = DOTween.defaultEaseType;
        }

        public DGActionTransformBase SetEase(Ease ease)
        {
            this.m_ease = ease;
            return this;
        }
    }
}