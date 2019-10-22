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
        protected static T m_instance = null;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                return m_instance;
            }
        }

        public virtual void OnSingletonInit()
        {

        }

        public virtual void Dispose()
        {
            if (MonoSingletonCreator.IsUnitTestMode)
            {
                var curTrans = transform;
                do
                {
                    var parent = curTrans.parent;
                    DestroyImmediate(curTrans.gameObject);
                    curTrans = parent;
                } while (curTrans != null);

                m_instance = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            m_instance = null;
        }
    }
}