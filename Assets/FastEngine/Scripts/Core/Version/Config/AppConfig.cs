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
    public class AppConfig : ConfigObject
    {
        // App 运行模式
        public AppRunModel runModel { get; set; }
        // 是否开启日志
        public bool enableLog { get; set; }

        // 使用系统语言
        public bool useSystemLanguage { get; set; }
        // 指定语言
        public SystemLanguage language { get; set; }
        // 默认语言
        public SystemLanguage defaultLanguage { get; set; }
        // 支持的语言
        public List<SystemLanguage> supportedLanuages { get; set; }

        // 分辨率([width,height] runModel 为 Test 情况下使用)
        public int resolutionWidth { get; set; }
        public int resolutionHeight { get; set; }

        // 基础资源包压缩文件数量
        public int compressFileTotalCount { get; set; }

        // 版本信息
        public VersionConfig version { get; set; }

        protected override void OnInitialize()
        {
            if (supportedLanuages == null)
            {
                supportedLanuages = new List<SystemLanguage>();
            }
            if (version == null)
            {
                version = new VersionConfig();
            }
        }
    }
}