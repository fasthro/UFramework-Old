/*
 * @Author: fasthro
 * @Date: 2020-02-29 21:57:13
 * @Description: fairy 3d gameObject(模型、粒子、骨骼动画等3D对象在UI中显示)
 */

using System.Collections.Generic;
using FairyGUI;
using FastEngine.Core;
using UnityEngine;

namespace FastEngine.FairyUI
{
    /// <summary>
    /// 封装 GoWrapper 
    /// </summary>
    public class FairyGoWrapper
    {
        public int sortingOrder { get; private set; }
        public GoWrapper wrapper { get; private set; }
        public GGraph holder { get; private set; }

        public GameObject gameObject { get; set; }

        private Vector3 _position;
        public Vector3 position
        {
            get { return _position; }
            set
            {
                _position = value;
                _position.z = (float)sortingOrder;
                if (gameObject != null)
                    gameObject.transform.localPosition = value;
            }
        }

        private Vector3 _scale;
        public Vector3 scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                if (gameObject != null)
                    gameObject.transform.localScale = value;
            }
        }

        private Vector3 _angles;
        public Vector3 angles
        {
            get { return _angles; }
            set
            {
                _angles = value;
                if (gameObject != null)
                    gameObject.transform.localEulerAngles = value;
            }
        }

        private bool _visible;
        public bool visible
        {
            get
            {
                return _visible;

            }
            set
            {
                _visible = value;
                wrapper.visible = value;
            }
        }
        private AssetBundleLoader m_bundleLoader;

        /// <summary>
        /// 构造函函数
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="holder"></param>
        /// <param name="go"></param>
        public FairyGoWrapper(int sortingOrder, GGraph holder, GameObject go) { Initialize(sortingOrder, holder, go); }

        /// <summary>
        /// 构造函数（资源路径进行资源加载）
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="holder"></param>
        /// <param name="resPath">资源路径进行bundle加载</param>
        public FairyGoWrapper(int sortingOrder, GGraph holder, string resPath)
        {
            m_bundleLoader = AssetBundleLoader.Allocate(resPath, null);
            var ready = m_bundleLoader.LoadSync();
            if (ready) gameObject = GameObject.Instantiate<GameObject>(m_bundleLoader.assetRes.GetAsset<GameObject>());
            else gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            Initialize(sortingOrder, holder, gameObject);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="holder"></param>
        /// <param name="go"></param>
        private void Initialize(int sortingOrder, GGraph holder, GameObject go)
        {
            gameObject = go;
            var pos = gameObject.transform.localPosition;
            pos.z = sortingOrder;
            position = pos;
            scale = gameObject.transform.localScale;
            angles = gameObject.transform.localEulerAngles;

            wrapper = new GoWrapper(go);
            holder.SetNativeObject(wrapper);
        }

        /// <summary>
        /// 设置显示物体像素大小
        /// </summary>
        /// <param name="size"></param>
        public void SetPixelSize(float size) { scale = new Vector3(size, size, size); }
            
        /// <summary>
        /// 设置显示物体位置
        /// </summary>
        /// <param name="size"></param>
        public void SetXY(float x, float y) { position = new Vector3(x, sortingOrder, y); }

        /// <summary>
        /// 更换 GameObject
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cloneMaterial"></param>
        public void ReplaceGameObject(GameObject target, bool cloneMaterial)
        {
            if (gameObject != null) GameObject.DestroyImmediate(gameObject);

            gameObject = target;
            gameObject.transform.localPosition = position;
            gameObject.transform.localScale = scale;
            gameObject.transform.localEulerAngles = angles;

            wrapper.SetWrapTarget(target, cloneMaterial);
        }

        /// <summary>
        /// 更换 GameObject
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="cloneMaterial"></param>
        public void ReplaceGameObject(string resPath, bool cloneMaterial)
        {
            if (gameObject != null) GameObject.DestroyImmediate(gameObject);
            if (m_bundleLoader != null)
            {
                m_bundleLoader.Unload();
                m_bundleLoader = null;
            }

            m_bundleLoader = AssetBundleLoader.Allocate(resPath, null);
            var ready = m_bundleLoader.LoadSync();
            if (ready) gameObject = GameObject.Instantiate<GameObject>(m_bundleLoader.assetRes.GetAsset<GameObject>());
            else gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            gameObject.transform.localPosition = position;
            gameObject.transform.localScale = scale;
            gameObject.transform.localEulerAngles = angles;

            wrapper.SetWrapTarget(gameObject, cloneMaterial);
        }

        /// <summary>
        /// 缩放 GameObject
        /// </summary>
        /// <param name="resPath"></param>
        /// <param name="cloneMaterial"></param>
        public void SetZoomSize(float size)
        {
            var childs = gameObject.transform.GetComponentsInChildren<ParticleSystem>();
            for (int i = 1; i < childs.Length; i++)
            {
                var main = childs[i].main;
                main.scalingMode = ParticleSystemScalingMode.Local;
                var scale = childs[i].transform.localScale;
                childs[i].transform.localScale = new Vector3(scale.x * size,scale.y*size,scale.z*size);
            }
        }

        /// <summary>
        /// 更新 GameObject
        /// </summary>
        public void UpdateGameObject() { wrapper.CacheRenderers(); }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            if (gameObject != null) GameObject.DestroyImmediate(gameObject);
            if (m_bundleLoader != null)
            {
                m_bundleLoader.Unload();
                m_bundleLoader = null;
            }
            wrapper.Dispose();
        }
    }

    /// <summary>
    /// window 3d gameObject 管理
    /// </summary>
    public class Fairy3DGameObject
    {
        // 排序代表的Z轴深度值
        public int sortingOrder { get; private set; }

        // go wrapper hashset
        private HashSet<FairyGoWrapper> m_wrappers = new HashSet<FairyGoWrapper>();

        public Fairy3DGameObject(int sortingOrder) { this.sortingOrder = sortingOrder; }

        /// <summary>
        /// create wrapper
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="holder"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public FairyGoWrapper Create(GGraph holder, GameObject go)
        {
            var wrapper = new FairyGoWrapper(sortingOrder, holder, go);
            m_wrappers.Add(wrapper);
            return wrapper;
        }

        /// <summary>
        /// create wrapper
        /// </summary>
        /// <param name="sortingOrder"></param>
        /// <param name="holder"></param>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public FairyGoWrapper Create(GGraph holder, string resPath)
        {
            var wrapper = new FairyGoWrapper(sortingOrder, holder, resPath);
            m_wrappers.Add(wrapper);
            return wrapper;
        }

        /// <summary>
        /// remove wrapper and dispose wrapper
        /// </summary>
        /// <param name="wrapper"></param>
        public void Remove(FairyGoWrapper wrapper)
        {
            wrapper.Dispose();
            if (m_wrappers.Contains(wrapper))
                m_wrappers.Remove(wrapper);
        }

        /// <summary>
        ///  remove all wrapper and dispose all wrapper
        /// </summary>
        public void RemoveAll()
        {
            foreach (FairyGoWrapper wrapper in m_wrappers)
                wrapper.Dispose();
            m_wrappers.Clear();
        }
    }

    /// <summary>
    /// window 3d gameObject manager
    /// </summary>
    public class FairyWindow3DGameObject
    {
        static bool initialized = false;

        // 默认模型Z轴厚度
        const int Z_AXIS_THICKNESS = 1000;

        // 窗口字典
        static Dictionary<FairyWindow, Fairy3DGameObject> m_windowDictionary;

        // static stage camera
        static StageCamera m_uiCamera;
        static StageCamera uiCamera
        {
            get
            {
                if (m_uiCamera == null)
                {
                    m_uiCamera = GameObject.FindObjectOfType<StageCamera>();
                }
                return m_uiCamera;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            if (!initialized) initialized = true;
            else return;

            m_windowDictionary = new Dictionary<FairyWindow, Fairy3DGameObject>();
        }

        /// <summary>
        /// add winodw return fairy 3d gameObject
        /// </summary>
        /// <param name="window"></param>
        /// <returns>fairyModel</returns>
        public static Fairy3DGameObject Add(FairyWindow window)
        {
            Initialize();
            Fairy3DGameObject fairyModel = null;
            if (!m_windowDictionary.TryGetValue(window, out fairyModel))
            {
                var baseLayer = (int)window.layer * FairyWindowLayer.LAYER_INTERVAL;
                var sortingOrder = baseLayer + (window.sortingOrder - baseLayer) * Z_AXIS_THICKNESS;
                fairyModel = new Fairy3DGameObject(sortingOrder);
                m_windowDictionary.Add(window, fairyModel);
                UpdateCameraFar(sortingOrder);
            }
            return fairyModel;
        }

        /// <summary>
        /// remove
        /// </summary>
        /// <param name="window"></param>
        public static void Remove(FairyWindow window)
        {
            Initialize();
            Fairy3DGameObject fairyModel = null;
            if (m_windowDictionary.TryGetValue(window, out fairyModel))
            {
                fairyModel.RemoveAll();
                m_windowDictionary.Remove(window);
            }
        }

        public static Fairy3DGameObject Get(FairyWindow window)
        {
            Initialize();
            Fairy3DGameObject fairyModel = null;
            m_windowDictionary.TryGetValue(window, out fairyModel);
            return fairyModel;
        }

        public static void Sort()
        {
            Initialize();
        }

        static void UpdateCameraFar(int sortingOrder)
        {
            if (uiCamera != null && uiCamera.cachedCamera != null)
            {
                var far = sortingOrder / 100;
                if (far < 30) far = 30;
                uiCamera.cachedCamera.farClipPlane = far;
            }
        }
    }
}