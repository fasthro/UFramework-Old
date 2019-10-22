/*
 * @Author: fasthro
 * @Date: 2019-10-18 11:23:02
 * @Description: 用户日志,日志快关受 Logger.logEnabled 影响
 */
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class LogUser
    {
        private string m_mark;
        private bool m_enabled = false;

        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="enabled"></param>
        protected void InitializeLogUser(string mark, bool enabled)
        {
            if (Logger.logEnabled)
                this.m_enabled = enabled;
            this.m_mark = mark;
        }

        protected void Log(string message)
        {
            if (!m_enabled) return;
            if (string.IsNullOrEmpty(m_mark)) Debug.Log(message);
            else Debug.Log(string.Format("[{0}] {1}", m_mark, message));
        }
        protected void LogFormat(string format, params object[] args)
        {
            if (!m_enabled) return;
            Debug.LogFormat(format, args);
        }

        protected void LogError(string message)
        {
            if (!m_enabled) return;
            if (string.IsNullOrEmpty(m_mark)) Debug.LogError(message);
            else Debug.LogError(string.Format("[{0}] {1}", m_mark, message));
        }
        protected void LogErrorFormat(string format, params object[] args)
        {
            if (!m_enabled) return;
            Debug.LogErrorFormat(format, args);
        }

        protected void LogWarning(string message)
        {
            if (!m_enabled) return; if (!m_enabled) return;
            if (string.IsNullOrEmpty(m_mark)) Debug.LogWarning(message);
            else Debug.LogWarning(string.Format("[{0}] {1}", m_mark, message));
        }
        protected void LogWarningFormat(string format, params object[] args)
        {
            if (!m_enabled) return; if (!m_enabled) return;
            Debug.LogWarningFormat(format, args);
        }
    }
}