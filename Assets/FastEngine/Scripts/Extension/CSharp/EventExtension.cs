/*
 * @Author: fasthro
 * @Date: 2019-08-03 15:13:58
 * @Description: Event Extension
 */
using System;

namespace FastEngine
{
    public static class EventExtension
    {
        /// <summary>
        /// Call delegate
        /// </summary>
        public static bool InvokeGracefully(this Delegate self, params object[] args)
        {
            if (null != self)
            {
                self.DynamicInvoke(args);
                return true;
            }
            return false;
        }

    }
}