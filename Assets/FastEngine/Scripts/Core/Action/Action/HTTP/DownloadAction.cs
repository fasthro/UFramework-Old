/*
 * @Author: fasthro
 * @Date: 2019-10-07 13:34:07
 * @Description: Download (Http)
 */
using System.Collections;
using System.Collections.Generic;
using System.Net;
using CI.HttpClient;
using UnityEngine;

namespace FastEngine.Core
{
    public class DownloadAction : HttpActionBase
    {
        // 下载数据缓存
        private byte[] m_downloadCache;
        // 下载数据写入游标
        private int m_downloadWriteSeek;

        public DownloadAction(string url) : base(url)
        {
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_sent) return;

            m_sent = true;

            m_client.Get(uri, HttpCompletionOption.StreamResponseContent, (response) =>
            {
                BroadcastCallback<int>(ActionEvent.HttpProgress, response.PercentageComplete);

                if (m_downloadCache == null)
                    m_downloadCache = new byte[response.ContentLength];

                var bs = response.ReadAsByteArray();
                bs.CopyTo(m_downloadCache, m_downloadWriteSeek);
                m_downloadWriteSeek += bs.Length;

                if (response.IsSuccessStatusCode)
                {
                    if (response.PercentageComplete >= 100)
                    {
                        BroadcastCallback<byte[]>(ActionEvent.HttpSucceed, m_downloadCache);
                        DownloadClose();
                        isCompleted = true;
                    }
                }
                else
                {
                    BroadcastCallback<HttpResponseMessage>(ActionEvent.HttpFailled, response);
                    DownloadClose();
                    isCompleted = true;
                }
            });
        }

        private void DownloadClose()
        {
            m_downloadCache = null;
            m_downloadWriteSeek = 0;
        }

        protected override void OnReset()
        {
            base.OnReset();
            DownloadClose();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            DownloadClose();
        }
    }
}
