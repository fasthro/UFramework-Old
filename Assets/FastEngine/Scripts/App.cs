/*
 * @Author: fasthro
 * @Date: 2019-10-09 14:34:03
 * @Description: App 入口
 */

using UnityEngine;
using FastEngine.Common;

namespace FastEngine.Core
{
    public delegate void ApplicationBoolCallback(bool enable);
    public delegate void ApplicationVoidCallback();

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
            Log.Initialize(true);
        }

        #region 生命周期
        // public stat
        #endregion
    }
}