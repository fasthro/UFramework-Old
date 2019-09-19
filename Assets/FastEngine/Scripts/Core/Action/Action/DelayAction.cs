/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:09:28
 * @Description: Delay Action
 */

using UnityEngine;

namespace FastEngine.Core
{
    public class DelayAction : ActionBase
    {
        // 延迟时间
        private float m_delayTime = 0;
        
        // 时间记录
        private float m_delay = 0;
        
        /// <summary>
        /// Delay Action
        /// </summary>
        /// <param name="time">需要延迟的时间(秒))</param>
        public DelayAction(float time)
        {
            m_delayTime = time;
        }

        protected override void OnExecute(float deltaTime)
        {
            m_delay += deltaTime;
            isCompleted = m_delay >= m_delayTime;
        }

        protected override void OnReset()
        {
            m_delay = 0;
        }

        protected override void OnDispose()
        {
            m_delay = 0;
        }
    }
}

