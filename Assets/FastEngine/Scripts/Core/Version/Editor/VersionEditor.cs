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

        [MenuItem("FastEngine/Version -> Build Android Application", false, 401)]
        public static void BuildAndroid()
        {
            BuildPack.Run(BuildTarget.Android);
        }

        // [MenuItem("FastEngine/Version -> Build Android Environment", false, 402)]
        // public static void BuildAndroidEnv()
        // {
        //     BuildPack.Run(BuildTarget.Android, false);
        // }

        [MenuItem("FastEngine/Version -> Build iOS Application", false, 410)]
        public static void BuildiOS()
        {
            BuildPack.Run(BuildTarget.iOS);
        }

        // [MenuItem("FastEngine/Version -> Build iOS Environment", false, 411)]
        // public static void BuildiOSEnv()
        // {
        //     BuildPack.Run(BuildTarget.iOS, false);
        // }

        [MenuItem("FastEngine/Version -> Build Windows Application", false, 420)]
        public static void BuildWindows()
        {
            BuildPack.Run(BuildTarget.StandaloneWindows64);
        }

        // [MenuItem("FastEngine/Version -> Build Windows Environment", false, 421)]
        // public static void BuildWindowsEnv()
        // {
        //     if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        //     {
        //         BuildPack.Run(EditorUserBuildSettings.activeBuildTarget, false);
        //     }
        // }

        [MenuItem("FastEngine/Version -> 切换到开发模式", false, 430)]
        public static void SwitchDevelop()
        {
            BuildPack.SwitchDevelop();
        }
    }
}