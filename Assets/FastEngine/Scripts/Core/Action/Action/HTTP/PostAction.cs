/*
 * @Author: fasthro
 * @Date: 2019-10-07 13:23:31
 * @Description: Post (Http)
 */
using System.Collections.Generic;
using System.Net;
using CI.HttpClient;

namespace FastEngine.Core
{
    public class PostAction : HttpActionBase
    {
        // field dictionary
        private Dictionary<string, string> m_fields;

        public PostAction(string url) : base(url)
        {
        }

        public PostAction AddField(string key, byte value)
        {
            return AddField(key, value.ToString());
        }

        public PostAction AddField(string key, int value)
        {
            return AddField(key, value.ToString());
        }

        public PostAction AddField(string key, float value)
        {
            return AddField(key, value.ToString());
        }

        public PostAction AddField(string key, long value)
        {
            return AddField(key, value.ToString());
        }

        public PostAction AddField(string key, string value)
        {
            if (m_fields == null) m_fields = new Dictionary<string, string>();
            if (!m_fields.ContainsKey(key)) m_fields.Add(key, value);
            return this;
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_sent) return;

            m_sent = true;

            m_client.Post(uri, new FormUrlEncodedContent(m_fields), HttpCompletionOption.AllResponseContent, (response) =>
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
    }
}