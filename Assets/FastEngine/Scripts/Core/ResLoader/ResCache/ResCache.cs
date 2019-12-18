/*
 * @Author: fasthro
 * @Date: 2019-06-26 14:12:35
 * @Description: 资源池，缓存所有资源
 */

using System.Collections.Generic;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/ResLoader/ResCache")]
    public class ResCache : MonoSingleton<ResCache>
    {
        // 资源池字典
        readonly Dictionary<string, Res> resDictionary = new Dictionary<string, Res>();

        /// <summary>
        /// 获取Res对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="create"></param>
        public Res _Get(ResData data, bool create = false)
        {
            Res res = null;
            if (resDictionary.TryGetValue(data.poolKey, out res))
            {
                res.Retain();
                return res;
            }

            if (!create) return null;

            res = ResFactory.Create(data);

            if (res == null) return null;

            res.Retain();
            resDictionary.Add(data.poolKey, res);

            return res;
        }

        /// <summary>
        /// 资源移除
        /// </summary>
        /// <param name="data"></param>
        public void _Remove(ResData data)
        {
            if (resDictionary.ContainsKey(data.poolKey))
            {
                resDictionary.Remove(data.poolKey);
            }
        }

        #region DEBUG
#if UNITY_EDITOR
        public List<Res> GetPoolList()
        {
            List<Res> list = new List<Res>();
            resDictionary.ForEach((item) =>
            {
                list.Add(item.Value);
            });
            return list;
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

        /// <summary>
        /// 资源移除
        /// </summary>
        /// <param name="data"></param>
        public static void Remove(ResData data)
        {
            Instance._Remove(data);
        }
        #endregion
    }
}