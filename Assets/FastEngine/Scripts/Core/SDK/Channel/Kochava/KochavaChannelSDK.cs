/*
 * @Author: fasthro
 * @Date: 2020-05-21 14:33:44
 * @Description: Kochava 数据采集
 */
#if SDK_CHANNEL_KOCHAVA
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class KochavaChannelSDK : SDKChannel
    {
        // 已经接受到 attribution
        private bool m_attributionReceived = false;

        // 归因数据
        static Dictionary<string, object> attributionDictionary;

        public KochavaChannelSDK(SDKChannelInfo info) : base(info)
        {
            Initialize();
        }

        protected override void OnInitialize()
        {
            // initialize Kochava using the configuration provided
            // NOTE: If you wish to initialize Kochava elsewhere (in your own code) please consult the documentation for examples at https://support.kochava.com/sdk-integration/unity-sdk-integration
            string platformAppGUID = string.Empty;
#if UNITY_IPHONE && !UNITY_EDITOR
            platformAppGUID = "korise-of-warfare-ios-zkbyhezw";
#elif UNITY_ANDROID && !UNITY_EDITOR
            platformAppGUID = "korise-of-warfare-android-tfa";
#endif
            Kochava.Tracker.Config.SetLogLevel(Kochava.DebugLogLevel.info);
            Kochava.Tracker.Config.SetAppGuid(platformAppGUID);
            Kochava.Tracker.Config.SetRetrieveAttribution(true);
            Kochava.Tracker.Config.SetIntelligentConsentManagement(false);
            Kochava.Tracker.Config.SetPartnerName("");
            Kochava.Tracker.Config.SetPartnerApp("");
            Kochava.Tracker.Config.SetSleep(false);

            // To prevent overriding values set using the setter we only apply this value once on the first launch.
            bool latSet = PlayerPrefs.HasKey("kou__latset");
            if (!latSet)
            {
                Kochava.Tracker.Config.SetAppLimitAdTracking(false);
                PlayerPrefs.SetInt("kou__latset", 1);
            }

            Kochava.Tracker.Initialize();

            Kochava.Tracker.SetAttributionHandler(OnAttributionCallback);

            // 尝试获取归因数据
            WaitAttributionCallback();

            OnInitializeComplete();
        }

        protected override void OnSendEvent(string eventName)
        {
            Kochava.Tracker.SendEvent(new Kochava.Event(eventName));
        }

        protected override void OnSendEvent(string eventName, ParamDictionary param)
        {
            Kochava.Event optionsEvent = new Kochava.Event(eventName);
            param.AsDictionary().ForEach<string, object>((key, value) =>
            {
                optionsEvent.SetCustomValue(key, value.ToString());
            });
            Kochava.Tracker.SendEvent(optionsEvent);
        }

        /// <summary>
        /// 归因数据回调
        /// </summary>
        /// <param name="att"></param>
        private void OnAttributionCallback(string attribution)
        {
            log("归因数据: " + attribution);

            if (attribution.Length > 0)
            {
                if (!m_attributionReceived)
                {
                    m_attributionReceived = true;
                    attributionDictionary = JsonFx.Json.JsonReader.Deserialize<Dictionary<string, object>>(attribution);

                    DNACallbackInfo info = new DNACallbackInfo(channelInfo.channelName);
                    info.AddParam(attributionDictionary);
                    SDKCallback.Broadcast(SDKCallbackEvent.DNA, info);
                }
            }
            else WaitAttributionCallback();
        }

        /// <summary>
        /// 等待归因
        /// </summary>
        private void WaitAttributionCallback()
        {
            var delay = new DelayAction(60);
            delay.BindCallback(ActionEvent.Completed, () =>
            {
                OnAttributionCallback(Kochava.Tracker.GetAttribution());
            });
            delay.Start();
        }
    }
}
#endif