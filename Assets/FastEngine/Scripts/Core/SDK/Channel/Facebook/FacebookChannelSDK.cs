/*
 * @Author: fasthro
 * @Date: 2020-05-08 19:58:19
 * @Description: Facebook登录
 */

#if SDK_CHANNEL_FACEBOOK
using System.Collections.Generic;
using Facebook.MiniJSON;
using Facebook.Unity;

namespace FastEngine.Core
{
    public class FacebookChannelSDK : SDKChannel
    {
        public FacebookChannelSDK(SDKChannelInfo info) : base(info)
        {
        }

        protected override void OnInitialize()
        {
            if (FB.IsInitialized)
            {
                OnInitComplete();
            }
            else
            {
                FB.Init(OnInitComplete, OnHideUnity);
            }
        }

        protected override void OnLogin()
        {
            if (FB.IsInitialized)
            {
                if (FB.IsLoggedIn)
                {
                    LoginSucceedRequestUserInfo();
                }
                else
                {
                    FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, OnHandleResult);
                }
            }
        }

        protected override void OnLogout()
        {
            if (FB.IsLoggedIn || FB.IsInitialized)
            {
                FB.LogOut();
            }
        }

        private void LoginSucceedRequestUserInfo()
        {
            FB.API("/me?fields=email,id,name", HttpMethod.GET, OnLoginSucceedRequestUserInfoResult);
        }

        private void OnInitComplete()
        {
            if (logEnabled)
            {
                log("Facebook SDK Init Complete");
            }
            FB.ActivateApp();
            OnInitializeComplete();
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (logEnabled)
            {
                log("Facebook SDK Init Hide Unity");
            }
        }

        private void OnHandleResult(IResult result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                // error
                if (logEnabled)
                {
                    log("Facebook SDK Handle Result Error: " + result.Error);
                }
                OnLoginError(result.Error);
            }
            else if (result.Cancelled)
            {
                // cancel
                if (logEnabled)
                {
                    log("Facebook SDK Handle Result Cancel!");
                }
                OnLoginCancel();
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                // succeed
                if (logEnabled)
                {
                    log("Facebook SDK Handle Result Succeed!");
                }
                LoginSucceedRequestUserInfo();
            }
            else
            {
                // error
                if (logEnabled)
                {
                    log("Facebook SDK Handle Result Error, Empty Response");
                }
                OnLoginError("Empty Response");
            }
        }

        private void OnLoginSucceedRequestUserInfoResult(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.RawResult))
            {
                // succeed
                if (logEnabled)
                {
                    log("Facebook SDK Handle Request UserInfo Result Succeed!" + result.RawResult);
                }
                LoginCallbackInfo info = new LoginCallbackInfo(channelInfo.channelName);
                var map = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
                if (map.ContainsKey("id"))
                {
                    info.AddParam("userId", map["id"]);
                }
                else info.AddParam("userId", "");

                if (map.ContainsKey("email"))
                {
                    info.AddParam("email", map["email"]);
                }
                else info.AddParam("email", "");

                if (map.ContainsKey("name"))
                {
                    info.AddParam("name", map["name"]);
                }
                else info.AddParam("name", "");

                OnLoginSucceed(info);
            }
            else
            {
                if (logEnabled)
                {
                    log("Facebook SDK Handle Request UserInfo Result Error!");
                }
                OnLoginError(result.Error);
            }
        }
    }
}
#endif