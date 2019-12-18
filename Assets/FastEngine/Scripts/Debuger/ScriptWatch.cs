/*
 * @Author: fasthro
 * @Date: 2019-11-12 19:49:03
 * @Description: 脚本监视器
 */
using System.Diagnostics;

namespace FastEngine.Debuger
{
    public class ScriptWatch
    {
        static bool enabledWatch;
        static Stopwatch stopwatch = new Stopwatch();
        static string mark;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize(bool enabled)
        {
            enabledWatch = enabled;
        }

        /// <summary>
        /// 开始监测
        /// </summary>
        public static void Start(string markStr)
        {
            if (!enabledWatch) return;
            mark = markStr;
            stopwatch.Reset();
            stopwatch.Start();
        }

        /// <summary>
        /// 停止监测
        /// </summary>
        public static void Stop()
        {
            if (!enabledWatch) return;
            stopwatch.Stop();
            var t = stopwatch.Elapsed.TotalMilliseconds;
            string color = "";
            if (t <= 30)
            {
                color = "#00ff00";
            }
            else if (t > 30 && t < 100)
            {
                color = "#00ffff";
            }
            else
            {
                color = "#ff0000";
            }
            UnityEngine.Debug.Log(string.Format("<color={0}>[Script Watch] {1} 耗时: {2}毫秒</color>", color, mark, t));
        }
    }
}
