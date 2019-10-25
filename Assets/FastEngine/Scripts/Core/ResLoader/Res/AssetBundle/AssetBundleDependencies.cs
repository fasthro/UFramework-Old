/*
 * @Author: fasthro
 * @Date: 2019-06-27 17:33:16
 * @Description: AssetBundle 资源依赖
 */
 
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/ResLoader/AssetBundleDependencies")]
    public class AssetBundleDependencies : MonoSingleton<AssetBundleDependencies>
    {
        private AssetBundle m_bundle;
        private AssetBundleManifest m_manifest;

        public override void InitializeSingleton()
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