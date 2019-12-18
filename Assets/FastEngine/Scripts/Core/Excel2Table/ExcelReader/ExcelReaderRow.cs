/*
 * @Author: fasthro
 * @Date: 2019-12-18 15:27:10
 * @Description: Excel Reader Row
 */
using System.Collections.Generic;

namespace FastEngine.Core.Excel2Table
{
    public class ExcelReaderRow
    {
        public List<string> descriptions;   // 描述
        public List<string> fields;         // 字段
        public List<FieldType> types;       // 类型
        public List<string> datas;          // 数据

        public ExcelReaderRow()
        {
            descriptions = new List<string>();
            fields = new List<string>();
            types = new List<FieldType>();
            datas = new List<string>();
        }
    }
}