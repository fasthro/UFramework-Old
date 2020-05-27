/*
 * @Author: fasthro
 * @Date: 2020-05-09 14:32:31
 * @Description: SDK 编辑器窗口
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using FastEngine.Core;

namespace FastEngine.Editor.SDK
{
    public class SDKEditorWindow : FastEditorWindow
    {
        #region gui
        // 平台
        private int buildTarget;
        private string[] targetStrs = new string[] { "Develop", "Android", "iOS" };

        // 服务
        private int serviceTarget;
        private string[] serviceStrs = new string[] { "Login", "Pay", "DNA" };
        #endregion

        #region data
        // 配置
        private SDKConfig config;

        // 渠道类名称列表
        private string[] channelClassNames;

        // 渠道类型名称列表
        private string[] channelNames;

        // channel name value dict
        private Dictionary<string, int> channelNameValueDict;

        // 当前平台信息
        private SDKPlatformInfo platformInfo;
        // 当前服务信息
        private SDKServiceInfo serviceInfo;
        // 当前所选渠道索引
        private int channelIndex;

        #endregion

        protected override void OnInitialize()
        {
            titleContent.text = "SDK 配置编辑器";

            channelClassNames = GetChannelClassNames(typeof(SDKChannel));
            channelNames = GetChannelNames(channelClassNames);

            channelNameValueDict = GetChannelNameValueDict();
        }

        void OnGUI()
        {
            if (config == null) config = Config.ReadEditorDirectory<SDKConfig>();

            if (GUILayout.Button("Save"))
            {
                SetPlatform(config.develop);
                SetPlatform(config.android);
                SetPlatform(config.iOS);

                Config.Write<SDKConfig>(config);
                AssetDatabase.Refresh();
            }

            buildTarget = GUILayout.Toolbar(buildTarget, targetStrs);
            if (buildTarget == 0) platformInfo = config.develop;
            else if (buildTarget == 1) platformInfo = config.android;
            else if (buildTarget == 2) platformInfo = config.iOS;

            EditorGUILayout.BeginVertical("box");

            serviceTarget = GUILayout.Toolbar(serviceTarget, serviceStrs);
            if (serviceTarget == 0) serviceInfo = platformInfo.login;
            else if (serviceTarget == 1) serviceInfo = platformInfo.pay;
            else if (serviceTarget == 2) serviceInfo = platformInfo.dna;

            EditorGUILayout.BeginVertical("box");

            for (int i = 0; i < serviceInfo.channels.Count; i++)
            {
                var channel = serviceInfo.channels[i];
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    if (serviceTarget == 0)
                        channel.index = EditorGUILayout.Popup("Channel", channel.index, channelNames);
                    else if (serviceTarget == 1)
                        channel.index = EditorGUILayout.Popup("Channel", channel.index, channelNames);
                    else if (serviceTarget == 2)
                        channel.index = EditorGUILayout.Popup("Channel", channel.index, channelNames);

                    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Height(18)))
                    {
                        serviceInfo.channels.RemoveAt(i);
                        break;
                    }
                }
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                if (serviceTarget == 0 && channelNames.Length > 0)
                    serviceInfo.channels.Add(new SDKChannelInfo());
                else if (serviceTarget == 1 && channelNames.Length > 0)
                    serviceInfo.channels.Add(new SDKChannelInfo());
                else if (serviceTarget == 2 && channelNames.Length > 0)
                    serviceInfo.channels.Add(new SDKChannelInfo());
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 获取渠道类名称列表
        /// </summary>
        /// <param name="channelType"></param>
        /// <returns></returns>
        string[] GetChannelClassNames(Type channelType)
        {
            List<string> channels = new List<string>();
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(channelType))
                    channels.Add(types[i].Name);
            }
            return channels.ToArray();
        }

        /// <summary>
        /// 获取渠道名称列表
        /// </summary>
        /// <param name="classNames"></param>
        /// <returns></returns>
        string[] GetChannelNames(string[] classNames)
        {
            string[] channelNames = new string[classNames.Length];
            for (int i = 0; i < classNames.Length; i++)
            {
                channelNames[i] = classNames[i].Replace("ChannelSDK", "");
            }
            return channelNames;
        }

        /// <summary>
        /// 获取渠道枚举值字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, int> GetChannelNameValueDict()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var value in Enum.GetValues(typeof(ChannelName)))
            {
                dict.Add(value.ToString(), Convert.ToInt32(value));
            }
            return dict;
        }

        #region set channel
        /// <summary>
        /// 设置凭条配置
        /// </summary>
        /// <param name="platform"></param>
        void SetPlatform(SDKPlatformInfo platform)
        {
            platform.login.channels.ForEach((item) =>
            {
                SetLoginChannel(item);
            });
            platform.pay.channels.ForEach((item) =>
            {
                SetPayChannel(item);
            });
            platform.dna.channels.ForEach((item) =>
            {
                SetDNAChannel(item);
            });
        }

        void SetLoginChannel(SDKChannelInfo channel)
        {
            channel.channelName = (ChannelName)channelNameValueDict[channelNames[channel.index]];
            channel.className = channelClassNames[channel.index];
        }

        void SetPayChannel(SDKChannelInfo channel)
        {
            channel.channelName = (ChannelName)channelNameValueDict[channelNames[channel.index]];
            channel.className = channelNames[channel.index];
        }

        void SetDNAChannel(SDKChannelInfo channel)
        {
            channel.channelName = (ChannelName)channelNameValueDict[channelNames[channel.index]];
            channel.className = channelNames[channel.index];
        }
        #endregion
    }
}
