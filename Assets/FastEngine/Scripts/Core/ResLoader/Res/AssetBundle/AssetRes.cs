/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Asset 资源
 */
using System;
using System.Collections;
using UnityEngine;

namespace FastEngine.Core
{
    public class AssetRes : Res, IPoolObject, IRunAsyncObject
    {
        private AssetBundleRequest m_request;
        private BundleRes m_bundleRes;

        public static AssetRes Allocate(ResData data)
        {
            var res = ObjectPool<AssetRes>.Instance.Allocate();
            res.Init(data);
            return res;
        }

        #region IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            // 移除缓存
            ResCache.Remove(ResData.AllocateAsset(assetName, bundleName));
            // 回收
            ObjectPool<AssetRes>.Instance.Recycle(this);
        }
        #endregion

        public void Init(ResData data)
        {
            m_bundleName = data.bundleName;
            m_assetName = data.assetName;
            m_state = ResState.Waiting;
            m_type = ResType.Asset;
            m_asset = null;
            m_assetBundle = null;
        }

        /// <summary>
        /// 搜索bundle
        /// </summary>
        private bool SearchAssetBundle()
        {
            if (m_assetBundle == null)
            {
                m_bundleRes = ResCache.Get<BundleRes>(ResData.AllocateBundle(bundleName));
                if (m_bundleRes != null)
                {
                    m_assetBundle = m_bundleRes.assetBundle;
                }
            }
            return m_assetBundle != null;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            if (m_state == ResState.Ready)
                return true;

            if (!SearchAssetBundle())
            {
                m_state = ResState.Failed;
                return false;
            }
            m_state = ResState.Loading;
            m_asset = m_assetBundle.LoadAsset(m_assetName);
            if (m_asset == null)
            {
                m_state = ResState.Failed;
                return false;
            }
            m_state = ResState.Ready;
            return true;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            if (m_state == ResState.Ready)
            {
                Notification(true);
            }
            else if (m_state == ResState.Waiting || m_state == ResState.Failed)
            {
                if (!SearchAssetBundle())
                {
                    m_state = ResState.Failed;
                    Notification(false);
                }
                else
                {
                    m_state = ResState.Loading;
                    RunAsync.Instance.Push(this);
                }
            }
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <param name="async"></param>
        public IEnumerator AsyncRun(IRunAsync async)
        {
            var request = m_assetBundle.LoadAssetAsync(m_assetName);

            m_request = request;
            yield return request;
            m_request = null;

            if (!request.isDone)
            {
                m_state = ResState.Failed;
                async.OnRunAsync();
                Notification(false);
                yield break;
            }

            m_asset = request.asset;

            if (m_asset == null)
            {
                m_state = ResState.Failed;
                async.OnRunAsync();
                Notification(false);
                yield break;
            }

            m_state = ResState.Ready;
            async.OnRunAsync();
            Notification(true);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            Release();
        }

        /// <summary>
        /// 引用次数为0处理
        /// </summary>
        protected override void OnZeroRef()
        {
            if (m_asset != null)
            {
                if (m_asset is GameObject) { }
                else Resources.UnloadAsset(m_asset);
            }

            if (m_bundleRes != null)
            {
                m_bundleRes.Unload();
                m_bundleRes = null;
            }

            m_assetBundle = null;
            m_asset = null;

            Recycle();
        }
    }
}