/*
 * @Author: fasthro
 * @Date: 2019-08-03 15:11:42
 * @Description: Func Extension
 */

using System;

namespace FastEngine.Common
{
    public static class FuncExtension
    {
        /// <summary>
        /// Call
        /// </summary>
        public static T InvokeGracefully<T>(this Func<T> func)
        {
            return null != func ? func() : default(T);
        }
    }
}