/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 csv
 */
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public class Excel2CSV : Excel2Any
    {
        private StringBuilder m_stringBuilder = new StringBuilder();

        public Excel2CSV(ExcelReader reader) : base(reader)
        {
            m_stringBuilder.Clear();

            // fields
            m_stringBuilder.Clear();
            for (int i = 0; i < reader.fields.Count; i++)
            {
                if (i == reader.fields.Count - 1) { m_stringBuilder.Append(reader.fields[i]); }
                else { m_stringBuilder.Append(reader.fields[i] + ","); }
            }
            m_stringBuilder.Append("\r\n");

            // data
            for (int i = 0; i < reader.rows.Count; i++)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    if (k == reader.fields.Count - 1) { m_stringBuilder.Append(WrapContext(reader.rows[i].datas[k], reader.types[k])); }
                    else { m_stringBuilder.Append(WrapContext(reader.rows[i].datas[k], reader.types[k]) + ","); }
                }
                m_stringBuilder.Append("\r\n");
            }
            FilePathUtils.FileWriteAllText(reader.options.dataOutFilePath, m_stringBuilder.ToString());
        }

        private string WrapContext(string content, FieldType type)
        {
            switch (type)
            {
                case FieldType.Byte:
                case FieldType.Int:
                case FieldType.Long:
                case FieldType.Float:
                case FieldType.Double:
                case FieldType.Boolean:
                    return content;
                case FieldType.i18n:
                    return WrapI18nContext(content);
                default:
                    return string.Format("\"{0}\"", content);
            }
        }

        private string WrapI18nContext(string content)
        {
            char[] separator = new char[] { ':' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            var model = typeof(LanguageModel);
            var modelField = model.GetField(datas[0]);

            var key = typeof(LanguageKey);
            var keyField = key.GetField(string.Format("{0}_{1}", datas[0], datas[1]));

            if (modelField != null && keyField != null)
            {
                return string.Format("{0}:{1}", (int)modelField.GetValue(null), (int)keyField.GetValue(null));
            }
            Debug.LogError("table not find i18n: " + datas[0] + ":" + datas[1]);
            return "";
        }
    }
}