/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: Singleton
 */

namespace FastEngine.Common
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T m_instance;
        private static object m_lock = new object();

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

        protected Singleton() { }

        public virtual void Dispose()
        {
            m_instance = null;
        }

        public virtual void OnSingletonInit() { }
    }
}