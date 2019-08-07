/*
 * @Author: fasthro
 * @Date: 2019-08-05 20:33:23
 * @Description: 事件消息
 */

using System;
using System.Collections.Generic;

namespace FastEngine.Core
{
    public static class EventMessager
    {
        // 事件字典
        private static Dictionary<int, Delegate> m_eventDic = new Dictionary<int, Delegate>();

        // 受保护的事件，保证不被清理
        private static List<int> m_prodectedEvents = new List<int>();

        // 清理的事件列表
        private static List<int> m_cleanEvents;

        /// <summary>
        /// 标记保护事件
        /// </summary>
        /// <param name="eventType"></param>
        public static void MarkAsProtected(int eventType)
        {
            if (!m_prodectedEvents.Contains(eventType))
                m_prodectedEvents.Add(eventType);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void AddListener(int eventType, EventCallback callback)
        {
            if (!m_eventDic.ContainsKey(eventType))
                m_eventDic.Add(eventType, null);

            m_eventDic[eventType] = (EventCallback)m_eventDic[eventType] + callback;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void AddListener<T>(int eventType, EventCallback<T> callback)
        {
            if (!m_eventDic.ContainsKey(eventType))
                m_eventDic.Add(eventType, null);

            m_eventDic[eventType] = (EventCallback<T>)m_eventDic[eventType] + callback;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        public static void RemoveListener(int eventType)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic.Remove(eventType);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(int eventType, EventCallback callback)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic[eventType] = (EventCallback)m_eventDic[eventType] - callback;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void RemoveListener<T>(int eventType, EventCallback<T> callback)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic[eventType] = (EventCallback<T>)m_eventDic[eventType] - callback;
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="eventType"></param>
        public static void Broadcast(int eventType)
        {
            Delegate dc = null;
            if (m_eventDic.TryGetValue(eventType, out dc))
            {
                var callback = dc as EventCallback;
                if (callback != null) callback();
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="arg"></param>
        public static void Broadcast<T>(int eventType, T arg)
        {
            Delegate dc = null;
            if (m_eventDic.TryGetValue(eventType, out dc))
            {
                var callback = dc as EventCallback<T>;
                if (callback != null) callback(arg);
            }
        }

        /// <summary>
        /// 清理事件
        /// </summary>
        public static void Clean()
        {
            if (m_cleanEvents == null)
                m_cleanEvents = new List<int>();
            m_cleanEvents.Clear();

            foreach (KeyValuePair<int, Delegate> item in m_eventDic)
            {
                var eventType = item.Key;
                if (!m_prodectedEvents.Contains(eventType))
                {
                    m_cleanEvents.Add(eventType);
                }
            }

            for (int i = 0; i < m_cleanEvents.Count; i++)
            {
                m_eventDic.Remove(m_cleanEvents[i]);
            }
        }
    }
}