/*
 * @Author: fasthro
 * @Date: 2019-06-26 18:08:21
 * @Description: AssetBundle资源路径
 */
using System.IO;
using UnityEngine;

namespace FastEngine.Core
{
    public class AssetBundlePath
    {
        /// <summary>
        /// 平台标识Id
        /// </summary>
        public static string PlatformId()
        {
            string ids = "";
#if UNITY_ANDROID
            ids = "Android";
#elif UNITY_IPHONE
            ids = "IOS";
#elif UNITY_STANDALONE_WIN
            ids = "Windows";
#elif UNITY_STANDALONE_OSX
            ids = "OSX";
#endif
            return ids;
        }

        /// <summary>
        /// AssetBunlde 路径
        /// </summary>
        /// <param name="bundleName"></param>
        public static string GetFilePath(string bundleName)
        {
            return Path.Combine(RootDirectory(), bundleName);
        }

        /// <summary>
        /// AssetBundle 根目录
        /// </summary>
        /// <returns></returns>
        public static string RootDirectory()
        {
            if (Application.isMobilePlatform)
            {
                return Path.Combine(Application.persistentDataPath, PlatformId());
            }
            return Path.Combine(Application.streamingAssetsPath, PlatformId());
        }

        /// <summary>
        /// AssetBundle 依赖文件路径
        /// </summary>
        /// <returns></returns>
        public static string DependencieFilePath()
        {
            return Path.Combine(RootDirectory(), PlatformId());
        }

        /// <summary>
        /// AssetBundle 资源映射配置路径
        /// </summary>
        /// <returns></returns>
        public static string MappingFilePath()
        {
            return Path.Combine(RootDirectory(), PlatformId() + ".json");
        }

        /// <summary>
        /// AssetBundle 编辑器配置路径
        /// </summary>
        /// <returns></returns>
        public static string EditorConfigFilePath()
        {
            return Path.Combine(Application.dataPath, "EditorConfig/AssetBundleConfig.json");
        }
    }
}