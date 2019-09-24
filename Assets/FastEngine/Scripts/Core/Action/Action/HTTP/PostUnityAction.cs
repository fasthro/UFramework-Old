﻿/*
 * @Author: fasthro
 * @Date: 2019-10-07 13:23:31
 * @Description: Post Unity WWWForm (Http)
 */

using System.Net;
using CI.HttpClient;
using UnityEngine;

namespace FastEngine.Core
{
    public class PostUnityAction : HttpActionBase
    {
        private WWWForm m_wf;
        private ByteArrayContent m_content;

        public PostUnityAction(string url, WWWForm wf) : base(url)
        {
            m_wf = wf;
            m_content = new ByteArrayContent(m_wf.data, m_wf.headers["Content-Type"]);
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_sent) return;

            m_sent = true;

            m_client.Post(uri, m_content, HttpCompletionOption.AllResponseContent, (response) =>
           {
               if (response.IsSuccessStatusCode)
               {
                   BroadcastCallback<HttpResponseMessage>(ACTION_CALLBACK_TYPE.HTTP_SUCCEED, response);
               }
               else
               {
                   BroadcastCallback<HttpResponseMessage>(ACTION_CALLBACK_TYPE.HTTP_FAILLED, response);
               }
               isCompleted = true;
           });
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            m_wf = null;
            m_content = null;
        }
    }
}