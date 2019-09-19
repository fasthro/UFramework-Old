/*
 * @Author: fasthro
 * @Date: 2019-09-19 11:10:44
 * @Description: 可枚举的集合扩展（Array、List<T>、Dictionary<K,V>)
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastEngine.Common
{
    public static class IEnumerableExtension
    {
        #region Array Extension

        /// <summary>
        /// 遍历数组
        /// <code>
        /// var testArray = new[] { 1, 2, 3 };
        /// testArray.ForEach(number => number.LogInfo());
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns> 返回自己 </returns>
        public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        /// <summary>
        /// 遍历 IEnumerable
        /// <code>
        /// // IEnumerable<T>
        /// IEnumerable<int> testIenumerable = new List<int> { 1, 2, 3 };
        /// testIenumerable.ForEach(number => number.LogInfo());

        /// // 支持字典的遍历
        /// new Dictionary<string, string>()
        ///         .ForEach(keyValue => Log.I("key:{0},value:{1}", keyValue.Key, keyValue.Value));
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (var item in selfArray)
            {
                action(item);
            }

            return selfArray;
        }

        #endregion

        #region List Extension

        /// <summary>
        /// 倒序遍历
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse(number => number.LogInfo()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>返回自己</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i]);

            return selfList;
        }

        /// <summary>
        /// 倒序遍历（可获得索引)
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse((number,index)=> number.LogInfo()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>The each reverse.</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T, int> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i], i);

            return selfList;
        }

        /// <summary>
        /// 遍历列表(可获得索引）
        /// <code>
        /// var testList = new List<int> {1, 2, 3 };
        /// testList.Foreach((number,index)=>number.LogInfo()); // 1, 2, 3,
        /// </code>
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">目标表</param>
        /// <param name="action">行为</param>
        public static void ForEach<T>(this List<T> list, Action<int, T> action)
        {
            for (var i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }

        #endregion

        #region Dictionary Extension

        /// <summary>
        /// 合并字典
        /// <code>
        /// // 示例
        /// var dictionary1 = new Dictionary<string, string> { { "1", "2" } };
        /// var dictionary2 = new Dictionary<string, string> { { "3", "4" } };
        /// var dictionary3 = dictionary1.Merge(dictionary2);
        /// dictionary3.ForEach(pair => Log.I("{0}:{1}", pair.Key, pair.Value));
        /// </code>
        /// </summary>
        /// <returns>The merge.</returns>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="dictionaries">Dictionaries.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            params Dictionary<TKey, TValue>[] dictionaries)
        {
            return dictionaries.Aggregate(dictionary,
                (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value));
        }

        /// <summary>
        /// 遍历字典
        /// <code>
        /// var dict = new Dictionary<string,string> {{"name","liangxie},{"age","18"}};
        /// dict.ForEach((key,value)=> Log.I("{0}:{1}",key,value);//  name:liangxie    age:18
        /// </code>
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
        {
            var dictE = dict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        /// <summary>
        /// 字典添加新的词典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="addInDict"></param>
        /// <param name="isOverride"></param>
        public static void AddRange<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> addInDict,
            bool isOverride = false)
        {
            var dictE = addInDict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                if (dict.ContainsKey(current.Key))
                {
                    if (isOverride)
                        dict[current.Key] = current.Value;
                    continue;
                }

                dict.Add(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        #endregion
    }
}