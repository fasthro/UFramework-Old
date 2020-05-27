/*
 * @Author: fasthro
 * @Date: 2020-05-08 20:11:54
 * @Description: 游戏配置
 */

using UnityEngine;
using System.Collections.Generic;
using LitJson;

namespace FastEngine.Core
{
    public static class Config
    {
        // 配置缓存字典
        static Dictionary<string, ConfigObject> configDict = new Dictionary<string, ConfigObject>();

        /// <summary>
        /// 读取Data目录配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ReadDataDirectory<T>() where T : ConfigObject, new()
        {
#if UNITY_EDITOR
            return ReadEditorDirectory<T>();
#else
            string cn = typeof(T).Name;

            ConfigObject co = null;
            if (configDict.TryGetValue(cn, out co))
                return (T)co;

            var cp = FilePathUtils.Combine(AppUtils.ConfigDataDirectory(), cn + ".json");
            bool succeed = false;
            return Parse<T>(FilePathUtils.FileReadAllText(cp, out succeed));
#endif
        }

        /// <summary>
        /// 读取Resource目录配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ReadResourceDirectory<T>() where T : ConfigObject, new()
        {
#if UNITY_EDITOR
            return ReadEditorDirectory<T>();
#else
            string cn = typeof(T).Name;

            ConfigObject co = null;
            if (configDict.TryGetValue(cn, out co))
                return (T)co;

            var cp = FilePathUtils.Combine(AppUtils.ConfigResourceDirectory(), cn);
            var textAsset = Resources.Load<TextAsset>(cp);
            string content = textAsset.text;
            Resources.UnloadAsset(textAsset);
            return Parse<T>(content);
#endif
        }

        /// <summary>
        /// 读取编辑器目录配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ReadEditorDirectory<T>() where T : ConfigObject, new()
        {
            string cn = typeof(T).Name;

            ConfigObject co = null;
            if (configDict.TryGetValue(cn, out co))
                return (T)co;

            var cp = FilePathUtils.Combine(AppUtils.ConfigEditorDirectory(), cn + ".json");
            bool succeed = false;
            return Parse<T>(FilePathUtils.FileReadAllText(cp, out succeed));
        }


        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public static void Write<T>(object data)
        {
#if UNITY_EDITOR
            var cp = FilePathUtils.Combine(AppUtils.ConfigEditorDirectory(), typeof(T).Name + ".json");
            FilePathUtils.FileWriteAllText(cp, JsonMapper.ToJson(data));
#endif
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="data"></param>
        /// <param name="directory">目标目录</param>
        /// <typeparam name="T"></typeparam>
        public static void Write<T>(object data, string directory)
        {
#if UNITY_EDITOR
            var cp = FilePathUtils.Combine(directory, typeof(T).Name + ".json");
            FilePathUtils.FileWriteAllText(cp, JsonMapper.ToJson(data));
#endif
        }

        /// <summary>
        /// 解析对象
        /// </summary>
        /// <param name="content"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Parse<T>(string content) where T : ConfigObject, new()
        {
            T obj;
            if (!string.IsNullOrEmpty(content))
                obj = JsonMapper.ToObject<T>(content);
            else obj = new T();
            obj.Initialize();
            return obj;
        }
    }
}