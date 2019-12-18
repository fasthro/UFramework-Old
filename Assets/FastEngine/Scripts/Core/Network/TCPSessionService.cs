/*
 * @Author: fasthro
 * @Date: 2019-10-23 13:20:56
 * @Description: tcp session service (处理会话消息注册和广播)
 */
using System;
using System.Collections.Generic;
using LuaInterface;

namespace FastEngine.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pack"></param>
    public delegate void TCPSessionServiceEventCallabck(SocketPack pack);

    /// <summary>
    /// 
    /// </summary>
    public delegate void TCPSessionServiceBuiltInEventCallabck();

    /// <summary>
    /// 内置服务
    /// </summary>
    public enum TCPSessionServiceBuiltIn
    {
        Connected = -1,
        Disconnected = -2,
    }

    public class TCPSessionService
    {
        // event dic
        static Dictionary<int, Delegate> m_eventDic = new Dictionary<int, Delegate>();

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void AddListener(int cmd, TCPSessionServiceEventCallabck callback)
        {
            if (!m_eventDic.ContainsKey(cmd))
                m_eventDic.Add(cmd, null);

            m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] - callback;
            m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] + callback;
        }

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void AddBuiltInListener(TCPSessionServiceBuiltIn et, TCPSessionServiceBuiltInEventCallabck callback)
        {
            int cmd = (int)et;
            if (!m_eventDic.ContainsKey(cmd))
                m_eventDic.Add(cmd, null);

            m_eventDic[cmd] = (TCPSessionServiceBuiltInEventCallabck)m_eventDic[cmd] - callback;
            m_eventDic[cmd] = (TCPSessionServiceBuiltInEventCallabck)m_eventDic[cmd] + callback;
        }

        /// <summary>
        /// 注册 Lua Function 监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="self"></param>
        /// <param name="func"></param>
        public static void AddListener(int cmd, LuaTable self, LuaFunction func)
        {
            if (!m_eventDic.ContainsKey(cmd))
                m_eventDic.Add(cmd, null);

            var callback = (TCPSessionServiceEventCallabck)DelegateTraits<TCPSessionServiceEventCallabck>.Create(func, self);
            m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] - callback;
            m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] + callback;
        }


        /// <summary>
        /// 移除cmd所有监听
        /// </summary>
        /// <param name="cmd"></param>
        public static void RemoveListener(int cmd)
        {
            if (m_eventDic.ContainsKey(cmd))
                m_eventDic[cmd] = null;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(int cmd, TCPSessionServiceEventCallabck callback)
        {
            if (m_eventDic.ContainsKey(cmd))
                m_eventDic[cmd] = (TCPSessionServiceEventCallabck)m_eventDic[cmd] - callback;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="callback"></param>
        public static void RemoveBuiltInListener(TCPSessionServiceBuiltIn et, TCPSessionServiceBuiltInEventCallabck callback)
        {
            int cmd = (int)et;
            if (m_eventDic.ContainsKey(cmd))
                m_eventDic[cmd] = (TCPSessionServiceBuiltInEventCallabck)m_eventDic[cmd] - callback;
        }

        /// <summary>
        /// 移除 Lua Function 监听
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="self"></param>
        /// <param name="func"></param>
        public static void RemoveListener(int cmd, LuaTable self, LuaFunction func)
        {
            if (m_eventDic.ContainsKey(cmd))
            {
                var callback = m_eventDic[cmd];

                LuaState state = func.GetLuaState();
                LuaDelegate target;
                if (self != null)
                    target = state.GetLuaDelegate(func, self);
                else
                    target = state.GetLuaDelegate(func);

                Delegate[] ds = callback.GetInvocationList();

                for (int i = 0; i < ds.Length; i++)
                {
                    LuaDelegate ld = ds[i].Target as LuaDelegate;
                    if (ld != null && ld.Equals(target))
                    {
                        m_eventDic[cmd] = (TCPSessionServiceEventCallabck)Delegate.Remove(callback, ds[i]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="pack"></param>
        public static void Broadcast(SocketPack pack)
        {
            Delegate cb = null;
            if (m_eventDic.TryGetValue(pack.cmd, out cb))
            {
                var callback = cb as TCPSessionServiceEventCallabck;
                callback.InvokeGracefully(pack);
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="pack"></param>
        public static void Broadcast(TCPSessionServiceBuiltIn et)
        {
            int cmd = (int)et;
            Delegate cb = null;
            if (m_eventDic.TryGetValue(cmd, out cb))
            {
                var callback = cb as TCPSessionServiceBuiltInEventCallabck;
                callback.InvokeGracefully();
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Clear()
        {
            foreach (KeyValuePair<int, Delegate> item in m_eventDic)
            {
                m_eventDic[item.Key] = null;
            }
            m_eventDic.Clear();
        }
    }
}