/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 any
 */
namespace FastEngine.Core.Excel2Table
{
    public abstract class Excel2Any
    {
        protected ExcelReader reader;
        
        public Excel2Any(ExcelReader reader)
        {
            this.reader = reader;
        }
    }
}