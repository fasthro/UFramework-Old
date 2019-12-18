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
using FastEngine.Utils;
using System.Collections.Generic;
using LitJson;

namespace FastEngine.Editor.Localization
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

        [MenuItem("FastEngine/Localization -> 打开配置", false, 200)]
        static void OpenConfig()
        {
            LocalizationCEW.Open<LocalizationCEW>();
        }

        [MenuItem("FastEngine/Localization -> 生成多语言", false, 201)]
        public static void Generate()
        {
            var filePath = FilePathUtils.Combine(Application.dataPath, "Localization/Localization.xlsx");
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("error", "Localization file does not exist!", "ok");
                return;
            }

            // 清空目录
            FilePathUtils.DirectoryDelete(RootPath);

            // 加载语言配置
            languages = AppUtils.LoadConfig<List<SystemLanguage>>(AppUtils.EditorConfigPath("LocalizationConfig"));

            // 读取解析语言文件
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    Sheet[] sheets = new Sheet[reader.ResultsCount];

                    var result = reader.AsDataSet();
                    for (int i = 0; i < reader.ResultsCount; i++)
                    {
                        var dataTable = result.Tables[i];
                        var columnCount = dataTable.Columns.Count;
                        var rowCount = dataTable.Rows.Count;

                        var sheet = new Sheet();
                        sheet.name = dataTable.TableName;
                        sheet.columns = new Column[rowCount];
                        for (int rc = 0; rc < rowCount; rc++)
                        {
                            sheet.columns[rc] = new Column();
                        }

                        for (int r = 1; r < rowCount; r++)
                        {
                            sheet.columns[r].values = new string[columnCount];
                            for (int c = 0; c < columnCount; c++)
                            {
                                var context = dataTable.Rows[r][c].ToString();
                                if (c == 0)
                                {
                                    sheet.columns[r].key = context;
                                }
                                else
                                {
                                    sheet.columns[r].values[c] = context;
                                }
                            }
                        }

                        // 生成模块多语言
                        GenerateSheetValue(sheet);
                        sheets[i] = sheet;
                    }
                    // 生成key
                    GenerateLua(sheets);
                    GenerateCSharp(sheets);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    Debug.Log("export language succeed!");
                }
            }
        }

        /// <summary>
        /// 生成Luakey
        /// </summary>
        /// <param name="sheets"></param>
        static void GenerateLua(Sheet[] sheets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("--[[ aotu generated]]");
            builder.AppendLine("--[[");
            builder.AppendLine(" * @Author: fasthro");
            builder.AppendLine(" * @Description: 多语言key");
            builder.AppendLine(" ]]");

            // model
            builder.AppendLine("language_model = {");
            for (int i = 0; i < sheets.Length; i++)
            {
                builder.AppendLine(string.Format("\t{0} = \"{1}\",", sheets[i].name, sheets[i].name));
            }
            builder.AppendLine("}");

            // key
            builder.AppendLine("language_key = {");

            for (int i = 0; i < sheets.Length; i++)
            {
                builder.AppendLine(sheets[i].ToLuaKeyString());
            }
            builder.AppendLine("}");
            FilePathUtils.FileWriteAllText(LuaKeyPath, builder.ToString());
        }

        /// <summary>
        /// 生成C#key
        /// </summary>
        /// <param name="sheets"></param>
        static void GenerateCSharp(Sheet[] sheets)
        {
            StringBuilder builder = new StringBuilder();
            // model
            builder.AppendLine("public static class LanguageModel");
            builder.AppendLine("{");

            for (int i = 0; i < sheets.Length; i++)
            {
                builder.AppendLine(string.Format("\tpublic static string {0} = \"{1}\";", sheets[i].name, sheets[i].name));
            }
            builder.AppendLine("}");

            // key
            builder.AppendLine("public static class LanguageKey");
            builder.AppendLine("{");

            for (int i = 0; i < sheets.Length; i++)
            {
                builder.AppendLine(sheets[i].ToCSharpKeyString());
            }
            builder.AppendLine("}");
            FilePathUtils.FileWriteAllText(CSharpKeyPath, builder.ToString());
        }

        /// <summary>
        /// 生成多语言
        /// </summary>
        /// <param name="sheet"></param>
        static void GenerateSheetValue(Sheet sheet)
        {
            for (int i = 0; i < languages.Count; i++)
            {
                var path = FilePathUtils.Combine(RootPath, languages[i].ToString(), sheet.name + ".txt");
                FilePathUtils.FileWriteAllText(path, sheet.ToValueString(i + 1));
            }
        }
    }
}