/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: Singleton
 */

namespace FastEngine
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T instance;
        private static object obj = new object();

        public static T Instance
        {
            get
            {
                if (null == Singleton<T>.instance)
                {
                    lock (obj)
                    {
                        if (null == Singleton<T>.instance)
                        {
                            Singleton<T>.instance = System.Activator.CreateInstance<T>();
                            (Singleton<T>.instance as Singleton<T>).InitializeSingleton();
                        }
                    }
                }
                return instance;
            }
        }

        public virtual void InitializeSingleton() { }

        public static bool HasInstance() { return Singleton<T>.instance != null; }

        public virtual void Dispose()
        {
            if (Singleton<T>.instance != null) Singleton<T>.instance = (T)((object)null);
        }
    }
}