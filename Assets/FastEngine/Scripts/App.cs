/*
 * @Author: fasthro
 * @Date: 2019-10-09 14:34:03
 * @Description: App 入口
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using FastEngine.Core;
using Logger = FastEngine.Core.Logger;

namespace FastEngine
{
    public delegate void AppBehaviourCallback();
    public delegate void AppBehaviourCallback<T>(T arg);

    public enum APP_BEHAVIOUR
    {
        AppQuit,
        AppPause,
        AppFocus,

        Update,
        FixedUpdate,
        LateUpdate,

        OnGUI,
        OnDrawGizmos,
    }

    public class App : MonoSingleton<App>
    {
        void Awake() { AppRun(); }

        /// <summary>
        /// 程序启动
        /// </summary>
        public void AppRun()
        {
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // 日志
            Logger.Initialize(true);

            // 网络TCP
            // TCPSession.Initialize("192.168.1.41", 8080, true);
            TCPSession.Initialize("192.168.1.47", 8083, true);

            // 资源

            // Lua
        }

        public void AppQuit()
        {
            // 网络TCP
            TCPSession.Disconnecte();
        }

        #region Delegate
        private Dictionary<APP_BEHAVIOUR, Delegate> m_callbackDic = new Dictionary<APP_BEHAVIOUR, Delegate>();

        /// <summary>
        /// 绑定 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        /// <param name="callback">callback</param>
        public void BindCallback(APP_BEHAVIOUR act, AppBehaviourCallback callback)
        {
            if (!m_callbackDic.ContainsKey(act))
                m_callbackDic.Add(act, null);

            m_callbackDic[act] = (AppBehaviourCallback)m_callbackDic[act] + callback;
        }

        /// <summary>
        /// 绑定 Callback
        /// </summary>
        /// <param name="act"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        public void BindCallback<T>(APP_BEHAVIOUR act, AppBehaviourCallback<T> callback)
        {
            if (!m_callbackDic.ContainsKey(act))
                m_callbackDic.Add(act, null);

            m_callbackDic[act] = (AppBehaviourCallback<T>)m_callbackDic[act] + callback;
        }

        /// <summary>
        /// 广播 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        protected void BroadcastCallback(APP_BEHAVIOUR act)
        {
            if (m_callbackDic.ContainsKey(act))
                ((AppBehaviourCallback)m_callbackDic[act]).InvokeGracefully();
        }

        /// <summary>
        /// 广播 Callback
        /// </summary>
        /// <param name="act"></param>
        /// <param name="arg"></param>
        /// <typeparam name="T"></typeparam>
        protected void BroadcastCallback<T>(APP_BEHAVIOUR act, T arg)
        {
            if (m_callbackDic.ContainsKey(act))
                ((AppBehaviourCallback<T>)m_callbackDic[act]).InvokeGracefully(arg);
        }


        #endregion

        #region 生命周期
        void OnApplicationQuit()
        {
            AppQuit();
            BroadcastCallback(APP_BEHAVIOUR.AppQuit);
        }
        void OnApplicationPause(bool pause) { BroadcastCallback<bool>(APP_BEHAVIOUR.AppPause, pause); }
        void OnApplicationFocus(bool focus) { BroadcastCallback<bool>(APP_BEHAVIOUR.AppFocus, focus); }
        void Update() { BroadcastCallback(APP_BEHAVIOUR.Update); }
        private void LateUpdate() { BroadcastCallback(APP_BEHAVIOUR.LateUpdate); }
        private void FixedUpdate() { BroadcastCallback(APP_BEHAVIOUR.FixedUpdate); }
        void OnGUI() { BroadcastCallback(APP_BEHAVIOUR.OnGUI); }
        private void OnDrawGizmos() { BroadcastCallback(APP_BEHAVIOUR.OnDrawGizmos); }
        #endregion
    }
}