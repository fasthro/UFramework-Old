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
        /// 获取平台标识
        /// </summary>
        public static string GetPlatformIds()
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
        /// 获取完整的AssetBunlde路径
        /// </summary>
        /// <param name="bundleName"></param>
        public static string GetFullPath(string bundleName)
        {
            string root = Path.Combine(Application.streamingAssetsPath, GetPlatformIds());
            return Path.Combine(root, bundleName);
        }
    }
}