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
using FastEngine.Debuger;

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

    /// <summary>
    /// App 运行模式
    /// </summary>
    /// 开发模式
    ///     - 热更新:关闭
    ///     - Localization:非Bundle加载
    ///     - UI:非Bundle加载
    ///     - 其他资源:Bundle加载
    /// 正式模式
    ///     - 热更新:开启
    ///     - Localization:Bundle加载
    ///     - UI:Bundle加载
    ///     - 其他资源:Bundle加载
    /// 测试模式
    public enum AppRunModel
    {
        /// <summary>
        /// 开发模式
        /// - 关闭热更新
        /// </summary>
        Develop,
        /// <summary>
        /// 正式模式
        /// - 开启热更新
        /// - 开启bundle资源加载
        /// - 关闭日志
        /// </summary>
        Release,
        /// <summary>
        /// 测试模式
        /// - 开启热更新
        /// - 开启bundle资源加载
        /// - 开启日志
        /// </summary>
        Test,
    }

    [MonoSingletonPath("FastEngine/App")]
    public class App : MonoSingleton<App>
    {
        /// <summary>
        /// 运行模式
        /// </summary>
        public static AppRunModel runModel { get; private set; }

        /// <summary>
        /// 程序启动
        /// </summary>
        public void AppRun()
        {
            var appConfig = Config.ReadResourceDirectory<AppConfig>();

            // 运行模式
            runModel = appConfig.runModel;

            Application.runInBackground = true;
            Screen.fullScreen = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 2;

            #region Test Model

            // 设置测试环境分辨率
            if (runModel == AppRunModel.Test)
            {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
                Screen.SetResolution(appConfig.resolutionWidth, appConfig.resolutionHeight, false);
#endif
            }

            #endregion

            #region fairyGUI

            // 关闭移除包同时卸载Bundle
            UIPackage.unloadBundleByFGUI = false;

            // fairy 设置屏幕分辨率
            GRoot.inst.SetContentScaleFactor(2048, 1152, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            // 设置字体
            UIConfig.defaultFont = "Helvetica Condensed";
            // FontManager.RegisterFont(FontManager.GetFont("OLIVERSB"), "Oliver's Barney");

            #endregion

            DOTween.Init(true, true, LogBehaviour.Default);                         // DOTween
            ScriptWatch.Initialize(runModel != AppRunModel.Release);                // 代码监测
            Logger.Initialize(appConfig.enableLog);                                 // 日志
            TCPSession.Initialize(appConfig.enableLog);                             // 网络TCP
            var language = appConfig.useSystemLanguage ? Application.systemLanguage : appConfig.language;
            i18n.Initialize(language, appConfig.defaultLanguage);                   // 国际化
            SDK.Instance.Initialize();                                              // SDK
            if (runModel == AppRunModel.Develop)                                    // Lua(开发模式或者热更新完成方可启动)
            {
                Lua.Initialize();
            }
            else
            {
                Messenger.AddListener(MessengerEvent.HOTFIX_FINISHED, () =>
                {
                    Lua.Initialize();
                });
            }
        }

        public void AppQuit()
        {
            TCPSession.Disconnecte();      // 网络TCP
            Lua.Close();                   // Lua
            i18n.Dispose();                // 国际化
            SDK.Instance.Dispose();        // SDK
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