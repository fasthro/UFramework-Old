/*
 * @Author: fasthro
 * @Date: 2019-09-23 13:56:11
 * @Description: DoTween Action Base (对DoTween的Action包装)
 */
using DG.Tweening;
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class DGActionBase : ActionBase
    {
        // tween
        protected Tween m_tween;
        public Tween tween { get { return m_tween; } }

        // restore
        protected bool m_isRV = true;

        /// <summary>
        /// Set Restore Value
        /// </summary>
        /// <param name="irv"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SetRestoreValue<T>(bool irv) where T : DGActionBase
        {
            m_isRV = irv;
            return this as T;
        }

        /// <summary>
        /// Set Restore Value
        /// </summary>
        /// <param name="irv"></param>
        /// <returns>DGActionTransformBase</returns>
        public DGActionTransformBase SetRestoreValue(bool irv)
        {
            return SetRestoreValue<DGActionTransformBase>(irv);
        }

        /// <summary>
        /// Restore Value
        /// </summary>
        protected virtual void OnRestoreValue() { }

        protected override void OnExecute(float deltaTime)
        {
            if (m_tween.IsActive())
            {
                if (!m_tween.IsPlaying())
                {
                    if (m_isRV) OnRestoreValue();
                    if (isReseted) m_tween.Restart();
                    else m_tween.Play();
                }
            }
        }

        protected override void OnDispose()
        {
            if (!m_tween.IsActive())
                m_tween.Kill();
            m_tween = null;
        }
    }
}