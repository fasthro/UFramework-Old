/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: MonoSingleton
 */
using UnityEngine;

namespace FastEngine
{
    public abstract class MonoSingletonProperty<T> : MonoBehaviour, ISingleton where T : MonoSingletonProperty<T>
    {
        protected static T instance = null;
        private static readonly object obj = new object();

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                }
                return MonoSingletonProperty<T>.instance;
            }
        }

        private void Awake() { this.InitializeSingleton(); }

        public void InitializeSingleton() { }

        public void Dispose()
        {
            GameObject.Destroy(instance.gameObject);
            instance = null;
        }

        
    }
}