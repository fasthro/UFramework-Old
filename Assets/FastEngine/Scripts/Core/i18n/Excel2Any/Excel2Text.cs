/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:52:23
 * @Description: excel 2 text data
 */
namespace FastEngine.Core.I18n
{
    public class Excel2Text
    {
        public Excel2Text(ExcelReader reader)
        {
            FilePathUtils.DirectoryClean(AppUtils.i18nDataDirectory());

            for (int i = 0; i < reader.sheets.Length; i++)
            {
                var sheet = reader.sheets[i];
                for (int l = 0; l < reader.options.languages.Count; l++)
                {
                    var language = reader.options.languages[l];

                    FilePathUtils.FileWriteAllText(FilePathUtils.Combine(AppUtils.i18nDataDirectory(), language.ToString(), i.ToString() + ".txt"), sheet.ToValueString(l + 1));
                }
            }
        }
    }
}