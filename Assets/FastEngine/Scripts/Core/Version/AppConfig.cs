/*
 * @Author: fasthro
 * @Date: 2019-11-27 15:03:43
 * @Description: App 配置
 */
using System.Collections.Generic;
using FastEngine;
using UnityEngine;

namespace FastEngine.Core
{
    public class AppConfig
    {
        // 是否开启日志
        public bool enableLog { get; set; }
        // App 运行模式
        public AppRunModel runModel { get; set; }
        // 使用系统语言
        public bool useSystemLanguage { get; set; }
        // 指定语言
        public SystemLanguage language { get; set; }
        // 默认语言
        public SystemLanguage defaultLanguage { get; set; }
        // 支持的语言
        public List<SystemLanguage> supportedLanuages { get; set; }
        // QATest
        public bool QATest { get; set; }

        public AppConfig()
        {
            
        }
    }
}