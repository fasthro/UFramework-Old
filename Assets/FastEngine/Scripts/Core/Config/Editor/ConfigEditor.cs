/*
 * @Author: fasthro
 * @Date: 2020-05-22 19:45:41
 * @Description: Config Editor
 */ 

  using UnityEditor;

namespace FastEngine.Editor.FastConfig
{
    public class ConfigEditor
    {
        [MenuItem("FastEngine/Config -> 打开配置", false, 800)]
        public static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<ConfigEditorWindow>();
        }

        [MenuItem("FastEngine/Config -> 构建环境", false, 801)]
        public static void Build()
        {
            
        }
    }
}

