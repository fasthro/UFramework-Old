/*
 * @Author: fasthro
 * @Date: 2019-10-07 13:35:29
 * @Description: 网络请求Action Base
 */

using System;
using CI.HttpClient;

namespace FastEngine.Core
{
    public abstract class HttpActionBase : ActionBase
    {
        // http client
        protected HttpClient m_client;

        // url
        protected string m_url;

        // uri
        private Uri m_uri;
        public Uri uri
        {
            get
            {
                if (m_uri == null) m_uri = new Uri(m_url);
                return m_uri;
            }
        }

        // sent request
        protected bool m_sent;

        public HttpActionBase(string url)
        {
            m_client = new HttpClient();
            m_url = url;
        }

        /// <summary>
        /// 超时
        /// </summary>
        /// <param name="timeout"></param>
        public void SetTimeout(int timeout)
        {
            m_client.Timeout = timeout;
        }

        protected override void OnReset()
        {
            m_sent = false;
            m_client.Abort();
        }

        protected override void OnDispose()
        {
            m_client.Abort();
            m_client = null;
            m_uri = null;
        }
    }
}