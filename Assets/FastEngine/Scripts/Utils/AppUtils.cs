/*
 * @Author: fasthro
 * @Date: 2019-11-27 10:57:12
 * @Description: App Utils
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using UnityEngine;

namespace FastEngine.Utils
{
    public static class AppUtils
    {
        /// <summary>
        /// 数据根目录
        /// </summary>
        /// <returns></returns>
        public static string DataRootDirectory()
        {
#if UNITY_EDITOR
            if (App.runModel == AppRunModel.Develop)
            {
                return Application.streamingAssetsPath;
            }
            else return FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "PersistentData");
#else
            return Application.persistentDataPath;
#endif
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppRawPath()
        {
#if UNITY_EDITOR
            return Application.streamingAssetsPath + "/";
#else
            if(Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/";
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return Application.dataPath + "/Raw/";
            }
            else return Application.streamingAssetsPath + "/";
#endif
        }

        /// <summary>
        /// Bundle根目录
        /// </summary>
        /// <returns></returns>
        public static string BundleRootDirectory()
        {
            if (!Application.isPlaying) return FilePathUtils.Combine(Application.streamingAssetsPath, PlatformUtils.PlatformId());
            return FilePathUtils.Combine(DataRootDirectory(), PlatformUtils.PlatformId());
        }

        /// <summary>
        /// 编辑器配置路径
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string EditorConfigPath(string configName)
        {
            return FilePathUtils.Combine(Application.dataPath, "EditorConfig", configName + ".json");
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string MD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string MD5File(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="path">配置路径</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadConfig<T>(string path) where T : new()
        {
            bool succeed = false;
            var context = FilePathUtils.FileReadAllText(path, out succeed);
            if (succeed) return JsonMapper.ToObject<T>(context);
            else return new T();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="context">配置文本内容</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadConfig2<T>(string context) where T : new()
        {
            return JsonMapper.ToObject<T>(context);
        }
    }
}