/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:31:12
 * @Description: 版本配置 1.0.1.1 前三位游戏版本，最后一位资源版本
 */
namespace FastEngine.Core
{
    public class VersionConfig : IConfig
    {
        // 大号
        public int large { get; set; }
        // 中号
        public int middle { get; set; }
        // 小号
        public int small { get; set; }
        // 资源号
        public int resource { get; set; }

        // 压缩文件总数量(用于计算解压进度)
        public int compressFileTotalCount { get; set; }

        public void Initialize() { }

        /// <summary>
        /// to version string
        /// </summary>
        /// <returns></returns>
        public string ToVersionString()
        {
            return string.Format("{0}.{1}.{2}", large, middle, small);
        }

        /// <summary>
        /// to resource version
        /// </summary>
        /// <returns></returns>
        public string ToResourceString()
        {
            return string.Format("{0}.{1}.{2}.{3}", large, middle, small, resource);
        }

        /// <summary>
        /// 版本字符串描述转换成 VersionConfig
        /// </summary>
        /// <param name="versionStr"></param>
        /// <returns></returns>
        public static VersionConfig ToObject(string versionStr)
        {
            var vs = versionStr.Split('.');
            if (vs.Length < 3) return null;
            var version = new VersionConfig();
            try
            {
                version.large = int.Parse(vs[0]);
                version.middle = int.Parse(vs[1]);
                version.small = int.Parse(vs[2]);
                if (vs.Length >= 4)
                    version.resource = int.Parse(vs[3]);

            }
            catch
            {
                return null;
            }
            return version;
        }

        /// <summary>
        /// 比对游戏版本，决定游戏是否需要强制更新包
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="bc"></param>
        /// <returns>-1 小于, 0 等于, 1 大于</returns>
        public static int CompareVersion(VersionConfig ac, VersionConfig bc)
        {
            if (ac.large > bc.large) return 1;
            else if (ac.large < bc.large) return -1;
            else
            {
                if (ac.middle > bc.middle) return 1;
                else if (ac.middle > bc.middle) return -1;
                else
                {
                    if (ac.small > bc.small) return 1;
                    else if (ac.small > bc.small) return -1;
                    else return 0;
                }
            }
        }

        /// <summary>
        /// 比对资源版本，决定游戏是否需要热更新
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="bc"></param>
        /// <returns>-1 小于, 0 等于, 1 大于</returns>
        public static int CompareResourceVersion(VersionConfig ac, VersionConfig bc)
        {
            if (ac.resource > bc.resource) return 1;
            else if (ac.resource < bc.resource) return -1;
            else return 0;
        }
    }
}