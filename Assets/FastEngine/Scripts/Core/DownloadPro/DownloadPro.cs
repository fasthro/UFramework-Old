/*
 * @Author: fasthro
 * @Date: 2019-11-30 12:13:19
 * @Description: 下载文件(支持断点续传，支持多文件同时下载)
 */
using System.Collections;
using System.Collections.Generic;
using FastEngine;
using UnityEngine.Networking;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/DownloadPro")]
    public class DownloadPro : MonoSingleton<DownloadPro>
    {
        // 并行下载数量(默认支持2个同时下载)
        private int m_multiDownloadCount;
        // 当前下载任务数量
        private int m_downloadCount;
        // request map
        private Dictionary<string, UnityWebRequest> m_requestDic;
        // wait download map<url, DownloadHandlerPro>
        private Dictionary<string, DownloadHandlerPro> m_waitDic;

        public override void InitializeSingleton()
        {
            this.m_downloadCount = 0;
            this.m_multiDownloadCount = 2;
            m_requestDic = new Dictionary<string, UnityWebRequest>();
            m_waitDic = new Dictionary<string, DownloadHandlerPro>();
        }

        /// <summary>
        /// 设置并行下载数量
        /// </summary>
        /// <param name="multiDownloadCount">并行下载数量</param>
        public void SetMultiDownloadCount(int multiDownloadCount)
        {
            this.m_multiDownloadCount = multiDownloadCount;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        public DownloadHandlerPro Download(string url, string savePath)
        {
            if (m_requestDic.ContainsKey(url))
                return m_requestDic[url].downloadHandler as DownloadHandlerPro;
            if (m_waitDic.ContainsKey(url))
                return m_waitDic[url];

            var handler = new DownloadHandlerPro(url, savePath);
            if (m_downloadCount < m_multiDownloadCount) CoroutineFactory.CreateAndStart(DownloadAsync(handler));
            else m_waitDic.Add(url, handler);
            return handler;
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        public void CancelDownload(string url)
        {

        }

        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="handler"></param>
        public void CancelDownload(DownloadHandlerPro handler)
        {

        }

        /// <summary>
        /// 异步下载
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IEnumerator DownloadAsync(DownloadHandlerPro handler)
        {
            m_downloadCount++;
            var request = UnityWebRequest.Get(handler.url);
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            request.SetRequestHeader("Range", handler.headerRangeValue);
            request.downloadHandler = handler;
            handler.Download();
            yield return request.SendWebRequest();
            DownloadFinished(request);
        }

        /// <summary>
        /// 下载完成进行处理
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        private void DownloadFinished(UnityWebRequest request)
        {
            var handler = request.downloadHandler as DownloadHandlerPro;
            if (request.isDone)
            {
                m_requestDic.Remove(handler.url);
                handler.Release();
                request.Dispose();
            }

            m_downloadCount--;

            foreach (string key in m_waitDic.Keys)
            {
                CoroutineFactory.CreateAndStart(DownloadAsync(m_waitDic[key]));
                m_waitDic.Remove(key);
                break;
            }
        }
    }
}