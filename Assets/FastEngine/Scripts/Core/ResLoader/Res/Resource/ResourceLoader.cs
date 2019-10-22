/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Resources 资源加载器
 */

namespace FastEngine.Core
{
    public class ResourceLoader : ResLoader, IPoolObject
    {
        // 主资源
        protected ResourceRes m_mainRes;
        public ResourceRes assetRes { get { return m_mainRes; } }

        public static ResourceLoader Allocate(string assetName)
        {
            return Allocate(assetName, null);
        }

        public static ResourceLoader Allocate(string assetName, ResNotificationListener listener)
        {
            var loader = ObjectPool<ResourceLoader>.Instance.Allocate();
            loader.Init(assetName, listener);
            return loader;
        }

        #region IPoolObject=
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResourceLoader>.Instance.Recycle(this);
        }
        #endregion

        public void Init(string assetName, ResNotificationListener listener)
        {
            m_mainRes = ResPool.Get<ResourceRes>(ResData.AllocateResource(assetName), true);
            m_listener = listener;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            bool ready = m_mainRes.LoadSync();
            m_listener.InvokeGracefully(ready, m_mainRes);
            return ready;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            m_mainRes.AddNotification(OnReceiveNotification);
            m_mainRes.LoadAsync();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="res"></param>
        protected override void OnReceiveNotification(bool readly, Res res)
        {
            m_mainRes.RemoveNotification(OnReceiveNotification);
            m_listener.InvokeGracefully(readly, res);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            m_mainRes.RemoveNotification(OnReceiveNotification);
            m_mainRes.Unload();
            m_mainRes = null;

            m_listener = null;

            Recycle();
        }
    }
}