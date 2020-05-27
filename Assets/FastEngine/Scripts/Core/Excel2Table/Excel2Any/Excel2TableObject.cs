/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 csharp object
 */
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public class Excel2TableObject : Excel2Any
    {
        private string template = @"// FastEngine
// excel2table auto generate

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FastEngine;
using FastEngine.Core;
using FastEngine.Core.Excel2Table;

namespace $namespace$
{
    public class $tableName$TableData
    {
$variable$
    }

    public class $tableName$Table : Singleton<$tableName$Table>, ITableObject
    {
        public string tableName { get { return $tableNameStr$; } }
        public int maxCount { get { return m_tableDatas.Length; } }
        
        public DataFormatOptions dataFormatOptions = DataFormatOptions.$dataFormat$;
        private $tableName$TableData[] m_tableDatas;
        private Dictionary<int, $tableName$TableData> m_tableDataIntDictionary;
        private Dictionary<string, $tableName$TableData> m_tableDataStringDictionary;
        private Dictionary<int, Dictionary<int, $tableName$TableData>> m_tableDataInt2IntDictionary;

        public override void InitializeSingleton()
        {
            switch (dataFormatOptions)
            {
                case DataFormatOptions.Array:
                    m_tableDatas = new TableParseCSV(tableName).ParseArray<$tableName$TableData>();
                    break;
                case DataFormatOptions.IntDictionary:
                    m_tableDataIntDictionary = new TableParseCSV(tableName).ParseIntDictionary<$tableName$TableData>();
                    break;
                case DataFormatOptions.StringDictionary:
                    m_tableDataStringDictionary = new TableParseCSV(tableName).ParseStringDictionary<$tableName$TableData>();
                    break;
                case DataFormatOptions.Int2IntDictionary:
                    m_tableDataInt2IntDictionary = new TableParseCSV(tableName).ParseInt2IntDictionary<$tableName$TableData>();
                    break;
            }
        }

        private $tableName$TableData _GetIndexData(int index)
        {
            if (dataFormatOptions == DataFormatOptions.Array)
            {
                if (index >= 0 && index < m_tableDatas.Length)
                {
                    return m_tableDatas[index];
                }
            }
            else $GetIndexDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(int key)
        {
            if (dataFormatOptions == DataFormatOptions.IntDictionary)
            {
                $tableName$TableData data = null;
                if (m_tableDataIntDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else $GetIntKeyDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(string key)
        {
            if (dataFormatOptions == DataFormatOptions.StringDictionary)
            {
                $tableName$TableData data = null;
                if (m_tableDataStringDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else $GetStringKeyDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(int key1, int key2)
        {
            if (dataFormatOptions == DataFormatOptions.Int2IntDictionary)
            {
                Dictionary<int, $tableName$TableData> dictionary = null;
                if (m_tableDataInt2IntDictionary.TryGetValue(key1, out dictionary))
                {
                    $tableName$TableData data = null;
                    if (dictionary.TryGetValue(key2, out data))
                    {
                        return data;
                    }
                }
            }
            else $GetInt2IntKeyDataError$
            return null;
        }

        public static $tableName$TableData GetIndexData(int index) { return Instance._GetIndexData(index); }
        public static $tableName$TableData GetKeyData(int key) { return Instance._GetKeyData(key); }
        public static $tableName$TableData GetKeyData(string key) { return Instance._GetKeyData(key); }
        public static $tableName$TableData GetKeyData(int key1, int key2) { return Instance._GetKeyData(key1, key2); }
    }
}";

        private StringBuilder m_stringBuilder = new StringBuilder();

        public Excel2TableObject(ExcelReader reader) : base(reader)
        {
            if (string.IsNullOrEmpty(reader.options.tableModelNamespace)) reader.options.tableModelNamespace = "Table";

            template = template.Replace("$namespace$", reader.options.tableModelNamespace);
            template = template.Replace("$tableName$", reader.options.tableName);
            template = template.Replace("$tableNameStr$", string.Format("\"{0}\"", reader.options.tableName));
            template = template.Replace("$dataFormat$", reader.options.dataFormatOptions.ToString());
            template = template.Replace("$GetIndexDataError$", string.Format("Debug.LogError(\"[{0}Table] DataFormatOptions: {1}. Please use the GetKeyData(index)\");", reader.options.tableName, DataFormatOptions.Array.ToString()));
            template = template.Replace("$GetIntKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] DataFormatOptions: {1}. Please use the GetKeyData(int-key)\");", reader.options.tableName, DataFormatOptions.IntDictionary.ToString()));
            template = template.Replace("$GetStringKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] DataFormatOptions: {1}. Please use the GetKeyData(string-key)\");", reader.options.tableName, DataFormatOptions.StringDictionary.ToString()));
            template = template.Replace("$GetInt2IntKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] DataFormatOptions: {1}. Please use the GetKeyData(int-key, int-key)\");", reader.options.tableName, DataFormatOptions.Int2IntDictionary.ToString()));

            for (int i = 0; i < reader.fields.Count; i++)
            {
                m_stringBuilder.AppendLine("        // " + reader.descriptions[i]);
                m_stringBuilder.AppendLine(string.Format("        public {0} {1};", TypeUtils.FieldTypeToTypeContent(reader.types[i]), reader.fields[i]));
                // 方便国家化字段调用,为国际化添加字段
                if (reader.types[i] == FieldType.i18n)
                {
                    m_stringBuilder.AppendLine(string.Format("        public string {0}_i18n {{ get {{ return {1}.ToString(); }} }}", reader.fields[i], reader.fields[i]));
                }
            }
            template = template.Replace("$variable$", m_stringBuilder.ToString());

            FilePathUtils.FileWriteAllText(reader.options.tableModelOutFilePath, template);
        }
    }
}