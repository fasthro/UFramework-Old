/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 多语言
 */
using System.Collections.Generic;
using FastEngine.Utils;
using UnityEngine;

namespace FastEngine.Core
{

    [MonoSingletonPath("FastEngine/Localization")]
    public class Localization : MonoSingleton<Localization>
    {
        public SystemLanguage language { get; private set; }
        private Dictionary<string, string[]> m_modelMap = new Dictionary<string, string[]>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="systemLanguage"></param>
        /// <param name="defaultLanguage">默认语言，找不到系统语言自动为系统语言</param>
        private void InternalInitialize(SystemLanguage systemLanguage, SystemLanguage defaultLanguage)
        {
            language = systemLanguage;
            // 中文默认就是中文简体
            if (language == SystemLanguage.Chinese)
                language = SystemLanguage.ChineseSimplified;
            // 是否使用默认语言
            if (!App.appConfig.supportedLanuages.Contains(this.language))
                language = defaultLanguage;
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="model"></param>
        private void InternalLoadMode(string model)
        {
            if (m_modelMap.ContainsKey(model)) return;

            string text = "";
            if (App.runModel == AppRunModel.Develop)
            {
                bool succeed = false;
                var rootPath = FilePathUtils.Combine(Application.dataPath, "Localization/Language");
                var filePath = FilePathUtils.Combine(rootPath, language.ToString(), model + ".txt");
                text = FilePathUtils.FileReadAllText(filePath, out succeed);
            }
            else
            {
                var resPath = FilePathUtils.Combine("Localization/Language", language.ToString(), model);
                var loader = AssetBundleLoader.Allocate(resPath, null);
                var ready = loader.LoadSync();
                text = loader.assetRes.GetAsset<TextAsset>().text;
                loader.Unload();
                loader = null;
            }
            m_modelMap.Add(model, text.Split('\n'));
        }

        /// <summary>
        /// 获取多语言
        /// </summary>
        /// <param name="model">模块</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        private string InternalGet(string model, int key)
        {
            string[] ls = null;
            if (m_modelMap.TryGetValue(model, out ls))
            {
                if (key >= 0 && key < ls.Length) return ls[key];
            }
            else
            {
                InternalLoadMode(model);
                if (m_modelMap.TryGetValue(model, out ls))
                {
                    if (key >= 0 && key < ls.Length) return ls[key];
                }
            }
            Debug.LogError("language get error! model: " + model + " key: " + key);
            return "";
        }

        /// <summary>
        /// 释放
        /// </summary>
        private void InternalRelease()
        {
            m_modelMap.Clear();
        }

        #region API
        public static void Initialize(SystemLanguage language, SystemLanguage defaultLanguage) { Instance.InternalInitialize(language, defaultLanguage); }
        public static void Release() { Instance.InternalRelease(); }
        public static void LoadModel(string model) { Instance.InternalLoadMode(model); }
        public static string Get(string model, int key) { return Instance.InternalGet(model, key - 1); }
        #endregion
    }
}