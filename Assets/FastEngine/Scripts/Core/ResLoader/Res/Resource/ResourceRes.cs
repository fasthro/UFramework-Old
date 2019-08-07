/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Resources 资源
 */
using System;
using System.Collections;
using UnityEngine;

namespace FastEngine.Core
{
    public class ResourceRes : Res, IPoolObject, IRunAsyncObject
    {
        public static ResourceRes Allocate(ResData data)
        {
            var res = ObjectPool<ResourceRes>.Instance.Allocate();
            res.Init(data);
            return res;
        }

        #region  IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResourceRes>.Instance.Recycle(this);
        }
        #endregion

        public void Init(ResData data)
        {
            m_assetName = data.assetName;
            m_state = ResState.Waiting;
            m_type = ResType.Resource;
            m_asset = null;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            m_state = ResState.Loading;
            m_asset = Resources.Load(m_assetName);
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
                m_state = ResState.Loading;
                RunAsync.Instance.Push(this);
            }
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <param name="async"></param>
        public IEnumerator AsyncRun(IRunAsync async)
        {
            var request = Resources.LoadAsync(m_assetName);

            yield return request;

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
            m_asset = null;
            
            Recycle();
        }
    }
}