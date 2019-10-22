/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:09:28
 * @Description: Action System
 */
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("Action/System")]
    public class ActionSystem : MonoSingleton<ActionSystem>
    {
        // actions
        private List<IAction> m_actions = new List<IAction>();

        // remove action index
        private List<int> m_removes = new List<int>();

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(IAction action)
        {
            Instance.m_actions.Add(action);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="action"></param>
        public static void Remove(IAction action)
        {
            for (int i = 0; i < Instance.m_actions.Count; i++)
            {
                if (Instance.m_actions[i] == action)
                {
                    action.Dispose();
                    Instance.m_actions.RemoveAt(i);
                    break;
                }
            }
        }

        private void Update()
        {
            m_removes.Clear();
            for (int i = 0; i < m_actions.Count; i++)
            {
                var action = m_actions[i];
                if (action.Execute(Time.deltaTime))
                {
                    m_removes.Add(i);
                }
            }

            for (int i = m_removes.Count - 1; i >= 0; i--)
            {
                var index = m_removes[i];
                // m_actions[index].Dispose();
                m_actions.RemoveAt(index);
            }
        }
    }
}
