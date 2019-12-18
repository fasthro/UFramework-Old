/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:31:06
 * @Description: excel reader
 */
using System.IO;
using ExcelDataReader;

namespace FastEngine.Core.I18n
{
    public class ExcelReader
    {
        public ExcelReaderOptions options { get; private set; }
        public ExcelSheet[] sheets { get; private set; }

        public ExcelReader(ExcelReaderOptions options)
        {
            this.options = options;
        }

        public void Read()
        {
            using (var stream = File.Open(AppUtils.i18nExcelFilePath(), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    sheets = new ExcelSheet[reader.ResultsCount];

                    var result = reader.AsDataSet();
                    for (int i = 0; i < reader.ResultsCount; i++)
                    {
                        var dataTable = result.Tables[i];
                        var columnCount = dataTable.Columns.Count;
                        var rowCount = dataTable.Rows.Count;

                        var sheet = new ExcelSheet();
                        sheet.name = dataTable.TableName;
                        sheet.columns = new ExcelColumn[rowCount];
                        for (int rc = 0; rc < rowCount; rc++)
                        {
                            sheet.columns[rc] = new ExcelColumn();
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
                        sheets[i] = sheet;
                    }
                }
            }
        }
    }
}