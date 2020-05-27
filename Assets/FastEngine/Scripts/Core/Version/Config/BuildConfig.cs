/*
 * @Author: fasthro
 * @Date: 2019-11-27 15:03:43
 * @Description: 构建配置
 */
namespace FastEngine.Core
{
    public enum BuildPlatformType
    {
        Android,
        iOS,
        Windows
    }

    public abstract class BuildPlatformInfo
    {
        // bundleVersionCode 自增+1
        public int bundleVersionCode { get; set; }

        // 版本配置
        public VersionConfig version { get; set; }

        // c++ il2cpp
        public bool il2cpp;
    }

    public class AndroidBuildPlatformInfo : BuildPlatformInfo
    {
        public string keystoreName { get; set; }       // 签名文件
        public string keystorePass { get; set; }       // 密钥
        public string keyaliasName { get; set; }       // 别名
        public string keyaliasPass { get; set; }       // 密钥
    }

    public class IOSBuildPlatformInfo : BuildPlatformInfo
    {

    }

    public class WindowsBuildPlatformInfo : BuildPlatformInfo
    {

    }

    public class BuildConfig : ConfigObject
    {
        #region build

        // 是否清理打包
        public bool cleanBuild { get; set; }

        // platform
        public AndroidBuildPlatformInfo android { get; set; }
        public IOSBuildPlatformInfo ios { get; set; }
        public WindowsBuildPlatformInfo windows { get; set; }

        #endregion

        #region hotfix

        // 热更新目标版本
        public VersionConfig sourceVersion { get; set; }
        // 热更新版本         
        public VersionConfig fixVersion { get; set; }

        #endregion

        protected override void OnInitialize()
        {
            if (android == null)
            {
                android = new AndroidBuildPlatformInfo();
                InitPlatformVersion(android);
            }
            if (ios == null)
            {
                ios = new IOSBuildPlatformInfo();
                InitPlatformVersion(ios);
            }
            if (windows == null)
            {
                windows = new WindowsBuildPlatformInfo();
                InitPlatformVersion(windows);
            }
        }

        private void InitPlatformVersion(BuildPlatformInfo platformInfo)
        {
            if (platformInfo.version == null)
            {
                platformInfo.version = new VersionConfig();
            }
        }

        /// <summary>
        /// bundleVersionCode 自增
        /// </summary>
        /// <param name="platform"></param>
        public void AutoIncrementBundleVersionCode(BuildPlatformType platform)
        {
            if (platform == BuildPlatformType.Android)
            {
                android.bundleVersionCode++;
            }
            else if (platform == BuildPlatformType.iOS)
            {
                ios.bundleVersionCode++;
            }
            else if (platform == BuildPlatformType.Windows)
            {
                windows.bundleVersionCode++;
            }
        }

        /// <summary>
        /// 获取版本配置
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public VersionConfig GetVersion(BuildPlatformType platform)
        {
            if (platform == BuildPlatformType.Android)
            {
                return android.version;
            }
            else if (platform == BuildPlatformType.iOS)
            {
                return ios.version;
            }
            else if (platform == BuildPlatformType.Windows)
            {
                return windows.version;
            }
            return null;
        }
    }
}