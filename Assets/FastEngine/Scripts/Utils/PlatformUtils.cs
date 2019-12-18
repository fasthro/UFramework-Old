/*
 * @Author: fasthro
 * @Date: 2019-11-27 10:33:56
 * @Description: 平台工具类
 */
namespace FastEngine
{
    public static class PlatformUtils
    {
        /// <summary>
        /// 平台标识Id
        /// </summary>
        public static string PlatformId()
        {
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IPHONE
            return "iOS";
#elif UNITY_STANDALONE_WIN
            return "Windows";
#elif UNITY_STANDALONE_OSX
            return "OSX";
#else
            return "Unknown";
#endif
        }
    }
}