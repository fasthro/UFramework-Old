/*
 * @Author: fasthro
 * @Date: 2019-10-28 13:47:10
 * @Description: fairy ui window
 */
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using FastEngine.Core;
using Google.Protobuf;
using LuaInterface;
using UnityEngine;

namespace FastEngine.FairyUI
{
    /// <summary>
    /// window state
    /// </summary>
    public enum FairyWindowState
    {
        Init,
        Loading,
        Showing,
        Hided,
        Dispose,
    }

    /// <summary>
    /// fairy window
    /// </summary>
    public class FairyWindow : Window
    {
        public FairyLayerType layer { get; protected set; }
        public FairyWindowState state { get; protected set; }

        private string m_mainPackageName;  // mian package name
        private UIPackage m_mianPackage;   // main package
        private string[] m_dependencies;   // 依赖包名称列表
        private string m_comName;          // 组件名称
        private int[] m_tcpCmds;           // TCP 网络协议(自动监听和自动取消监听)

        public bool enabledLog = true;     // 是否开启日志
        public string logMark = "";        // 日志标志
        public bool fullScreen = true;     // 是否全屏UI(标志是否为全屏UI，可以通过此标志设置场景相机在打开全屏UI的时候关闭渲染，进行DrawCall优化)

        public GComponent handle { get { return contentPane; } }  // 窗口组件句柄

        /// <summary>
        /// window
        /// </summary>
        /// <param name="layer">层</param>
        /// <param name="comName">组将名称</param>
        /// <param name="packageName">组件所在包名称</param>
        /// <param name="dependPackageNames">依赖包名称列表</param>
        /// <param name="tcpCmds">网络命令列表</param>
        /// <returns></returns>
        public FairyWindow(FairyLayerType layer, string comName, string packageName, string[] dependPackageNames = null, int[] tcpCmds = null) : base()
        {
            this.layer = layer;
            this.m_comName = comName;
            this.m_mainPackageName = packageName;
            this.m_dependencies = dependPackageNames;
            this.m_tcpCmds = tcpCmds;
            this.state = FairyWindowState.Init;
        }

        #region api

        /// <summary>
        /// show window
        /// </summary>
        new public void Show()
        {
            if (state == FairyWindowState.Init)
            {
                // add main package
                m_mianPackage = FairyPackage.Add(m_mainPackageName);
                // add package dependencies
                for (int i = 0; i < m_mianPackage.dependencies.Length; i++)
                {
                    foreach (KeyValuePair<string, string> item in m_mianPackage.dependencies[i])
                    {
                        if (item.Key == "name") FairyPackage.Add(item.Value);
                    }
                }
                // add custom package dependencies
                if (m_dependencies != null) FairyPackage.Add(m_dependencies);
            }
            base.Show();
        }

        /// <summary>
        /// hide window
        /// </summary>
        new public void Hide()
        {
            base.Hide();
        }

        /// <summary>
        /// dispose window
        /// </summary>
        new public void Dispose()
        {
            for (int i = 0; i < m_mianPackage.dependencies.Length; i++)
            {
                foreach (KeyValuePair<string, string> item in m_mianPackage.dependencies[i])
                {
                    if (item.Key == "name") FairyPackage.Remove(item.Value);
                }
            }
            if (m_dependencies != null) FairyPackage.Remove(m_dependencies);
            Hide();
            base.Dispose();
        }

        /// <summary>
        /// refresh window
        /// </summary>
        public void Refresh()
        {
            if (state == FairyWindowState.Showing)
            {
                OnRefresh();
            }
        }
        #endregion

        #region on action

        protected override void OnInit()
        {
            state = FairyWindowState.Init;
            MakeFullScreen();
            contentPane = UIPackage.CreateObject(m_mainPackageName, m_comName).asCom;
            contentPane.SetSize(GRoot.inst.width, GRoot.inst.height);
            sortingOrder = FairyWindowSortingOrder.Add(this);
            gameObjectName = string.Format("FairyWindow-{0}-layer:{1}-{2}", m_comName, layer.ToString(), sortingOrder.ToString());
            base.OnInit();
        }

        protected override void OnShown()
        {
            state = FairyWindowState.Showing;
            // 注册网络协议
            if (m_tcpCmds != null)
            {
                for (int i = 0; i < m_tcpCmds.Length; i++)
                {
                    TCPSessionService.AddListener(m_tcpCmds[i], OnTCPReceived);
                }
            }
            base.OnShown();
        }

        protected override void OnHide()
        {
            state = FairyWindowState.Hided;
            // 移除网络协议
            if (m_tcpCmds != null)
            {
                for (int i = 0; i < m_tcpCmds.Length; i++)
                {
                    TCPSessionService.RemoveListener(m_tcpCmds[i], OnTCPReceived);
                }
            }
            base.OnHide();
        }

        protected virtual void OnDispose()
        {
            state = FairyWindowState.Dispose;
#if FAIRYGUI_TOLUA
            CallLua("OnDestory");
#endif
        }

        protected virtual void OnRefresh()
        {
#if FAIRYGUI_TOLUA
            CallLua("OnRefresh");
#endif
        }

        protected virtual void OnTCPReceived(SocketPack pack)
        {
#if FAIRYGUI_TOLUA
            if (_peerTable != null)
            {
                LuaFunction ctor = _peerTable.GetLuaFunction("OnTCPReceived");
                if (ctor != null)
                {
                    ctor.Call(this, pack);
                    ctor.Dispose();
                }
            }
#endif
        }
        #endregion

        #region 网络消息
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
            Debug.Log(string.Format("[FairyWindow-{0}] {1}", logMark, message));
        }

        public void LogError(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) logMark = m_comName;
            Debug.LogError(string.Format("[FairyWindow-{0}] {1}", logMark, message));
        }

        public void LogWarning(string message)
        {
            if (!FastEngine.Core.Logger.logEnabled) return;
            if (!enabledLog) return;
            if (string.IsNullOrEmpty(logMark)) logMark = m_comName;
            Debug.LogWarning(string.Format("[FairyWindow-{0}] {1}", logMark, message));
        }
        #endregion
    }
}

