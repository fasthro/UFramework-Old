/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel reader
 */
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public class ExcelReader
    {
        private string m_filePath;
        public ExcelReaderOptions options { get; private set; }

        public List<string> descriptions { get; private set; }
        public List<string> fields { get; private set; }
        public List<FieldType> types { get; private set; }
        public List<int> ignoreIndexs { get; private set; }
        public List<ExcelReaderRow> rows { get; private set; }

        public ExcelReader(string filePath, ExcelReaderOptions options)
        {
            this.m_filePath = filePath;
            this.options = options;
            this.descriptions = new List<string>();
            this.fields = new List<string>();
            this.types = new List<FieldType>();
            this.ignoreIndexs = new List<int>();
            this.rows = new List<ExcelReaderRow>();
        }

        public void Read()
        {
            descriptions.Clear();
            fields.Clear();
            types.Clear();
            ignoreIndexs.Clear();
            rows.Clear();

            FieldType fieldType;
            bool removeIgnore = false;

            using (var stream = File.Open(m_filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    for (int i = 0; i < reader.ResultsCount; i++)
                    {
                        var dataTable = result.Tables[i];
                        var columnCount = dataTable.Columns.Count;
                        var rowCount = dataTable.Rows.Count;

                        for (int r = 0; r < rowCount; r++)
                        {
                            var row = new ExcelReaderRow();
                            for (int c = 0; c < columnCount; c++)
                            {
                                var context = dataTable.Rows[r][c].ToString();
                                if (r == 0)
                                {
                                    descriptions.Add(context);
                                }
                                else if (r == 1)
                                {
                                    fields.Add(context);
                                }
                                else if (r == 2)
                                {
                                    fieldType = TypeUtils.TypeContentToFieldType(context);
                                    types.Add(fieldType);
                                    if (fieldType == FieldType.Ignore)
                                    {
                                        ignoreIndexs.Add(c);
                                    }
                                }
                                else
                                {
                                    row.datas.Add(context);
                                }
                            }

                            if (r > 2)
                            {
                                if (!removeIgnore)
                                {
                                    descriptions = RemoveIgnore<string>(descriptions, ignoreIndexs);
                                    fields = RemoveIgnore<string>(fields, ignoreIndexs);
                                    types = RemoveIgnore<FieldType>(types, ignoreIndexs);
                                    removeIgnore = true;
                                }
                                row.descriptions = descriptions;
                                row.fields = fields;
                                row.types = types;
                                row.datas = RemoveIgnore<string>(row.datas, ignoreIndexs);
                                rows.Add(row);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除忽略项
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="indexs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static List<T> RemoveIgnore<T>(List<T> sources, List<int> indexs)
        {
            int count = indexs.Count;
            int index = 0;
            int step = 0;
            while (count > 0)
            {
                index = indexs[0] - step;

                sources.RemoveAt(index);
                count--;
                step++;
            }

            return sources;
        }
    }
}