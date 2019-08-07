/*
 * @Author: fasthro
 * @Date: 2019-08-03 15:09:20
 * @Description: Action Extension
 */
using System;

namespace FastEngine.Common
{
    public static class ActionExtension
    {
        /// <summary>
        /// Call
        /// </summary>
        public static bool InvokeGracefully(this Action action)
        {
            if (null != action)
            {
                action();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call
        /// </summary>
        /// <returns></returns>
        public static bool InvokeGracefully<T>(this Action<T> action, T t)
        {
            if (null != action)
            {
                action(t);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call
        /// </summary>
        public static bool InvokeGracefully<T, K>(this Action<T, K> action, T t, K k)
        {
            if (null != action)
            {
                action(t, k);
                return true;
            }
            return false;
        }
    }
}