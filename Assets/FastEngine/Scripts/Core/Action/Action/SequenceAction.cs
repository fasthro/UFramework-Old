/*
 * @Author: fasthro
 * @Date: 2019-09-18 17:33:11
 * @Description: Sequence Action
 */

using System.Collections.Generic;

namespace FastEngine.Core
{
    public class SequenceAction : ActionBase
    {
        // Action list
        private List<IAction> m_actions = new List<IAction>();
        // Excute action list
        private List<IAction> m_excutes = new List<IAction>();

        // Action count
        public int count { get { return m_actions.Count; } }

        // 执行索引，如果索引等于 count 就证明已经完成
        public int executeIndex { get { return m_actions.Count - m_excutes.Count; } }

        // 执行 Action, 如果为null就证明已经完成没有在直接的Action
        public IAction executeAction { get { return m_excutes.Count > 0 ? m_excutes[0] : null; } }

        public SequenceAction(params IAction[] actions)
        {
            foreach (var action in actions)
            {
                m_actions.Add(action);
                m_excutes.Add(action);
            }
        }

        /// <summary>
        /// Append
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SequenceAction Append(IAction action)
        {
            m_actions.Add(action);
            m_excutes.Add(action);
            return this;
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_excutes.Count > 0)
            {
                var action = m_excutes[0];
                if (action.Execute(deltaTime))
                {
                    m_excutes.RemoveAt(0);
                }
            }

            isCompleted = m_excutes.Count == 0;
        }

        protected override void OnReset()
        {
            m_excutes.Clear();
            foreach (var action in m_actions)
            {
                action.Reset();
                m_excutes.Add(action);
            }
        }

        protected override void OnDispose()
        {
            m_actions.ForEach(action => Dispose());
            m_actions.Clear();
            m_excutes.Clear();
        }
    }
}