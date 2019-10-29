/*
 * @Author: fasthro
 * @Date: 2019-10-28 14:00:49
 * @Description: FUI 层级
 */
using System.Collections.Generic;
using System.Reflection;

namespace FastEngine.FUI
{
    /// <summary>
    /// UI面板层级定义
    /// </summary>
    public enum FLayer
    {
        Scene = 0,
        SceneTop = 10000,
        WindowBottom = 20000,
        Window = 30000,
        WindowTop = 40000,
        Popup = 50000,
        Guide = 60000,
        Notification = 70000,
        Net = 80000,
        Loader = 90000,
        Forward = 100000,
    }

    /// <summary>
    /// window cache
    /// </summary>
    public class FWindowLayerCache
    {
        private List<FWindow> m_windows = new List<FWindow>();
        private int m_baseIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public FWindowLayerCache(int index)
        {
            this.m_baseIndex = index;
        }

        /// <summary>
        /// 添加 window
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isAutoSort"></param>
        /// <returns></returns>
        public int Add(FWindow window, bool isAutoSort)
        {
            if (m_windows.Contains(window))
            {
                m_windows.Remove(window);
            }
            m_windows.Add(window);

            if (isAutoSort) Sort();

            return m_baseIndex + m_windows.Count;
        }

        /// <summary>
        /// 移除 window
        /// </summary>
        /// <param name="window"></param>
        public void Remove(FWindow window)
        {
            if (m_windows.Contains(window))
            {
                m_windows.Remove(window);
            }
        }

        /// <summary>
        /// 排序 cache 中的所有 window
        /// </summary>
        public void Sort()
        {
            for (int i = 0; i < m_windows.Count; i++)
                m_windows[i].sortingOrder = m_baseIndex + i;
        }
    }

    public class FWindowSortService
    {
        static Dictionary<FLayer, FWindowLayerCache> m_map = new Dictionary<FLayer, FWindowLayerCache>();

        public static void Initialize()
        {
            AddMap(FLayer.Scene);
            AddMap(FLayer.SceneTop);
            AddMap(FLayer.WindowBottom);
            AddMap(FLayer.Window);
            AddMap(FLayer.WindowTop);
            AddMap(FLayer.Guide);
            AddMap(FLayer.Notification);
            AddMap(FLayer.Net);
            AddMap(FLayer.Loader);
            AddMap(FLayer.Forward);
        }

        static void AddMap(FLayer layer) { m_map.Add(layer, new FWindowLayerCache((int)layer)); }

        /// <summary>
        /// 添加 window
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isAutoSort"></param>
        /// <returns></returns>
        public static int Add(FWindow window, bool isAutoSort = true)
        {
            FWindowLayerCache lc = null;
            if (m_map.TryGetValue(window.layer, out lc))
            {
                return lc.Add(window, isAutoSort);
            }
            return -1;
        }

        /// <summary>
        /// 移除 window
        /// </summary>
        /// <param name="window"></param>
        public static void Remove(FWindow window)
        {
            FWindowLayerCache lc = null;
            if (m_map.TryGetValue(window.layer, out lc))
            {
                lc.Remove(window);
            }
        }

        /// <summary>
        /// 排序 window
        /// </summary>
        /// <param name="layer"></param>
        public static void Sort(FLayer layer)
        {
            FWindowLayerCache lc = null;
            if (m_map.TryGetValue(layer, out lc))
            {
                lc.Sort();
            }
        }

        /// <summary>
        /// 排序所有 window
        /// </summary>
        public static void SortAll()
        {
            foreach (KeyValuePair<FLayer, FWindowLayerCache> item in m_map)
            {
                item.Value.Sort();
            }
        }
    }
}
