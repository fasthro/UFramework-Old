/*
 * @Author: fasthro
 * @Date: 2019-06-22 15:28:19
 * @Description: 资源基类
 */
using FastEngine.Utils;
using UnityEngine;

namespace FastEngine.Core
{
    /// <summary>
    /// 资源通知 Handler
    /// </summary>
    /// <param name="ready"></param>
    /// <param name="res"></param>
    public delegate void ResNotificationListener(bool ready, Res res);


    /// <summary>
    /// 资源基类
    /// </summary>
    public abstract class Res : IRef
    {
        // 资源名称
        protected string m_assetName;
        public string assetName { get { return m_assetName; } }

        // bundle名称
        protected string m_bundleName;
        public string bundleName { get { return m_bundleName; } }

        // 资源状态
        protected ResState m_state;
        public ResState state { get { return m_state; } }

        // 资源类型
        protected ResType m_type;
        public ResType type { get { return m_type; } }

        // 资源对象
        protected UnityEngine.Object m_asset;
        public UnityEngine.Object asset { get { return m_asset; } }

        // Bundle对象
        protected AssetBundle m_assetBundle;
        public AssetBundle assetBundle { get { return m_assetBundle; } }

        // 资源引用计数
        private int m_refCount = 0;
        public int refCount { get { return m_refCount; } }

        // 事件监听
        protected event ResNotificationListener m_notificationListener;

        /// <summary>
        /// 同步加载
        /// </summary>
        public abstract bool LoadSync();

        /// <summary>
        /// 异步加载
        /// </summary>
        public abstract void LoadAsync();

        /// <summary>
        /// 卸载资源
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// 获得资源
        /// </summary>
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if (m_asset == null)
               return null;
            return m_asset as T;
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="listener"></param>
        public void AddNotification(ResNotificationListener listener)
        {
            if (listener == null) return;
            m_notificationListener += listener;
        }

        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveNotification(ResNotificationListener listener)
        {
            if (listener == null) return;
            if (m_notificationListener == null) return;
            m_notificationListener -= listener;
        }

        /// <summary>
        /// 广播通知
        /// </summary>
        /// <param name="ready"></param>
        protected void Notification(bool ready)
        {
            m_notificationListener.InvokeGracefully(ready, this);
        }

        /// <summary>
        /// 引用次数为0处理
        /// </summary>
        protected abstract void OnZeroRef();

        #region 引用计数
        public void Retain()
        {
            ++m_refCount;
        }

        public void Release()
        {
            --m_refCount;
            if (m_refCount <= 0)
            {
                OnZeroRef();
            }
        }
        #endregion
    }
}