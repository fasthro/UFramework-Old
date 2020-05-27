/*
 * @Author: fasthro
 * @Date: 2020-05-09 14:32:39
 * @Description: SDK 编辑器
 */

 using UnityEditor;

namespace FastEngine.Editor.SDK
{
    public class SDKEditor
    {
        [MenuItem("FastEngine/SDK -> 打开配置", false, 700)]
        public static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<SDKEditorWindow>();
        }

        [MenuItem("FastEngine/SDK -> 构建环境", false, 701)]
        public static void Build()
        {
            
        }
    }
}
