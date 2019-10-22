/*
 * @Author: fasthro
 * @Date: 2019-06-26 14:12:35
 * @Description: 资源池，缓存所有资源
 */
 
using System.Collections.Generic;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/ResLoader/ResPool")]
    public class ResPool : MonoSingleton<ResPool>
    {
        // 资源池字典
        private readonly Dictionary<string, Res> poolDic = new Dictionary<string, Res>();
        // 资源池列表
        private readonly List<Res> poolList = new List<Res>();

        /// <summary>
        /// 获取Res对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="create"></param>
        public Res _Get(ResData data, bool create = false)
        {
            Res res = null;
            if (poolDic.TryGetValue(data.poolKey, out res))
            {
                res.Retain();
                return res;
            }

            if (!create) return null;

            res = ResFactory.Create(data);

            if (res == null) return null;

            res.Retain();

            poolDic.Add(data.poolKey, res);
            if (!poolList.Contains(res)) poolList.Add(res);

            return res;
        }

        #region DEBUG
#if UNITY_EDITOR
        public List<Res> GetPoolList()
        {
            return poolList;
        }
#endif
        #endregion


        #region API
        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="data"></param>
        /// <param name="createNew">如果没有是否创建</param>
        public static T Get<T>(ResData data, bool createNew = false) where T : Res
        {
            return Get(data, createNew) as T;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="data"></param>
        /// <param name="createNew">如果没有是否创建</param>
        public static Res Get(ResData data, bool createNew = false)
        {
            return Instance._Get(data, createNew);
        }
        #endregion
    }
}