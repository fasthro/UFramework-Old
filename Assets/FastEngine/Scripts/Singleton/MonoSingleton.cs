/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: MonoSingleton
 */
using UnityEngine;

namespace FastEngine
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        protected static T instance = null;
        private static readonly object obj = new object();
        private static bool isQuitApplication = false;

        public static T Instance
        {
            get
            {
                if (isQuitApplication)
                {
                    Debug.LogError(string.Format("Try To Call [MonoSingleton] Instance {0} When The Application Already Quit, return null inside", typeof(T)));
                    return null;
                }
                lock (obj)
                {
                    if (null == instance)
                    {
                        instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                    }
                }
                return MonoSingleton<T>.instance;
            }
        }

        private void Awake()
        {
            isQuitApplication = false;
            this.InitializeSingleton();
        }

        public virtual void InitializeSingleton() { }

        public static bool HasInstance() { return MonoSingleton<T>.instance != null; }

        public virtual void Dispose()
        {
            isQuitApplication = true;
            Debug.Log("[MonoSingleton] OnDestroy '" + typeof(T).FullName + "'");
            MonoSingleton<T>.instance = null;
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (MonoSingleton<T>.instance != null && MonoSingleton<T>.instance.gameObject == base.gameObject)
                MonoSingleton<T>.instance = (T)((object)null);
        }
    }
}