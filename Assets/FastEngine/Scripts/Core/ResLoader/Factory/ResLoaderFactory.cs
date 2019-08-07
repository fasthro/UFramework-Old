/*
 * @Author: fasthro
 * @Date: 2019-06-28 11:08:46
 * @Description: loader 工厂
 */
namespace FastEngine.Core
{
    public static class ResLoaderFactory
    {
        
        /// <summary>
        /// 创建 AssetBundleLoader
        /// </summary>
        /// <param name="bundleName">assetBundle名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="listener"></param>
        public static AssetBundleLoader CreateAssetBundleLoader(string bundleName, string assetName, ResNotificationListener listener)
        {
            return AssetBundleLoader.Allocate(bundleName, assetName, listener);
        }

        /// <summary>
        /// 创建 ResourceLoader
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="listener"></param>
        public static ResourceLoader CreateResourceLoader(string assetName, ResNotificationListener listener)
        {
            return ResourceLoader.Allocate(assetName, listener);
        }
    }
}