/*
 * @Author: fasthro
 * @Date: 2019-09-19 13:29:47
 * @Description: Repeat Action
 */
using UnityEngine;

namespace FastEngine.Core
{
    public class RepeatAction : ActionBase
    {
        // 重复次数
        private int m_repeatCount;

        // 当前重复剩余次数
        private int m_residueDegree;

        // 是否永久重复
        private bool m_loop;

        private IAction m_action;

        /// <summary>
        /// Repeat Action
        /// </summary>
        /// <param name="action">IAction</param>
        /// <param name="rc">重复次数，小于 1 为一直重复</param>
        public RepeatAction(IAction action, int repeatCount)
        {
            m_action = action;
            m_repeatCount = repeatCount;
            m_residueDegree = 0;
            m_loop = repeatCount < 1;
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_action.Execute(deltaTime))
            {
                m_residueDegree++;
                m_action.Reset();
                BroadcastCallback(ActionEvent.RepeatStepCompleted);
            }
            if (!m_loop)
            {
                isCompleted = m_residueDegree == m_repeatCount;
            }
        }

        protected override void OnReset()
        {
            m_action.Reset();
            m_residueDegree = 0;
        }

        protected override void OnDispose()
        {
            m_action.Dispose();
            m_action = null;
        }
    }
}