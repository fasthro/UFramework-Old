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

        /// <summary>
        /// 播放之前还原数据
        /// </summary>
        protected virtual void OnPlayRestore() { }

        protected override void OnExecute(float deltaTime)
        {
            if (m_tween.IsActive())
            {
                if (!m_tween.IsPlaying())
                {
                    OnPlayRestore();

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