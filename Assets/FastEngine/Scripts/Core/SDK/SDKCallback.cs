/*
 * @Author: fasthro
 * @Date: 2020-05-21 15:29:37
 * @Description: SDK Callback
 */

using System;
using System.Collections.Generic;

namespace FastEngine.Core
{
    /// <summary>
    /// Login Event Handler
    /// </summary>
    /// <param name="callbackInfo"></param>
    public delegate void LoginSDKCallbackEventHandler(LoginCallbackInfo callbackInfo);

    /// <summary>
    /// Pay Event Handler
    /// </summary>
    /// <param name="callbackInfo"></param>
    public delegate void PaySDKCallbackEventHandler(PayCallbackInfo callbackInfo);

    /// <summary>
    /// DNA Event Handler
    /// </summary>
    /// <param name="callbackInfo"></param>
    public delegate void DNASDKCallbackEventHandler(DNACallbackInfo callbackInfo);

    /// <summary>
    /// 事件类型
    /// </summary>
    public enum SDKCallbackEvent
    {
        LOGIN,
        PAY,
        DNA,
    }

    [MonoSingletonPath("FastEngine/SDKCallback")]
    public class SDKCallback : MonoSingleton<SDKCallback>
    {
        static Dictionary<SDKCallbackEvent, Delegate> m_eventDic = new Dictionary<SDKCallbackEvent, Delegate>();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void AddListener(SDKCallbackEvent eventType, LoginSDKCallbackEventHandler callback)
        {
            if (!m_eventDic.ContainsKey(eventType))
                m_eventDic.Add(eventType, null);

            m_eventDic[eventType] = (LoginSDKCallbackEventHandler)m_eventDic[eventType] + callback;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void AddListener(SDKCallbackEvent eventType, PaySDKCallbackEventHandler callback)
        {
            if (!m_eventDic.ContainsKey(eventType))
                m_eventDic.Add(eventType, null);

            m_eventDic[eventType] = (PaySDKCallbackEventHandler)m_eventDic[eventType] + callback;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void AddListener(SDKCallbackEvent eventType, DNASDKCallbackEventHandler callback)
        {
            if (!m_eventDic.ContainsKey(eventType))
                m_eventDic.Add(eventType, null);

            m_eventDic[eventType] = (DNASDKCallbackEventHandler)m_eventDic[eventType] + callback;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(SDKCallbackEvent eventType, LoginSDKCallbackEventHandler callback)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic[eventType] = (LoginSDKCallbackEventHandler)m_eventDic[eventType] - callback;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(SDKCallbackEvent eventType, PaySDKCallbackEventHandler callback)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic[eventType] = (PaySDKCallbackEventHandler)m_eventDic[eventType] - callback;
        }


        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public static void RemoveListener(SDKCallbackEvent eventType, DNASDKCallbackEventHandler callback)
        {
            if (m_eventDic.ContainsKey(eventType))
                m_eventDic[eventType] = (DNASDKCallbackEventHandler)m_eventDic[eventType] - callback;
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callbackInfo"></param>
        public static void Broadcast(SDKCallbackEvent eventType, SDKCallbackInfo callbackInfo)
        {
            Delegate dc = null;
            if (m_eventDic.TryGetValue(eventType, out dc))
            {
                if (eventType == SDKCallbackEvent.LOGIN)
                {
                    var callback = dc as LoginSDKCallbackEventHandler;
                    callback.InvokeGracefully(callbackInfo as LoginCallbackInfo);
                }
                else if (eventType == SDKCallbackEvent.PAY)
                {
                    var callback = dc as PaySDKCallbackEventHandler;
                    callback.InvokeGracefully(callbackInfo as PayCallbackInfo);
                }
                else if (eventType == SDKCallbackEvent.DNA)
                {
                    var callback = dc as DNASDKCallbackEventHandler;
                    callback.InvokeGracefully(callbackInfo as DNACallbackInfo);
                }
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public static void Clean()
        {
            m_eventDic.Clear();
        }
    }
}