/*
 * @Author: fasthro
 * @Date: 2019-10-23 13:20:56
 * @Description: tcp session service (处理会话消息注册和广播)
 */
using System;
using System.Collections.Generic;

namespace FastEngine.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pack"></param>
    public delegate void TCPSessionServiceEventCallabck(SocketPack pack);

    public class TCPSessionService
    {
        // event dic
        static Dictionary<int, TCPSessionServiceEventCallabck> m_eventDic = new Dictionary<int, TCPSessionServiceEventCallabck>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void AddListener(int cmd, TCPSessionServiceEventCallabck callback)
        {
            if (!m_eventDic.ContainsKey(cmd))
                m_eventDic.Add(cmd, null);

            m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] + callback;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="cmd"></param>
        public static void RemoveListener(int cmd)
        {
            if (m_eventDic.ContainsKey(cmd))
                m_eventDic[cmd] = null;
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(int cmd, TCPSessionServiceEventCallabck callback)
        {
            if (m_eventDic.ContainsKey(cmd))
                m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] - callback;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="pack"></param>
        public static void Broadcast(SocketPack pack)
        {
            TCPSessionServiceEventCallabck dc = null;
            if (m_eventDic.TryGetValue(pack.cmd, out dc))
            {
                var callback = dc as TCPSessionServiceEventCallabck;
                if (callback != null) callback.InvokeGracefully(pack);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Clear()
        {
            foreach (KeyValuePair<int, TCPSessionServiceEventCallabck> item in m_eventDic)
            {
                m_eventDic[item.Key] = null;
            }
            m_eventDic.Clear();
        }
    }
}