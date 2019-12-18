/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:14:45
 * @Description: 版本编辑器
 */
using UnityEditor;

namespace FastEngine.Editor.Version
{
    public class HotfixEditor
    {
        [MenuItem("FastEngine/HotfixUpdate -> 打开配置", false, 500)]
        public static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<HotfixEditorWindow>();
        }

        [MenuItem("FastEngine/HotfixUpdate -> 构建资源", false, 501)]
        public static void Build()
        {
            BuildHotfix.Run();
        }
    }
}