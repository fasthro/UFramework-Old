/*
 * @Author: fasthro
 * @Date: 2019-08-03 15:04:42
 * @Description: Class Extention
 */
namespace FastEngine.Common
{
    public static class ClassExtention
    {
        /// <summary>
        /// 空判断
        /// </summary>
        public static bool IsNull<T>(this T selfObj) where T : class
        {
            return null == selfObj;
        }

        /// <summary>
        /// 非空判断
        /// </summary>
        public static bool IsNotNull<T>(this T selfObj) where T : class
        {
            return null != selfObj;
        }
    }
}
