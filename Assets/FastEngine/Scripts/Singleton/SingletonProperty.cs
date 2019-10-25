/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 
 */

namespace FastEngine
{
    public static class SingletonProperty<T> where T : class, ISingleton
    {
        private static T instance;
        private static readonly object obj = new object();

        public static T Instance
        {
            get
            {
                lock (obj)
                {
                    if (instance == null)
                        instance = SingletonCreator.CreateSingleton<T>();
                }
                return instance;
            }
        }

        public static void Dispose()
        {
            instance = null;
        }
    }
}