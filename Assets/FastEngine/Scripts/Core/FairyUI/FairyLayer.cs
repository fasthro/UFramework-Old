/*
 * @Author: fasthro
 * @Date: 2019-10-28 14:00:49
 * @Description: fairy ui layer
 */
using System;
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
        SceneTop,
        WindowBottom,
        MainWindow,
        MainTop,
        Window,
        WindowTop,
        Popup,
        Guide,
        Notification,
        Network,
        Loader,
        Forward
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
        /// next layer number
        /// </summary>
        /// <returns></returns>
        public int Next() { return m_baseIndex + m_windows.Count + 1; }

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
    /// fairy window layer sortingOrder
    /// </summary>
    public class FairyWindowLayer
    {
        // 层级间隔
        public const int LAYER_INTERVAL = 1000000;

        // 初始化
        static bool initialized = false;
        // layer 字典
        static Dictionary<FairyLayerType, FairyLayer> layerDictionary;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            if (!initialized) initialized = true;
            else return;

            layerDictionary = new Dictionary<FairyLayerType, FairyLayer>();

            Type type = typeof(FairyLayerType);
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                var layer = (FairyLayerType)fieldInfos[i].GetValue(null);
                layerDictionary.Add(layer, new FairyLayer((int)layer * LAYER_INTERVAL));
            }
        }

        /// <summary>llll
        /// add window to layer
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isAutoSort"></param>
        /// <returns></returns>
        public static int Add(FairyWindow window, bool isAutoSort = true)
        {
            Initialize();
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
            Initialize();
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(window.layer, out fairyLayer))
                fairyLayer.Remove(window);
        }

        /// <summary>
        /// next layer number
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static int Next(FairyLayerType layer)
        {
            Initialize();
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(layer, out fairyLayer))
            {
                return fairyLayer.Next();
            }
            return (int)layer * LAYER_INTERVAL;
        }

        /// <summary>
        /// sort window to layer
        /// </summary>
        /// <param name="layer"></param>
        public static void Sort(FairyLayerType layer)
        {
            Initialize();
            FairyLayer fairyLayer = null;
            if (layerDictionary.TryGetValue(layer, out fairyLayer))
                fairyLayer.Sort();
        }

        /// <summary>
        /// sor window to all layer
        /// </summary>
        public static void SortAll()
        {
            Initialize();
            layerDictionary.ForEach<FairyLayerType, FairyLayer>((TargetException, layer) =>
            {
                layer.Sort();
            });
        }
    }
}
