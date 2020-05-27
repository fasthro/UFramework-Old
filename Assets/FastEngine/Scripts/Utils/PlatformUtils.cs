/*
 * @Author: fasthro
 * @Date: 2019-11-27 10:33:56
 * @Description: 平台工具类
 */
using UnityEngine;

namespace FastEngine
{
    /// <summary>
    /// 平台设备类型
    /// </summary>
    public enum PlatformDeviceType
    {
        IOS_MOBILE,
        IOS_TABLET,
        ANDROID_MOBILE,
        ANDROID_TABLET,
        UNKNOWN
    }

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

        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <returns></returns>
        public static PlatformDeviceType GetDeviceType()
        {
#if UNITY_ANDROID
            float physicScreen = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / Screen.dpi;
            if (physicScreen >= 7f) return PlatformDeviceType.ANDROID_TABLET;
            else return PlatformDeviceType.ANDROID_MOBILE;
#elif UNITY_IPHONE
            string generation = UnityEngine.iOS.Device.generation.ToString().ToLower().Trim();
            if (generation.StartsWith("ipad")) return PlatformDeviceType.IOS_TABLET;
            else return PlatformDeviceType.IOS_MOBILE;
#else
       return PlatformDeviceType.UNKNOWN;
#endif
        }
    }
}