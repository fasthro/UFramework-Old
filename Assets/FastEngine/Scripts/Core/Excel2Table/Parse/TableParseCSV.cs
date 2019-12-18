/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse csv
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public class TableParseCSV : TableParse
    {
        static char[] lineSeparator = new char[] { '\r', '\n' };
        static char[] separator = new char[] { ',' };

        private Type _modelType;
        private string[] m_lines;
        private FieldInfo[] m_fields;

        public TableParseCSV(string tableName) : base(tableName, FormatOptions.CSV) { LoadAsset(); }

        /// <summary>
        /// 构建数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void BuildData<T>()
        {
            _modelType = typeof(T);
            // read lines
            m_lines = m_content.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);

            // create field info
            var _fiedls = m_lines[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);
            m_fields = new FieldInfo[_fiedls.Length];
            for (int i = 0; i < _fiedls.Length; i++)
                m_fields[i] = _modelType.GetField(_fiedls[i]);

        }

        /// <summary>
        /// 解析为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T[] ParseArray<T>()
        {
            BuildData<T>();
            T[] array = new T[m_lines.Length - 1];
            for (int i = 1; i < m_lines.Length; i++)
                array[i - 1] = CreateInstance<T>(ParseCSVLine(m_lines[i]));

            return array;
        }

        /// <summary>
        /// 解析为字典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override Dictionary<string, T> ParseStringDictionary<T>()
        {
            BuildData<T>();
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            string[] lc = null;
            for (int i = 1; i < m_lines.Length; i++)
            {
                lc = ParseCSVLine(m_lines[i]);
                dictionary.Add(lc[0], CreateInstance<T>(lc));
            }
            return dictionary;
        }

        /// <summary>
        /// 解析为字典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override Dictionary<int, T> ParseIntDictionary<T>()
        {
            BuildData<T>();
            Dictionary<int, T> dictionary = new Dictionary<int, T>();
            string[] lc = null;
            for (int i = 1; i < m_lines.Length; i++)
            {
                lc = ParseCSVLine(m_lines[i]);
                dictionary.Add(int.Parse(lc[0]), CreateInstance<T>(lc));
            }
            return dictionary;
        }

        /// <summary>
        /// 解析为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override Dictionary<int, Dictionary<int, T>> ParseInt2IntDictionary<T>()
        {
            BuildData<T>();
            Dictionary<int, Dictionary<int, T>> dictionary = new Dictionary<int, Dictionary<int, T>>();
            string[] lc = null;
            for (int i = 1; i < m_lines.Length; i++)
            {
                lc = ParseCSVLine(m_lines[i]);
                var k = int.Parse(lc[0]);
                var l = int.Parse(lc[1]);
                if (!dictionary.ContainsKey(k))
                {
                    dictionary[k] = new Dictionary<int, T>();
                }
                dictionary[k].Add(l, CreateInstance<T>(lc));
            }
            return dictionary;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private T CreateInstance<T>(string[] line)
        {
            T obj = Activator.CreateInstance<T>();
            for (int i = 0; i < m_fields.Length; i++)
                SetValue(obj, m_fields[i], line[i]);
            return obj;
        }

        /// <summary>
        /// 设置对象属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private void SetValue(object obj, FieldInfo field, string value)
        {
            if (field.FieldType == typeof(byte))
            {
                field.SetValue(obj, byte.Parse(value));
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(obj, int.Parse(value));
            }
            else if (field.FieldType == typeof(long))
            {
                field.SetValue(obj, long.Parse(value));
            }
            else if (field.FieldType == typeof(float))
            {
                field.SetValue(obj, float.Parse(value));
            }
            else if (field.FieldType == typeof(double))
            {
                field.SetValue(obj, double.Parse(value));
            }
            else if (field.FieldType == typeof(string))
            {
                field.SetValue(obj, value);
            }
            else if (field.FieldType == typeof(i18nObject))
            {
                field.SetValue(obj, TypeUtils.ContentToI18nObjectValue(value));
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(obj, TypeUtils.ContentToBooleanValue(value));
            }
            else if (field.FieldType == typeof(Vector2))
            {
                field.SetValue(obj, TypeUtils.ContentToVector2Value(value));
            }
            else if (field.FieldType == typeof(Vector3))
            {
                field.SetValue(obj, TypeUtils.ContentToVector3Value(value));
            }
            else if (field.FieldType == typeof(byte[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayByteValue(value));
            }
            else if (field.FieldType == typeof(int[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayIntValue(value));
            }
            else if (field.FieldType == typeof(long[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayLongValue(value));
            }
            else if (field.FieldType == typeof(float[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayFloatValue(value));
            }
            else if (field.FieldType == typeof(double[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayDoubleValue(value));
            }
            else if (field.FieldType == typeof(string[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayStringValue(value));
            }
            else if (field.FieldType == typeof(bool[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayBooleanValue(value));
            }
            else if (field.FieldType == typeof(Vector2[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayVector2Value(value));
            }
            else if (field.FieldType == typeof(Vector3[]))
            {
                field.SetValue(obj, TypeUtils.ContentToArrayVector3Value(value));
            }
        }

        /// <summary>
        /// 解析 CSV 内容
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ParseCSVLine(string text, char separator = ',')
        {
            List<string> line = new List<string>();
            StringBuilder token = new StringBuilder();
            bool quotes = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (quotes == true)
                {
                    if ((text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\"') || (text[i] == '\"' && i + 1 < text.Length && text[i + 1] == '\"'))
                    {
                        token.Append('\"');
                        i++;
                    }
                    else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == 'n')
                    {
                        token.Append('\n');
                        i++;
                    }
                    else if (text[i] == '\"')
                    {
                        line.Add(token.ToString());
                        token = new StringBuilder();
                        quotes = false;
                        if (i + 1 < text.Length && text[i + 1] == separator)
                            i++;
                    }
                    else
                    {
                        token.Append(text[i]);
                    }
                }
                else if (text[i] == '\r' || text[i] == '\n')
                {
                    if (token.Length > 0)
                    {
                        line.Add(token.ToString());
                        token = new StringBuilder();
                    }
                    if (line.Count > 0)
                    {
                        return line.ToArray();
                    }
                }
                else if (text[i] == separator)
                {
                    line.Add(token.ToString());
                    token = new StringBuilder();
                }
                else if (text[i] == '\"')
                {
                    quotes = true;
                }
                else
                {
                    token.Append(text[i]);
                }
            }

            if (token.Length > 0)
            {
                line.Add(token.ToString());
            }
            return line.ToArray();
        }
    }
}