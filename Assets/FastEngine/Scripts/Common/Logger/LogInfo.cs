/*
 * @Author: fasthro
 * @Date: 2019-10-09 14:25:11
 * @Description: 日志信息
 */
using UnityEngine;

namespace FastEngine.Common
{
    public class LogInfo
    {
        // 内容
        public string content { get; private set; }
        // 堆栈
        public string track { get; private set; }
        // 类型
        public LogType type { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="track"></param>
        /// <param name="type"></param>
        public LogInfo(string content, string track, LogType type)
        {
            this.content = content;
            this.track = track;
            this.type = type;
        }
    }
}