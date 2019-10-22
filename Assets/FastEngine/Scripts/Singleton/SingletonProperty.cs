/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 
 */

namespace FastEngine
{
    public static class SingletonProperty<T> where T : class, ISingleton
    {
        private static T m_instance;
        private static readonly object m_lock = new object();

        public static T Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_instance == null)
                        m_instance = SingletonCreator.CreateSingleton<T>();
                }
                return m_instance;
            }
        }

        public static void Dispose()
        {
            m_instance = null;
        }
    }
}