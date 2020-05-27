/*
 * @Author: fasthro
 * @Date: 2020-05-22 19:57:10
 * @Description: 基础配置
 */
using System.Collections.Generic;

namespace FastEngine.Core
{
    /// <summary>
    /// 配置地址
    /// </summary>
    public enum ConfigAddress
    {
        Editor,
        Resource,
        Data,
    }

    public class BaseConfig : ConfigObject
    {
        public Dictionary<string, ConfigAddress> map { get; set; }

        protected override void OnInitialize()
        {
            if (map == null)
            {
                map = new Dictionary<string, ConfigAddress>();
            }
        }
    }
}
