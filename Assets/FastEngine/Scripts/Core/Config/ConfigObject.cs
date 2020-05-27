/*
 * @Author: fasthro
 * @Date: 2020-05-22 20:41:51
 * @Description: json config 对象基类
 */

namespace FastEngine.Core
{
    public abstract class ConfigObject
    {
        public void Initialize() { OnInitialize(); }
        protected virtual void OnInitialize() { }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Save<T>() where T : ConfigObject, new()
        {
            Config.Write<T>(this);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="directory">目标目录</param>
        /// <typeparam name="T"></typeparam>
        public void Save<T>(string directory) where T : ConfigObject, new()
        {
            Config.Write<T>(this, directory);
        }
    }
}