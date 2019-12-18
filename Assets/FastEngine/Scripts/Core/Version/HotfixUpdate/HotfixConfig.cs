/*
 * @Author: fasthro
 * @Date: 2019-11-29 13:49:18
 * @Description: 热更新配置
 */
namespace FastEngine.Core
{
    public class HotfixConfig
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public class FileInfo
        {
            public string path { get; set; }         // 文件路径
            public string hash { get; set; }         // hash key
            public string version { get; set; }      // version string(1.0.0.0)
            public int size { get; set; }            // 文件大小
        }

        public VersionConfig version { get; set; }   // 版本
        public FileInfo[] fileInfos { get; set; }    // 文件信息列表
        public bool hotfix { get; set; }             // 低版本是否进行热更新
        public int hotfixFileSize { get; set; }      // 热更文件总大小
        public int hotfixFileCount { get; set; }     // 热更文件总数量
    }
}
