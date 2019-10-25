/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 对象池
 */

using UnityEngine;

namespace FastEngine.Core
{
    public class ObjectPool<T> : Pool<T>, ISingleton where T : IPoolObject, new()
    {
        #region Singleton
        void ISingleton.InitializeSingleton() { }

        protected ObjectPool()
        {
            m_factory = new ObjectFactory<T>();
        }

        public static ObjectPool<T> Instance
        {
            get { return SingletonProperty<ObjectPool<T>>.Instance; }
        }

        public void Dispose()
        {
            SingletonProperty<ObjectPool<T>>.Dispose();
        }
        #endregion

        // 池最大数量,如果池中数量大于最大数，就移除多余的对象
        public int maxCount
        {
            get { return m_maxCount; }
            set
            {
                m_maxCount = value;

                if (m_stacks != null)
                {
                    if (m_maxCount > 0)
                    {
                        if (m_maxCount < m_stacks.Count)
                        {
                            int removeCount = m_stacks.Count - m_maxCount;
                            while (removeCount > 0)
                            {
                                m_stacks.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="maxCount">池中对象最大数量</param>
        /// <param name="initCount">池对象初始数量</param>
        public void Init(int maxCount, int initCount)
        {
            this.maxCount = maxCount;

            if (maxCount > 0)
            {
                initCount = Mathf.Min(maxCount, initCount);
            }

            if (count < initCount)
            {
                for (var i = count; i < initCount; ++i)
                {
                    Recycle(new T());
                }
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj"></param>
        public override bool Recycle(T obj)
        {
            if (obj == null || obj.isRecycled)
                return false;

            if (m_maxCount > 0)
            {
                if (m_stacks.Count >= m_maxCount)
                    return false;
            }

            obj.isRecycled = true;
            m_stacks.Push(obj);

            return true;
        }
    }
}