/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 
 */

using System;

namespace FastEngine.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
        private string m_pathInHierarchy;
        public string pathInHierarchy
        {
            get { return m_pathInHierarchy; }
        }

        public MonoSingletonPath(string pathInHierarchy)
        {
            m_pathInHierarchy = pathInHierarchy;
        }
    }
}
