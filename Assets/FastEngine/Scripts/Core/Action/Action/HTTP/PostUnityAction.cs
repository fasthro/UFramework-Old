/*
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
        private WWWForm m_field;
        private ByteArrayContent m_content;

        public PostUnityAction(string url, WWWForm field) : base(url)
        {
            m_field = field;
            m_content = new ByteArrayContent(m_field.data, m_field.headers["Content-Type"]);
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_sent) return;

            m_sent = true;

            m_client.Post(uri, m_content, HttpCompletionOption.AllResponseContent, (response) =>
           {
               if (response.IsSuccessStatusCode)
               {
                   BroadcastCallback<HttpResponseMessage>(ActionEvent.HttpSucceed, response);
               }
               else
               {
                   BroadcastCallback<HttpResponseMessage>(ActionEvent.HttpFailled, response);
               }
               isCompleted = true;
           });
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            m_field = null;
            m_content = null;
        }
    }
}