/*
 * @Author: fasthro
 * @Date: 2019-12-17 19:51:37
 * @Description: table editor
 */
using FastEngine.Core.Excel2Table;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Excel2Table
{
    public class TableEditor
    {
        [MenuItem("FastEngine/Table -> 打开配置", false, 600)]
        static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<TableEditorWindow>();
        }

        [MenuItem("FastEngine/Table -> 生成数据", false, 601)]
        public static void Generate()
        {
            TableConfig tableConfig = AppUtils.ReadEditorConfig<TableConfig>();

            tableConfig.tableDictionary.ForEach((item) =>
            {

                var options = new ExcelReaderOptions();
                options.tableName = item.Value.tableName;
                options.tableModelNamespace = tableConfig.tableModelNamespace;
                options.outFormatOptions = tableConfig.outFormatOptions;
                options.dataFormatOptions = item.Value.dataFormatOptions;
                options.dataOutDirectory = AppUtils.TableDataDirectory();
                options.tableModelOutDirectory = AppUtils.TableObjectDirectory();
                var reader = new ExcelReader(string.Format("{0}/{1}.xlsx", AppUtils.TableExcelDirectory(), item.Value.tableName), options);
                reader.Read();
                switch (tableConfig.outFormatOptions)
                {
                    case FormatOptions.CSV:
                        new Excel2CSV(reader);
                        break;
                    case FormatOptions.JSON:
                        new Excel2Json(reader);
                        break;
                    case FormatOptions.LUA:
                        new Excel2Lua(reader);
                        break;
                }
                new Excel2TableObject(reader);
            });
            AssetDatabase.Refresh();
            Debug.Log("Table Generate Succeed!");
        }
    }
}