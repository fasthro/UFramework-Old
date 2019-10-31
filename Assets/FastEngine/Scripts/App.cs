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
using DG.Tweening;
using FairyGUI;
using FastEngine.FUI;

namespace FastEngine
{
    public delegate void AppBehaviourCallback();
    public delegate void AppBehaviourCallback<T>(T arg);

    public enum AppBehaviour
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

    [MonoSingletonPath("FastEngine/App")]
    public class App : MonoSingleton<App>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        public void AppRun()
        {
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 2;

            // 设置屏幕分辨率
            Screen.SetResolution(2048, 1152, false);
            GRoot.inst.SetContentScaleFactor(2048, 1152, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
           
            DOTween.Init(true, true, LogBehaviour.Default);        // DOTween
            Logger.Initialize(true);                               // 日志
            TCPSession.Initialize("192.168.1.47", 8083, true);     // 网络TCP
            Lua.Initialize();                                      // Lua
            FWindowSortService.Initialize();                       // Window 排序服务
        }

        public void AppQuit()
        {
            TCPSession.Disconnecte();      // 网络TCP
            Lua.Close();                   // Lua
        }

        #region Delegate
        private Dictionary<AppBehaviour, Delegate> m_callbackDic = new Dictionary<AppBehaviour, Delegate>();

        /// <summary>
        /// 绑定 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        /// <param name="callback">callback</param>
        public void BindCallback(AppBehaviour act, AppBehaviourCallback callback)
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
        public void BindCallback<T>(AppBehaviour act, AppBehaviourCallback<T> callback)
        {
            if (!m_callbackDic.ContainsKey(act))
                m_callbackDic.Add(act, null);

            m_callbackDic[act] = (AppBehaviourCallback<T>)m_callbackDic[act] + callback;
        }

        /// <summary>
        /// 广播 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        protected void BroadcastCallback(AppBehaviour act)
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
        protected void BroadcastCallback<T>(AppBehaviour act, T arg)
        {
            if (m_callbackDic.ContainsKey(act))
                ((AppBehaviourCallback<T>)m_callbackDic[act]).InvokeGracefully(arg);
        }


        #endregion

        #region 生命周期
        void OnApplicationQuit()
        {
            AppQuit();
            BroadcastCallback(AppBehaviour.AppQuit);
        }
        void OnApplicationPause(bool pause) { BroadcastCallback<bool>(AppBehaviour.AppPause, pause); }
        void OnApplicationFocus(bool focus) { BroadcastCallback<bool>(AppBehaviour.AppFocus, focus); }
        void Update() { BroadcastCallback(AppBehaviour.Update); }
        private void LateUpdate() { BroadcastCallback(AppBehaviour.LateUpdate); }
        private void FixedUpdate() { BroadcastCallback(AppBehaviour.FixedUpdate); }
        void OnGUI() { BroadcastCallback(AppBehaviour.OnGUI); }
        private void OnDrawGizmos() { BroadcastCallback(AppBehaviour.OnDrawGizmos); }
        #endregion
    }
}