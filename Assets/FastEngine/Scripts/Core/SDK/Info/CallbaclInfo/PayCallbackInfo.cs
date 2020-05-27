/*
 * @Author: fasthro
 * @Date: 2020-05-21 15:36:45
 * @Description: 支付回调详情
 */ 
namespace FastEngine.Core
{
    public class PayCallbackInfo : SDKCallbackInfo
    {
        public PayCallbackInfo(ChannelName channelName) : base(channelName)
        {
        }
    }
}