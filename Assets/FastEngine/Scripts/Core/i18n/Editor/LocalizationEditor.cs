/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:43:01
 * @Description: 多语言编辑器
 */
using System.Data;
using System.IO;
using ExcelDataReader;
using FastEngine.Core;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;
using LitJson;
using FastEngine.Core.I18n;

namespace FastEngine.Editor.I18n
{
    public class LocalizationEditor
    {
        // 多语言 -> 生成多语言根目录
        static string RootPath { get { return FilePathUtils.Combine(Application.dataPath, "Localization/Language"); } }
        // 多语言 -> Lua 键值路径
        static string LuaKeyPath { get { return FilePathUtils.Combine(Application.dataPath, "LuaScripts/language.lua"); } }
        // 多语言 -> CSarp 键值路径
        static string CSharpKeyPath { get { return FilePathUtils.Combine(Application.dataPath, "Scripts/Language.cs"); } }
        static List<SystemLanguage> languages;

        [MenuItem("FastEngine/i18n -> 打开配置", false, 200)]
        static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<EditorWindow>();
        }

        [MenuItem("FastEngine/i18n -> 生成数据", false, 201)]
        public static void Generate()
        {
            var opt = new ExcelReaderOptions();
            opt.languages = AppUtils.ReadEditorConfig<i18nConfig>().languages;
            var reader = new ExcelReader(opt);
            reader.Read();

            new Excel2Text(reader);
            new Excel2Index(reader);
            new Excel2LuaIndex(reader);

            AssetDatabase.Refresh();

            Debug.Log("i18n generate succeed!");
        }
    }
}