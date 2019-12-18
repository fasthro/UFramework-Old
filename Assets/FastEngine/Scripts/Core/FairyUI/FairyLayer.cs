/*
 * @Author: fasthro
 * @Date: 2019-10-28 14:00:49
 * @Description: fairy ui layer
 */
using System.Collections.Generic;
using System.Reflection;

namespace FastEngine.FairyUI
{
    /// <summary>
    /// fairy ui layer Define
    /// </summary>
    public enum FairyLayerType
    {
        Scene = 0,
        SceneTop = 10000,
        WindowBottom = 20000,
        Window = 30000,
        WindowTop = 40000,
        Popup = 50000,
        Guide = 60000,
        Notification = 70000,
        Network = 80000,
        Loader = 90000,
        Forward = 100000,
    }

    /// <summary>
    /// fairy layer
    /// </summary>
    public class FairyLayer
    {
        private HashSet<FairyWindow> m_windows;
        private int m_baseIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public FairyLayer(int index)
        {
            this.m_baseIndex = index;
            this.m_windows = new HashSet<FairyWindow>();
        }

        /// <summary>
        /// add window
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isAutoSort"></param>
        /// <returns></returns>
        public int Add(FairyWindow window, bool isAutoSort)
        {
            if (m_windows.Contains(window)) m_windows.Remove(window);
            m_windows.Add(window);
            if (isAutoSort) Sort();
            return m_baseIndex + m_windows.Count;
        }

        /// <summary>
        /// remove window
        /// </summary>
        /// <param name="window"></param>
        public void Remove(FairyWindow window)
        {
            if (m_windows.Contains(window)) m_windows.Remove(window);
        }

        /// <summary>
        /// sort windows
        /// </summary>
        public void Sort()
        {
            int i = 0;
            foreach (FairyWindow window in m_windows)
            {
                window.sortingOrder = m_baseIndex + i;
                i++;
            }
        }
    }

    /// <summary>
    /// fairy window sortingOrder
    /// </summary>
    public class FairyWindowSortingOrder
    {
        static Dictionary<FairyLayerType, FairyLayer> layerDictionary;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            layerDictionary = new Dictionary<FairyLayerType, FairyLayer>();

            AddLayer(FairyLayerType.Scene);
            AddLayer(FairyLayerType.SceneTop);
            AddLayer(FairyLayerType.WindowBottom);
            AddLayer(FairyLayerType.Window);
            AddLayer(FairyLayerType.WindowTop);
            AddLayer(FairyLayerType.Guide);
            AddLayer(FairyLayerType.Notification);
            AddLayer(FairyLayerType.Network);
            AddLayer(FairyLayerType.Loader);
            AddLayer(FairyLayerType.Forward);
        }

        /// <summary>
        /// add layer
        /// </summary>
        /// <param name="layer"></param>
        static void AddLayer(FairyLayerType layer)
        {
            layerDictionary.Add(layer, new FairyLayer((int)layer));
        }

        /// <summary>
        /// add window to layer
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isAutoSort"></param>
        /// <returns></returns>
        public static int Add(FairyWindow window, bool isAutoSort = true)
        {
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(window.layer, out fairyLayer))
                return fairyLayer.Add(window, isAutoSort);
            return -1;
        }

        /// <summary>
        /// remove window to layer
        /// </summary>
        /// <param name="window"></param>
        public static void Remove(FairyWindow window)
        {
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(window.layer, out fairyLayer))
                fairyLayer.Remove(window);
        }

        /// <summary>
        /// sort window to layer
        /// </summary>
        /// <param name="layer"></param>
        public static void Sort(FairyLayerType layer)
        {
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(layer, out fairyLayer))
                fairyLayer.Sort();
        }

        /// <summary>
        /// sor window to all layer
        /// </summary>
        public static void SortAll()
        {
            layerDictionary.ForEach((item) => { item.Value.Sort(); });
        }
    }
}
