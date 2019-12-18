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
using LuaInterface;
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
        public FLayer layer { get; protected set; }
        public FWindowState state { get; protected set; }

        private bool m_autoDestory;        // 关闭界面自动销毁
        private string m_packName;         // mian package name
        private UIPackage m_pack;          // main package
        private string[] m_dependencies;   // 依赖包名称列表
        private string m_comName;          // 组件名称
        private int[] m_tcpCmds;           // TCP 网络协议(自动监听和自动取消监听)

        public bool enabledLog = true;     // 是否开启日志
        public string logMark = "";        // 日志标志
        public bool fullScreen = true;     // 是否全屏UI(标志是否为全屏UI，可以通过此标志设置场景相机在打开全屏UI的时候关闭渲染，进行DrawCall优化)

        public GComponent handle { get { return contentPane; } }  // 窗口组件句柄

        public FWindow(FLayer layer, string comName, string packName, string[] dependPackNames = null, int[] tcpCmds = null) : base()
        {
            this.layer = layer;
            this.m_comName = comName;
            this.m_packName = packName;
            this.m_dependencies = dependPackNames;
            this.m_tcpCmds = tcpCmds;
            this.state = FWindowState.Init;
        }

        public void ShowWindow()
        {
            if (state == FWindowState.Init)
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
            }

            Show();
        }

        public void HideWindow(bool autoDestory = false)
        {
            this.m_autoDestory = autoDestory;
            if (autoDestory)
            {
                for (int i = 0; i < m_pack.dependencies.Length; i++)
                {
                    foreach (KeyValuePair<string, string> item in m_pack.dependencies[i])
                    {
                        if (item.Key == "name") FPackService.Remove(item.Value);
                    }
                }
                if (m_dependencies != null) FPackService.Remove(m_dependencies);
            }
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

            // 注册网络协议
            if (m_tcpCmds != null)
            {
                for (int i = 0; i < m_tcpCmds.Length; i++)
                {
                    TCPAddListener(m_tcpCmds[i], this.TCPReceiveCallback);
                }
            }

            base.OnShown();
        }

        protected override void OnHide()
        {
            state = FWindowState.Hided;

            // 取消注册网络协议
            if (m_tcpCmds != null)
            {
                for (int i = 0; i < m_tcpCmds.Length; i++)
                {
                    TCPRemoveListener(m_tcpCmds[i], this.TCPReceiveCallback);
                }
            }

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
            Dispose();
            state = FWindowState.Destory;
#if FAIRYGUI_TOLUA
            CallLua("OnDestory");
#endif
        }

        #region 定时器

        #endregion

        #region 本地化多语言
        /// <summary>
        /// 获取多语言文本
        /// </summary>
        /// <param name="model">多语言模块名称</param>
        /// <param name="key">多语言可以</param>
        /// <returns></returns>
        public string LanguageGet(string model, int key)
        {
            return Localization.Get(model, key);
        }

        #endregion

        #region 网络消息

        private void TCPReceiveCallback(SocketPack pack)
        {
            OnTCPReceiveCallback(pack);
        }

        protected void OnTCPReceiveCallback(SocketPack pack)
        {
#if FAIRYGUI_TOLUA
            if (_peerTable != null)
            {
                LuaFunction ctor = _peerTable.GetLuaFunction("OnTCPReceiveCallback");
                if (ctor != null)
                {
                    ctor.Call(this, pack);
                    ctor.Dispose();
                }
            }
#endif
        }

        // 网络协议监听
        public void TCPAddListener(int cmd, TCPSessionServiceEventCallabck callback) { TCPSessionService.AddListener(cmd, callback); }
        public void TCPAddListener(int cmd, LuaFunction callback) { TCPSessionService.AddListener(cmd, _peerTable, callback); }
        public void TCPRemoveListener(int cmd) { TCPSessionService.RemoveListener(cmd); }
        public void TCPRemoveListener(int cmd, TCPSessionServiceEventCallabck callback) { TCPSessionService.RemoveListener(cmd, callback); }
        public void TCPRemoveListener(int cmd, LuaFunction callback) { TCPSessionService.RemoveListener(cmd, _peerTable, callback); }

        // 网络发送
        public void TCPSend(int cmd) { TCPSession.Send(cmd); }
        public void TCPSend(SocketPack pack) { TCPSession.Send(pack); }
        public void TCPSend(int cmd, IMessage message) { TCPSession.Send(cmd, message); }
        public void TCPSend(int cmd, LuaByteBuffer luabyte) { TCPSession.Send(cmd, luabyte); }
        #endregion

        #region log
        public void Log(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) logMark = m_comName;
            Debug.Log(string.Format("[FUI-{0}] {1}", logMark, message));
        }

        public void LogError(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) logMark = m_comName;
            Debug.LogError(string.Format("[FUI-{0}] {1}", logMark, message));
        }

        public void LogWarning(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) logMark = m_comName;
            Debug.LogWarning(string.Format("[FUI-{0}] {1}", logMark, message));
        }
        #endregion
    }
}

