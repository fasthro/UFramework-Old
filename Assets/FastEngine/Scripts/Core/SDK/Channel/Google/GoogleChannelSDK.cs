/*
 * @Author: fasthro
 * @Date: 2020-05-08 19:58:19
 * @Description: 谷歌登录
 */
#if SDK_CHANNEL_GOOGLE

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine;

namespace FastEngine.Core
{
    public class GoogleChannelSDK : SDKChannel
    {
        public GoogleChannelSDK(SDKChannelInfo info) : base(info)
        {
        }
#if UNITY_ANDROID
        protected override void OnInitialize()
        {
            PlayGamesPlatform.DebugLogEnabled = Logger.logEnabled;

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestEmail()
            .RequestServerAuthCode(true)
            .RequestIdToken()
            .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            OnInitializeComplete();
        }

        protected override void OnLogin()
        {
            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool success, string message) =>
                {
                    if (success)
                    {
                        var user = (PlayGamesLocalUser)Social.localUser;
                        LoginCallbackInfo info = new LoginCallbackInfo(channelInfo.channelName);
                        info.AddParam("userId", user.id);
                        info.AddParam("email", user.Email);
                        info.AddParam("name", user.userName);
                        OnLoginSucceed(info);
                    }
                    else
                    {
                        OnLoginError(message);
                    }
                });
            }
            else
            {
                var user = (PlayGamesLocalUser)Social.localUser;
                LoginCallbackInfo info = new LoginCallbackInfo(channelInfo.channelName);
                info.AddParam("userId", user.id);
                info.AddParam("email", user.Email);
                info.AddParam("name", user.userName);
                OnLoginSucceed(info);
            }
        }

        protected override void OnLogout()
        {
            if (Social.localUser.authenticated)
                ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
        }
#endif
    }
}
#endif