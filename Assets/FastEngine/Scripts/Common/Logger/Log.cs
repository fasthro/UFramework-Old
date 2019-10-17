/*
 * @Author: fasthro
 * @Date: 2019-10-09 14:01:22
 * @Description: 日志系统
 */
using UnityEngine;

namespace FastEngine.Common
{
    public class Log
    {
        // 日志记录器
        private static LogWriter logWriter;

        /// <summary>
        /// 日志初始化
        /// </summary>
        /// <param name="enabled">日志开关</param>
        public static void Initialize(bool enabled)
        {
            // Unity 日志开关
            Debug.unityLogger.logEnabled = enabled;

            if (!enabled) return;

            logWriter = new LogWriter();

            Application.logMessageReceivedThreaded -= ReceivedThreaded;
            Application.logMessageReceivedThreaded += ReceivedThreaded;
        }

        /// <summary>
        /// 接收日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="track"></param>
        /// <param name="type"></param>
        private static void ReceivedThreaded(string log, string track, LogType type)
        {
            logWriter.Received(new LogInfo(log, track, type));
        }

        #region 日志模块
        public static void LogInfo(string message, LogModule module = LogModule.Unity)
        {
            Debug.Log(LogModuleWrap.Wrap(module, message));
        }

        public static void LogWarning(string message, LogModule module = LogModule.Unity)
        {
            Debug.LogWarning(LogModuleWrap.Wrap(module, message));
        }

        public static void LogError(string message, LogModule module = LogModule.Unity)
        {
            Debug.LogError(LogModuleWrap.Wrap(module, message));
        }
        #endregion
    }
}