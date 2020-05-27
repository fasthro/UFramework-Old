/*
 * @Author: fasthro
 * @Date: 2020-05-22 14:41:30
 * @Description: CallbackInfo
 */

using System.Collections.Generic;

namespace FastEngine.Core
{
    public abstract class SDKCallbackInfo
    {
        // 渠道名称
        public ChannelName channelName { get; protected set; }

        // 参数字典
        protected ParamDictionary m_paramDictionary;

        public SDKCallbackInfo(ChannelName channelName)
        {
            this.channelName = channelName;
        }

        public void AddParam(ParamDictionary param)
        {
            AddParam(param.AsDictionary());
        }

        public void AddParam(Dictionary<string, object> param)
        {
            if (param != null)
            {
                param.ForEach<string, object>((key, value) =>
                {
                    AddParam(key, value);
                });
            }
        }

        public void AddParam(string key, object value)
        {
            if (m_paramDictionary == null)
                m_paramDictionary = new ParamDictionary();

            m_paramDictionary.AddParam(key, value);
        }

        public T GetParam<T>(string key)
        {
            object value = m_paramDictionary.GetParam(key);
            if (value != null)
                return (T)value;
            return default;
        }
    }
}
