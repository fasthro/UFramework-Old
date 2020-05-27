/*
 * @Author: fasthro
 * @Date: 2020-05-21 15:36:36
 * @Description: 登陆回调详情
 */
namespace FastEngine.Core
{
    public class LoginCallbackInfo : SDKCallbackInfo
    {
        public bool isSucceed;
        public bool isError;
        public bool isCancel;

        public string message;

        public LoginCallbackInfo(ChannelName channelName) : base(channelName)
        {
        }
    }
}