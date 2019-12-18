/*
 * @Author: fasthro
 * @Date: 2019-11-26 20:02:16
 * @Description: editor window 基类
 */
using LitJson;
using UnityEditor;

namespace FastEngine.Editor
{
    public abstract class FastEditorWindow : EditorWindow
    {
        public void Initialize()
        {
            OnInitialize();
        }
        protected virtual void OnInitialize() { }

        public static T ShowWindow<T>() where T : FastEditorWindow, new()
        {
            var window = GetWindow<T>(false, "");
            window.Initialize();
            return window;
        }
    }
}