/*
 * @Author: fasthro
 * @Date: 2019-06-27 17:33:16
 * @Description: AssetBundle 资源依赖
 */
using FastEngine.Common;
using UnityEngine;

namespace FastEngine.Core
{
    public class AssetBundleDependencies : Singleton<AssetBundleDependencies>
    {
        private AssetBundle m_bundle;
        private AssetBundleManifest m_manifest;

        private AssetBundleDependencies() { }

        public override void OnSingletonInit()
        {
            m_bundle = AssetBundle.LoadFromFile(AssetBundlePath.GetFullPath(AssetBundlePath.GetPlatformIds()));
            m_manifest = m_bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        }

        public string[] GetDependencies(string bundleName)
        {
            return m_manifest.GetAllDependencies(bundleName);
        }
    }
}