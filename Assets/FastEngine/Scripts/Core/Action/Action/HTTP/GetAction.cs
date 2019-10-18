/*
 * @Author: fasthro
 * @Date: 2019-10-07 13:23:31
 * @Description: Get (Http)
 */
 
using System.Net;
using CI.HttpClient;

namespace FastEngine.Core
{
    public class GetAction : HttpActionBase
    {
        public GetAction(string url) : base(url)
        {
        }

        protected override void OnExecute(float deltaTime)
        {
            if (m_sent) return;

            m_sent = true;

            m_client.Get(uri, HttpCompletionOption.AllResponseContent, (response) =>
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
    }
}