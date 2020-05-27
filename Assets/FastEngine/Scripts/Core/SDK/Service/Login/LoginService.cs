/*
 * @Author: fasthro
 * @Date: 2020-05-08 16:28:09
 * @Description: 登录访问
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using LitJson;
using UnityEngine;

namespace FastEngine.Core
{
    public class LoginService : SDKService
    {

        /// <summary>
        /// 渠道登录
        /// </summary>
        /// <param name="channelName"></param>
        public void Login(ChannelName channelName)
        {
            ILoginChannel channel = GetChannel(channelName);
            if (channel != null) channel.Login();
        }

        /// <summary>
        /// 渠道登出
        /// </summary>
        /// <param name="channelName"></param>
        public void Logout(ChannelName channelName)
        {
            ILoginChannel channel = GetChannel(channelName);
            if (channel != null) channel.Logout();
        }

        /// <summary>
        /// 登陆回调
        /// </summary>
        /// <param name="info"></param>
        public void OnLogin(LoginCallbackInfo info)
        {
            SDKCallback.Broadcast(SDKCallbackEvent.LOGIN, info);
        }

        /// <summary>
        /// 登出回调
        /// </summary>
        /// <param name="info"></param>
        public void OnLogout(LoginCallbackInfo info)
        {
            SDKCallback.Broadcast(SDKCallbackEvent.LOGIN, info);
        }
    }
}

