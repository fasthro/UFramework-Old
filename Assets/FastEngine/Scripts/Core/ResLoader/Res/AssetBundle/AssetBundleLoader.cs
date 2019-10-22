/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Assetbundle 资源加载器
 */

namespace FastEngine.Core
{
    public class AssetBundleLoader : ResLoader, IPoolObject
    {
        // bundle 资源
        protected BundleRes m_bundleRes;
        public BundleRes bundleRes { get { return m_bundleRes; } }

        // asset 资源
        protected AssetRes m_assetRes;
        public AssetRes assetRes { get { return m_assetRes; } }

        // 只加载bundle
        protected bool m_only;

        public static AssetBundleLoader Allocate(string bundleName, string assetName, ResNotificationListener listener)
        {
            var loader = ObjectPool<AssetBundleLoader>.Instance.Allocate();
            loader.Init(bundleName, assetName, listener);
            return loader;
        }

        #region IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<AssetBundleLoader>.Instance.Recycle(this);
        }
        #endregion

        public void Init(string bundleName, string assetName, ResNotificationListener listener)
        {
            m_only = string.IsNullOrEmpty(assetName);

            m_bundleRes = ResPool.Get<BundleRes>(ResData.AllocateBundle(bundleName), true);

            if (!m_only)
                m_assetRes = ResPool.Get<AssetRes>(ResData.AllocateAsset(assetName, bundleName), true);

            m_listener = listener;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            bool _bundle = m_bundleRes.LoadSync();
            bool _asset = m_only ? true : m_assetRes.LoadSync();
            bool ready = _bundle && _asset;
            m_listener.InvokeGracefully(ready, m_assetRes);
            return ready;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            bundleRes.AddNotification(OnReceiveNotification);
            bundleRes.LoadAsync();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="res"></param>

        protected override void OnReceiveNotification(bool ready, Res res)
        {
            if (res.type == ResType.Bundle)
            {
                bundleRes.RemoveNotification(OnReceiveNotification);
                if (m_only)
                {
                    m_listener.InvokeGracefully(ready, res);
                }
                else
                {
                    assetRes.AddNotification(OnReceiveNotification);
                    assetRes.LoadAsync();
                }
            }
            else if (res.type == ResType.Asset)
            {
                assetRes.RemoveNotification(OnReceiveNotification);
                m_listener.InvokeGracefully(ready, res);
            }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            if (m_bundleRes != null)
            {
                m_bundleRes.RemoveNotification(OnReceiveNotification);
                m_bundleRes.Unload();
                m_bundleRes = null;
            }

            if (m_assetRes != null)
            {
                m_assetRes.RemoveNotification(OnReceiveNotification);
                m_assetRes.Unload();
                m_assetRes = null;
            }

            m_listener = null;

            Recycle();
        }
    }
}