/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 国际化
 */
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class i18n
    {
        public static SystemLanguage language { get; private set; }
        private static Dictionary<int, string[]> modelDictonary = new Dictionary<int, string[]>();
        private static AssetBundleLoader resLoader;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="systemLanguage"></param>
        /// <param name="defaultLanguage">默认语言，找不到系统语言自动为系统语言</param>
        public static void Initialize(SystemLanguage systemLanguage, SystemLanguage defaultLanguage)
        {
            language = systemLanguage;
            // 中文默认就是中文简体
            if (language == SystemLanguage.Chinese)
            {
                language = SystemLanguage.ChineseSimplified;
            }
            // 是否使用默认语言
            if (!App.appConfig.supportedLanuages.Contains(language))
            {
                language = defaultLanguage;
            }
        }

        /// <summary>
        /// 获取多语言
        /// </summary>
        /// <param name="model">模块</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string Get(int model, int key)
        {
            string[] ls = null;
            if (modelDictonary.TryGetValue(model, out ls))
            {
                if (key >= 0 && key < ls.Length)
                {
                    return ls[key];
                }
            }
            else
            {
                BuildModel(model);
                if (modelDictonary.TryGetValue(model, out ls))
                {
                    if (key >= 0 && key < ls.Length)
                    {
                        return ls[key];
                    }
                }
            }
            Debug.LogError("i18n get error! model: " + model + " key: " + key);
            return "";
        }

        /// <summary>
        /// 构建模块
        /// </summary>
        /// <param name="model"></param>
        static void BuildModel(int model)
        {
            if (modelDictonary.ContainsKey(model)) return;

            string text = "";
            if (App.runModel == AppRunModel.Develop)
            {
                bool succeed = false;
                text = FilePathUtils.FileReadAllText(FilePathUtils.Combine(AppUtils.i18nDataDirectory(), language.ToString(), model + ".txt"), out succeed);
            }
            else
            {
                if (resLoader == null)
                {
                    resLoader = AssetBundleLoader.Allocate("language/" + language.ToString().ToLower(), null, null);
                    resLoader.LoadSync();
                }
                text = resLoader.bundleRes.assetBundle.LoadAsset<TextAsset>(model + ".txt").text;
            }
            modelDictonary.Add(model, text.Split('\n'));
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Release()
        {
            modelDictonary.Clear();

            if (resLoader != null)
            {
                resLoader.Unload();
                resLoader = null;
            }
        }
    }
}