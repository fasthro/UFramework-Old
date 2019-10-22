using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class PoolGameObject : Behaviour
    {
        // 所属对象池名称
        public string poolName;
        // 是否回收
        public bool isFree;
    }

    public class GameObjectPoolContainer
    {
        // 池名称
        private string m_poolName;
        public string poolName { get { return m_poolName; } }

        // 最大数量
        private int m_maxCount;
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
                                DestroyPoolGameObject(m_stacks.Pop());
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        // Prefab
        private GameObject m_prefab;

        private Transform m_root;

        // 池
        private Stack<PoolGameObject> m_stacks = new Stack<PoolGameObject>();

        /// <summary>
        /// GameObject Pool
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="prefab"></param>
        /// <param name="maxCount"></param>
        /// <param name="initCount"></param>
        public GameObjectPoolContainer(string poolName, GameObject prefab, int maxCount, int initCount, Transform root)
        {
            this.m_poolName = poolName;
            this.m_prefab = prefab;
            this.m_maxCount = maxCount;
            this.m_root = root;

            if (maxCount > 0)
                initCount = Mathf.Min(maxCount, initCount);

            for (int i = 0; i < initCount; i++)
            {
                Recycle(CreatePoolGameObject());
            }
        }

        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public PoolGameObject Allocate(Transform parent = null)
        {
            PoolGameObject pgo = null;
            if (m_stacks.Count > 0)
            {
                pgo = m_stacks.Pop();
                pgo.gameObject.transform.SetParent(parent, false);
                pgo.gameObject.SetActive(true);
            }
            else
            {
                GameObject newGo = GameObject.Instantiate(m_prefab) as GameObject;
                newGo.transform.SetParent(parent, false);
                pgo = newGo.AddComponent<PoolGameObject>();
                pgo.poolName = poolName;
                pgo.isFree = false;
            }
            return pgo;
        }

        /// <summary>
        /// 回收 PoolGameObject
        /// </summary>
        /// <param name="pgo"></param>
        public bool Recycle(PoolGameObject pgo)
        {
            if (pgo == null || pgo.isFree)
                return false;

            if (m_stacks.Count >= maxCount)
            {
                DestroyPoolGameObject(pgo);
                return false;
            }

            pgo.gameObject.SetActive(false);
            m_stacks.Push(pgo);
            pgo.isFree = true;
            pgo.transform.SetParent(m_root, false);

            return true;
        }

        /// <summary>
        /// 创建 PoolGameObject
        /// </summary>
        private PoolGameObject CreatePoolGameObject()
        {
            var go = GameObject.Instantiate<GameObject>(m_prefab);
            var pgo = go.GetComponent<PoolGameObject>();
            if (pgo == null) pgo = go.AddComponent<PoolGameObject>();
            pgo.poolName = poolName;
            pgo.isFree = false;
            return pgo;
        }

        /// <summary>
        /// 销毁 PoolGameObject
        /// </summary>
        private void DestroyPoolGameObject(PoolGameObject pgo)
        {
            pgo.transform.SetParent(null);
            GameObject.Destroy(pgo.gameObject);
            pgo = null;
        }
    }

    [MonoSingletonPath("FastEngine/GameObjectPool")]
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        // 池字典
        private Dictionary<string, GameObjectPoolContainer> m_poolDic = new Dictionary<string, GameObjectPoolContainer>();

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="prefab"></param>
        /// <param name="maxCount"></param>
        /// <param name="initCount"></param>
        public void CreatePool(string poolName, GameObject prefab, int maxCount, int initCount)
        {
            if (!m_poolDic.ContainsKey(poolName))
            {
                GameObject cGo = new GameObject(poolName);
                cGo.transform.SetParent(transform);
                GameObjectPoolContainer c = new GameObjectPoolContainer(poolName, prefab, maxCount, initCount, cGo.transform);
                m_poolDic.Add(poolName, c);
            }
        }

        /// <summary>
        /// 分配
        /// </summary>
        /// <param name="parent"></param>
        public PoolGameObject Allocate(string poolName, Transform parent = null)
        {
            GameObjectPoolContainer pc = null;
            if (!m_poolDic.TryGetValue(poolName, out pc))
            {
                Debug.LogError(string.Format("GameObject Pool Allocate Error. You need to create {0} Pool.", poolName));
                return null;
            }
            return pc.Allocate(parent);
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="pgo"></param>
        public bool Recycle(PoolGameObject pgo)
        {
            GameObjectPoolContainer pc = null;
            if (!m_poolDic.TryGetValue(pgo.poolName, out pc))
            {
                Debug.LogError(string.Format("GameObject Pool Recycle Error. You need to create {0} Pool.", pgo.poolName));
                return false;
            }
            return pc.Recycle(pgo);
        }
    }
}