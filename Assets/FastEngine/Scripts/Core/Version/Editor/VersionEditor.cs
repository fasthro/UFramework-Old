/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:14:45
 * @Description: 版本编辑器
 */
using UnityEditor;

namespace FastEngine.Editor.Version
{
    public class VersionEditor
    {
        [MenuItem("FastEngine/Version -> 打开配置", false, 400)]
        public static void OpenConfig()
        {
            FastEditorWindow.ShowWindow<VersionEditorWindow>();
        }

        [MenuItem("FastEngine/Version -> Build Android .apk", false, 401)]
        public static void BuildAndroid()
        {
            Build.Run(BuildTarget.Android);
        }

        [MenuItem("FastEngine/Version -> Build iOS .app", false, 410)]
        public static void BuildiOS()
        {
            Build.Run(BuildTarget.iOS);
        }

        [MenuItem("FastEngine/Version -> Build Windows .exe", false, 420)]
        public static void BuildWindows()
        {
            Build.Run(BuildTarget.StandaloneWindows64);
        }


        [MenuItem("FastEngine/Version -> 切换到开发模式", false, 430)]
        public static void SwitchDevelopModel()
        {
            Build.SwitchModel(AppRunModel.Develop);
        }

        [MenuItem("FastEngine/Version -> 切换到测试模式", false, 431)]
        public static void SwitchTestModel()
        {
            Build.SwitchModel(AppRunModel.Test);
        }

        [MenuItem("FastEngine/Version -> 切换到正式模式", false, 432)]
        public static void SwitchReleaseModel()
        {
            Build.SwitchModel(AppRunModel.Release);
        }
    }
}