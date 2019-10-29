/*
 * @Author: fasthro
 * @Date: 2019-10-28 13:47:10
 * @Description: FUI Window (对FairyGUI Window 的包装)
 */
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
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
        private string[] m_dependPackNames;

        // com
        private string m_comName;

        // log
        public bool enabledLog = true;
        public string logMark = "";

        public FWindow(FLayer layer, string comName, string packName, string[] dependPackNames = null) : base()
        {
            this.layer = layer;
            this.m_comName = comName;
            this.m_packName = packName;
            this.m_dependPackNames = dependPackNames;
        }

        public void ShowWindow()
        {
            FPackService.Add(m_packName);
            Show();
        }

        public void HideWindow(bool autoDestory = false)
        {
            this.m_autoDestory = autoDestory;
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

