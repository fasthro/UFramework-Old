/*
 * @Author: fasthro
 * @Date: 2020-05-08 17:53:41
 * @Description: SDK配置
 */
using System.Collections.Generic;

namespace FastEngine.Core
{
    public class SDKConfig : ConfigObject
    {
        public SDKPlatformInfo develop { get; set; }
        public SDKPlatformInfo android { get; set; }
        public SDKPlatformInfo iOS { get; set; }

        protected override void OnInitialize()
        {
            if (develop == null)
            {
                develop = new SDKPlatformInfo();
                initPlatform(develop);
            }
            if (android == null)
            {
                android = new SDKPlatformInfo();
                initPlatform(android);
            }
            if (iOS == null)
            {
                iOS = new SDKPlatformInfo();
                initPlatform(iOS);
            }
        }

        void initPlatform(SDKPlatformInfo info)
        {
            if (info.login == null)
            {
                info.login = new SDKServiceInfo();
                initService(info.login);
            }
            if (info.pay == null)
            {
                info.pay = new SDKServiceInfo();
                initService(info.pay);
            }
            if (info.dna == null)
            {
                info.dna = new SDKServiceInfo();
                initService(info.dna);
            }
        }

        void initService(SDKServiceInfo info)
        {
            if (info.channels == null)
                info.channels = new List<SDKChannelInfo>();
        }
    }
}
