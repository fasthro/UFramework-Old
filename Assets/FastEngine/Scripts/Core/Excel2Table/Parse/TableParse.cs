/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse
 */
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public abstract class TableParse
    {
        protected string m_tableName;
        protected FormatOptions m_format;
        protected string m_content;

        public TableParse(string tableName, FormatOptions format) { m_tableName = tableName; }

        protected void LoadAsset()
        {
            if (!string.IsNullOrEmpty(m_content)) return;
            if (App.runModel == AppRunModel.Develop)
            {
                var filePath = FilePathUtils.Combine(AppUtils.TableDataDirectory(), m_tableName + ".csv");
                bool succeed = false;
                m_content = FilePathUtils.FileReadAllText(filePath, out succeed);
            }
            else
            {
                var loader = AssetBundleLoader.Allocate(FilePathUtils.Combine(AppUtils.TableDataBundleRootDirectory(), m_tableName), null);
                loader.LoadSync();
                m_content = loader.assetRes.GetAsset<TextAsset>().text;
                loader.Unload();
                loader = null;
            }
        }

        public abstract T[] ParseArray<T>();
        public abstract Dictionary<string, T> ParseStringDictionary<T>();
        public abstract Dictionary<int, T> ParseIntDictionary<T>();
        public abstract Dictionary<int, Dictionary<int, T>> ParseInt2IntDictionary<T>();
    }
}