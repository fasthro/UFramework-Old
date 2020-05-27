/*
 * @Author: fasthro
 * @Date: 2020-05-08 19:22:27
 * @Description: SDK服务基类
 */

using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class SDKService
    {
        protected SDKServiceInfo m_serviceInfo;

        protected Dictionary<ChannelName, SDKChannel> channelDict;
        protected ChannelName[] channelNames;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="info"></param>
        public void Initialize(SDKServiceInfo info)
        {
            m_serviceInfo = info;

            var channelCount = m_serviceInfo.channels.Count;
            channelNames = new ChannelName[channelCount];
            channelDict = new Dictionary<ChannelName, SDKChannel>();
            for (int i = 0; i < channelCount; i++)
            {
                var channel = m_serviceInfo.channels[i];
                channelNames[i] = channel.channelName;
                channelDict.Add(channel.channelName, CreateChannel(channel));
            }

            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            if (channelNames != null)
            {
                for (int i = 0; i < channelNames.Length; i++)
                {
                    channelDict[channelNames[i]].Update();
                }
            }

            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        /// <summary>
        /// 获取渠道实例
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        protected SDKChannel GetChannel(ChannelName channelName)
        {
            SDKChannel channel;
            if (channelDict != null && channelDict.TryGetValue(channelName, out channel))
                return channel;
            return null;
        }

        protected static SDKChannel CreateChannel(SDKChannelInfo info)
        {
            SDKChannel channel = null;
            switch (info.channelName)
            {
                case ChannelName.Internal:
                    channel = new InternalChannelSDK(info);
                    break;
                case ChannelName.Google:
#if SDK_CHANNEL_GOOGLE
                    channel = new GoogleChannelSDK(info);
#endif
                    break;
                case ChannelName.Apple:
                    channel = new AppleChannelSDK(info);
                    break;
                case ChannelName.Facebook:
#if SDK_CHANNEL_FACEBOOK
                    channel = new FacebookChannelSDK(info);
#endif
                    break;
                case ChannelName.Kochava:
#if SDK_CHANNEL_KOCHAVA
                    channel = new KochavaChannelSDK(info);
#endif
                    break;
#if SDK_CHANNEL_DELTADNA
                case ChannelName.DeltaDNA:
                    channel = new DeltaDNAChannelSDK(info);
                    break;
#endif
            }
            return channel;
        }

        /// <summary>
        /// log
        /// </summary>
        /// <param name="message"></param>
        protected void log(object message)
        {
            if (Logger.logEnabled)
            {
                Debug.Log("[SDK] Service -> " + message.ToString());
            }
        }
    }
}
