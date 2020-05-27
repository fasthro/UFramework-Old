/*
 * @Author: fasthro
 * @Date: 2020-05-08 16:28:03
 * @Description: 数据分析服务
 */

using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class DNAService : SDKService
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        public virtual void Send(ChannelName channelName, string eventName)
        {
            IDNAChannel channel = GetChannel(channelName);
            if (channel != null) channel.SendEvent(eventName);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="eventName"></param>
        /// <param name="param"></param>
        public virtual void Send(ChannelName channelName, string eventName, ParamDictionary param)
        {
            IDNAChannel channel = GetChannel(channelName);
            if (channel != null) channel.SendEvent(eventName, param);
        }
    }
}
