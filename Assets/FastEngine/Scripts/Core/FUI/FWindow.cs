/*
 * @Author: fasthro
 * @Date: 2019-10-28 13:47:10
 * @Description: FUI Window (对FairyGUI Window 的包装)
 */
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using FastEngine.Core;
using Google.Protobuf;
using UnityEngine;

namespace FastEngine.FUI
{
    /// <summary>
    /// window state
    /// </summary>
    public enum FWindowState
    {
        Init,
        Loading,
        Showing,
        Hided,
        Destory,
    }

    /// <summary>
    /// window
    /// </summary>
    public class FWindow : Window
    {
        // layer
        public FLayer layer { get; protected set; }
        // state
        public FWindowState state { get; protected set; }

        // auto destory
        private bool m_autoDestory;

        // pack
        private string m_packName;
        private string[] m_dependencies;

        // main package
        private UIPackage m_pack;

        // com
        private string m_comName;

        // log
        public bool enabledLog = true;
        public string logMark = "";

        // 窗口组件包装
        public GComponent ui { get { return contentPane; } }

        public FWindow(FLayer layer, string comName, string packName, string[] dependPackNames = null) : base()
        {
            this.layer = layer;
            this.m_comName = comName;
            this.m_packName = packName;
            this.m_dependencies = dependPackNames;
        }

        public void ShowWindow()
        {
            // 加载包
            m_pack = FPackService.Add(m_packName);
            // 加载依赖包
            for (int i = 0; i < m_pack.dependencies.Length; i++)
            {
                foreach (KeyValuePair<string, string> item in m_pack.dependencies[i])
                {
                    if (item.Key == "name") FPackService.Add(item.Value);
                }
            }
            // 加载自定义依赖包
            if (m_dependencies != null) FPackService.Add(m_dependencies);

            Show();
        }

        public void HideWindow(bool autoDestory = false)
        {
            this.m_autoDestory = autoDestory;
            for (int i = 0; i < m_pack.dependencies.Length; i++)
            {
                foreach (KeyValuePair<string, string> item in m_pack.dependencies[i])
                {
                    if (item.Key == "name") FPackService.Add(item.Value);
                }
            }
            if (m_dependencies != null) FPackService.Remove(m_dependencies);
            Hide();
        }

        public void RefreshWindow()
        {
            if (state == FWindowState.Showing) OnRefresh();
        }

        protected override void OnInit()
        {
            state = FWindowState.Init;

            MakeFullScreen();
            contentPane = UIPackage.CreateObject(m_packName, m_comName).asCom;
            contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
            sortingOrder = FWindowSortService.Add(this);
            gameObjectName = string.Format("FWindow-{0}-layer:{1}-{2}", m_comName, layer.ToString(), sortingOrder.ToString());
            base.OnInit();
        }

        protected override void OnShown()
        {
            state = FWindowState.Showing;

            base.OnShown();
        }

        protected override void OnHide()
        {
            state = FWindowState.Hided;
            base.OnHide();

            if (this.m_autoDestory) { OnDestory(); }
        }

        protected void OnRefresh()
        {
#if FAIRYGUI_TOLUA
            CallLua("OnRefresh");
#endif
        }

        protected void OnDestory()
        {
            state = FWindowState.Destory;
#if FAIRYGUI_TOLUA
            CallLua("OnDestory");
#endif
        }

        #region 网络消息
        public void Send(int cmd) { TCPSession.Send(cmd); }
        public void Send(SocketPack pack) { TCPSession.Send(pack); }
        public void Send(int cmd, IMessage message) { TCPSession.Send(cmd, message); }
        public void Send(int cmd, string serialize) { TCPSession.Send(cmd, serialize); }
        #endregion

        #region log
        public void Log(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) Debug.Log(message);
            else Debug.Log(string.Format("[FUI-{0}] {1}", logMark, message));
        }

        public void LogError(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) Debug.LogError(message);
            else Debug.LogError(string.Format("[FUI-{0}] {1}", logMark, message));
        }

        public void LogWarning(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) Debug.LogWarning(message);
            else Debug.LogWarning(string.Format("[FUI-{0}] {1}", logMark, message));
        }
        #endregion
    }
}

