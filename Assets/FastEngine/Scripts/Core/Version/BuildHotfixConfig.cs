/*
 * @Author: fasthro
 * @Date: 2019-11-27 15:03:43
 * @Description: 打包热更新资源配置
 */
namespace FastEngine.Core
{
    public class BuildHotfixConfig
    {
        public bool cleanBuild { get; set; }                 // 是否清理打包
        public VersionConfig version { get; set; }           // 目标版本
        public VersionConfig fixVersion { get; set; }        // 修复版本
    }
}