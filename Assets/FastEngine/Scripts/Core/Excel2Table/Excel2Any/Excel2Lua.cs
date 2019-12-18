/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 lua code
 */
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public class Excel2Lua : Excel2Any
    {
        static string template = @"-- FastEngine
-- excel2table auto generate
-- $descriptions$
local datas = {
$line$
}
return datas
";
        private StringBuilder m_stringBuilder = new StringBuilder();
        private StringBuilder m_wrapStringBuilder = new StringBuilder();
        public Excel2Lua(ExcelReader reader) : base(reader)
        {
            // descriptions
            m_stringBuilder.Clear();
            for (int i = 0; i < reader.descriptions.Count; i++)
            {
                m_stringBuilder.Append(reader.descriptions[i] + ",");
            }
            template = template.Replace("$descriptions$", m_stringBuilder.ToString());

            // data0
            m_stringBuilder.Clear();
            for (int i = 0; i < reader.rows.Count; i++)
            {
                m_stringBuilder.Append("\t[" + i + "] = {");
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    if (k == reader.fields.Count - 1) { m_stringBuilder.Append(string.Format("{0} = {1} ", reader.fields[k], WrapContext(reader.rows[i].datas[k], reader.types[k]))); }
                    else { m_stringBuilder.Append(string.Format("{0} = {1}, ", reader.fields[k], WrapContext(reader.rows[i].datas[k], reader.types[k]))); }
                }
                if (i == reader.rows.Count - 1) { m_stringBuilder.Append("}\r\n"); }
                else { m_stringBuilder.Append("},\r\n"); }
            }
            template = template.Replace("$line$", m_stringBuilder.ToString());
            FilePathUtils.FileWriteAllText(reader.options.luaOutFilePath, template);
        }

        private string WrapContext(string context, FieldType type)
        {
            switch (type)
            {
                case FieldType.Boolean:
                    return TypeUtils.ContentToBooleanValue(context) ? "true" : "false";

                case FieldType.String:
                    return string.Format("\"{0}\"", context);

                case FieldType.Vector2:
                    Vector2 v2 = TypeUtils.ContentToVector2Value(context);
                    return string.Format("Vector2.New({0}, {1})", v2.x, v2.y);

                case FieldType.Vector3:
                    Vector3 v3 = TypeUtils.ContentToVector3Value(context);
                    return string.Format("Vector3.New({0}, {1}, {2})", v3.x, v3.y, v3.z);

                case FieldType.ArrayByte:
                case FieldType.ArrayInt:
                case FieldType.ArrayLong:
                case FieldType.ArrayFloat:
                case FieldType.ArrayDouble:
                    return "{" + context + "}";

                case FieldType.ArrayBoolean:
                    m_wrapStringBuilder.Clear();
                    m_wrapStringBuilder.Append("{");
                    bool[] bools = TypeUtils.ContentToArrayBooleanValue(context);
                    for (int i = 0; i < bools.Length; i++)
                    {
                        var bs = bools[i] ? "true" : "false";
                        if (i == bools.Length - 1) { m_wrapStringBuilder.Append(bs); }
                        else { m_wrapStringBuilder.Append(bs + ", "); }
                    }
                    m_wrapStringBuilder.Append("}");
                    return m_wrapStringBuilder.ToString();
                case FieldType.ArrayString:
                    m_wrapStringBuilder.Clear();
                    m_wrapStringBuilder.Append("{");
                    string[] strs = TypeUtils.ContentToArrayStringValue(context);
                    for (int i = 0; i < strs.Length; i++)
                    {
                        var str = string.Format("\"{0}\"", strs[i]);
                        if (i == strs.Length - 1) { m_wrapStringBuilder.Append(str); }
                        else { m_wrapStringBuilder.Append(str + ", "); }
                    }
                    m_wrapStringBuilder.Append("}");
                    return m_wrapStringBuilder.ToString();

                case FieldType.ArrayVector2:
                    m_wrapStringBuilder.Clear();
                    m_wrapStringBuilder.Append("{");
                    Vector2[] v2s = TypeUtils.ContentToArrayVector2Value(context);
                    for (int i = 0; i < v2s.Length; i++)
                    {
                        var v2str = string.Format("Vector2.New({0}, {1})", v2s[i].x, v2s[i].y);
                        if (i == v2s.Length - 1) { m_wrapStringBuilder.Append(v2str); }
                        else { m_wrapStringBuilder.Append(v2str + ", "); }
                    }
                    m_wrapStringBuilder.Append("}");
                    return m_wrapStringBuilder.ToString();

                case FieldType.ArrayVector3:
                    m_wrapStringBuilder.Clear();
                    m_wrapStringBuilder.Append("{");
                    Vector3[] v3s = TypeUtils.ContentToArrayVector3Value(context);
                    for (int i = 0; i < v3s.Length; i++)
                    {
                        var v3str = string.Format("Vector3.New({0}, {1}, {2})", v3s[i].x, v3s[i].y, v3s[i].z);
                        if (i == v3s.Length - 1) { m_wrapStringBuilder.Append(v3str); }
                        else { m_wrapStringBuilder.Append(v3str + ", "); }
                    }
                    m_wrapStringBuilder.Append("}");
                    return m_wrapStringBuilder.ToString();

                default:
                    return context;
            }
        }
    }
}