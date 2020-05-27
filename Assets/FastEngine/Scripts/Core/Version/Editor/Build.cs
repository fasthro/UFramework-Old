/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:24:52
 * @Description: 构建逻辑
 */
using System.IO;
using System.Linq;
using FastEngine.Core;
using FastEngine.Editor.AssetBundle;
using FastEngine.Editor.Lua;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class Build
    {
        // 压缩回调
        class ZipCallback : ZipUtils.ZipCallback
        {
            // 压缩文件数量
            public int count { get; private set; }
            public override void OnPostZip(ICSharpCode.SharpZipLib.Zip.ZipEntry _entry)
            {
                count++;
            }
        }

        // 资源压缩回调
        static ZipCallback zipCallback;
        // 打包配置
        static BuildConfig buildConfig;

        // 编辑器模式下持久化目录
        static string editorPersistentDataPath { get { return FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "PersistentData"); } }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="build">true:构建包 false:只否建资源</param>
        public static void Run(BuildTarget platform)
        {
            if (EditorUserBuildSettings.activeBuildTarget != platform)
            {
                Debug.LogError("构建平台:" + platform.ToString() + " 编辑器平台:" + EditorUserBuildSettings.activeBuildTarget);
                return;
            }

            var runModel = Config.ReadEditorDirectory<AppConfig>().runModel;
            if (runModel == AppRunModel.Develop)
            {
                Debug.LogError("当前为开发模式，无法构建包.");
                return;
            }

            // 切换模式
            SwitchModel(runModel);

            // // 构建配置
            buildConfig = Config.ReadEditorDirectory<BuildConfig>();
            buildConfig.AutoIncrementBundleVersionCode(ToBuildPlatformType(platform));

            // 生成资源压缩包
            MakeZipResource(platform);

            // 生成平台包
            MakePlatformBinary(platform);

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        /// <param name="runModel"></param>
        public static void SwitchModel(AppRunModel runModel)
        {
            // 清理编辑器下持久化目录
            FilePathUtils.DirectoryClean(AppUtils.EditorPersistentDataRootDirectory());

            // 清理 Resource Config 目录
            FilePathUtils.DirectoryClean(AppUtils.EditorResourceConfigRootDirectory());

            // 清理 StreamingAssets
            FilePathUtils.DirectoryClean(Application.streamingAssetsPath);

            var appConfig = Config.ReadEditorDirectory<AppConfig>();
            appConfig.runModel = runModel;
            appConfig.Save<AppConfig>();

            // 打包资源
            AssetBundleEditor.Build();
            AssetBundleEditor.CopySource();
            LuaBuildBundle.Build();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            if (runModel != AppRunModel.Develop)
            {
                // copy config
                var baseConfig = Config.ReadEditorDirectory<BaseConfig>();
                baseConfig.map.ForEach((item) =>
                {
                    if (item.Value == ConfigAddress.Resource)
                    {
                        var source = FilePathUtils.Combine(AppUtils.ConfigEditorDirectory(), item.Key + ".json");
                        var dest = FilePathUtils.Combine(AppUtils.EditorResourceConfigRootDirectory(), item.Key + ".json");
                        if (FilePathUtils.FileExists(source))
                            FilePathUtils.FileCopy(source, dest);
                    }
                    else if (item.Value == ConfigAddress.Data)
                    {
                        var source = FilePathUtils.Combine(AppUtils.ConfigEditorDirectory(), item.Key + ".json");
                        var dest = FilePathUtils.Combine(AppUtils.ConfigDataDirectory(), item.Key + ".json");
                        if (FilePathUtils.FileExists(source))
                            FilePathUtils.FileCopy(source, dest);
                    }
                });
            }
        }

        /// <summary>
        /// 生成资源压缩包
        /// </summary>
        private static void MakeZipResource(BuildTarget platform)
        {
            // 生成 VersionConfig
            var version = buildConfig.GetVersion(ToBuildPlatformType(platform));
            version.Save<VersionConfig>(AppUtils.ConfigDataDirectory());

            // 压缩资源
            Debug.Log("[Build] -> 压缩资源包");
            var outPath = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), PlatformUtils.PlatformId() + ".zip");
            FilePathUtils.DeleteFile(outPath);
            zipCallback = new ZipCallback();
            ZipUtils.Zip(new string[] { AppUtils.BundleRootDirectory() },
                FilePathUtils.Combine(Application.streamingAssetsPath, PlatformUtils.PlatformId() + ".zip"),
                null,
                zipCallback);

            // 记录压缩文件数量
            var appConfig = Config.ReadEditorDirectory<AppConfig>();
            appConfig.version = version;
            appConfig.compressFileTotalCount = zipCallback.count;
            appConfig.Save<AppConfig>(AppUtils.EditorResourceConfigRootDirectory());
            appConfig.Save<AppConfig>();

            // 清理所有与版本不相关的文件夹
            var dirs = Directory.GetDirectories(Application.streamingAssetsPath);
            for (int i = 0; i < dirs.Length; i++)
            {
                Directory.Delete(dirs[i], true);
            }
            // 清理与版本不相关的文件
            var files = Directory.GetFiles(Application.streamingAssetsPath, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                var file = files[i];
                if (!file.EndsWith(PlatformUtils.PlatformId() + ".zip")
                    && !file.EndsWith(".json"))
                {
                    File.Delete(files[i]);
                }
            }
            Debug.Log("[Build] -> 压缩资源完成(File Count: " + zipCallback.count + ")");
        }

        /// <summary>
        /// 生成平台包
        /// </summary>
        public static void MakePlatformBinary(BuildTarget platform)
        {
            Debug.Log("[Build] -> 构建平台包");

            var version = buildConfig.GetVersion(ToBuildPlatformType(platform));
            var buildPath = AppUtils.BuildVersionRootDirectory(version);

            // 清理 Build Version 目录
            FilePathUtils.DirectoryClean(buildPath);

            // 生成默认热更新配置
            var hotfixConfig = new HotfixConfig();
            hotfixConfig.version = version;
            hotfixConfig.hotfix = false;
            hotfixConfig.Save<HotfixConfig>(AppUtils.BuildVersionResourceRootDirectory(version));

            // 设置 PlayerSetting
            PlayerSettings.bundleVersion = version.ToVersionString();

            // android
            if (platform == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = buildConfig.android.bundleVersionCode;
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = buildConfig.android.keystoreName;
                PlayerSettings.Android.keystorePass = buildConfig.android.keystorePass;
                PlayerSettings.Android.keyaliasName = buildConfig.android.keyaliasName;
                PlayerSettings.Android.keyaliasPass = buildConfig.android.keyaliasPass;

                if (buildConfig.android.il2cpp)
                {
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                }
                else
                {
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
                }
            }
            // ios
            else if (platform == BuildTarget.iOS)
            {
                if (buildConfig.ios.il2cpp) PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
                else PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
            }
            // windows x86/64
            else if (platform == BuildTarget.StandaloneWindows64 || platform == BuildTarget.StandaloneWindows)
            {
                if (buildConfig.windows.il2cpp) PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
                else PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
            }

            // make 
            var outPath = string.Empty;
            if (platform == BuildTarget.Android)
            {
                outPath = buildPath + ".apk";
            }
            else if (platform == BuildTarget.StandaloneWindows64 || platform == BuildTarget.StandaloneWindows)
            {
                outPath = buildPath + ".exe";
            }

            // 保存构建配置
            buildConfig.Save<BuildConfig>();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            var scenes = EditorBuildSettings.scenes.Where(x => x.enabled).ToArray();
            BuildPipeline.BuildPlayer(scenes, outPath, platform, BuildOptions.None);

            Debug.Log("[Build] -> Succeed!");
        }

        /// <summary>
        /// BuildTarget To BuildPlatformType
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static BuildPlatformType ToBuildPlatformType(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return BuildPlatformType.Android;
                case BuildTarget.iOS:
                    return BuildPlatformType.iOS;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return BuildPlatformType.Windows;
                default:
                    return BuildPlatformType.Windows;
            }
        }
    }
}