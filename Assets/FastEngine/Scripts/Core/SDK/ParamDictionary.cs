/*
 * @Author: fasthro
 * @Date: 2020-05-21 19:54:27
 * @Description: Param Dictionary (参数字典)
 */

using System;
using System.Collections.Generic;
using LitJson;

namespace FastEngine.Core
{
    public class ParamDictionary
    {
        private Dictionary<string, object> m_params = new Dictionary<string, object>();

        public ParamDictionary() { }

        public ParamDictionary(ParamDictionary param)
        {
            this.m_params = new Dictionary<string, object>(param.m_params);
        }

        public ParamDictionary(Dictionary<string, object> param)
        {
            this.m_params = param;
        }

        /// <summary>
        /// Add Param
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ParamDictionary AddParam(string key, object value)
        {
            try
            {
                if (value is ParamDictionary)
                    m_params[key] = ((ParamDictionary)value).AsDictionary();
                else m_params[key] = value;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Key can not be null.", ex);
            }

            return this;
        }

        /// <summary>
        /// Get Param
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetParam(string key)
        {
            try
            {
                return m_params.ContainsKey(key) ? m_params[key] : null;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("Key can not be null.", ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new Exception("Key " + key + " not found.", ex);
            }
        }

        /// <summary>
        /// As Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> AsDictionary()
        {
            return m_params;
        }

        /// <summary>
        /// As Dictionary
        /// </summary>
        /// <returns></returns>
        public string AsJson()
        {
            return JsonMapper.ToJson(m_params);
        }
    }
}
