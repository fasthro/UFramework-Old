/*
 * @Author: fasthro
 * @Date: 2019-11-26 20:02:16
 * @Description: 配置窗口基类 Config Editor Window(CEW)
 */
using FastEngine.Utils;
using LitJson;
using UnityEditor;

namespace FastEngine.Editor
{
    public abstract class CEWBase : EditorWindow
    {
        // 配置路径
        protected string path;

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// 打开window
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Open<T>() where T : CEWBase, new()
        {
            var window = GetWindow<T>(false, "");
            window.OnInitialize();
            return window;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadConfig<T>() where T : new() { return AppUtils.LoadConfig<T>(path); }
    }
}