/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:39:07
 * @Description: table config
 */
using System.Collections.Generic;
using FastEngine.Core;

namespace FastEngine.Core.Excel2Table
{
    public class TableItem
    {
        public string tableName { get; set; }                          // 数据表名称
        public DataFormatOptions dataFormatOptions { get; set; }       // 数据格式
    }

    public class TableConfig : ConfigObject
    {
        public FormatOptions outFormatOptions { get; set; }                // 导出格式
        public string tableModelNamespace { get; set; }                    // tableModel 命名空间
        public Dictionary<string, TableItem> tableDictionary { get; set; } // 数据表

        protected override void OnInitialize()
        {
            if (tableDictionary == null)
            {
                tableDictionary = new Dictionary<string, TableItem>();
            }
        }
    }
}
