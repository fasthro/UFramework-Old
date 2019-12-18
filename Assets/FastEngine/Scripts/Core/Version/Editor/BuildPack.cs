/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:24:52
 * @Description: 构建包
 */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FastEngine.Core;
using FastEngine.Editor.AssetBundle;
using FastEngine.Editor.Lua;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class BuildPack
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

        // 打包配置
        static BuildPackConfig buildConfig;
        // 版本配置
        static VersionConfig versionConfig;
        // 资源压缩回调
        static ZipCallback zipCallback = new ZipCallback();
        // 编辑器模式下持久化目录
        static string editorPersistentDataPath { get { return FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "PersistentData"); } }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="build">true:构建包 false:只否建资源</param>
        public static void Run(BuildTarget platform, bool build = true)
        {
            if (EditorUserBuildSettings.activeBuildTarget != platform)
            {
                Debug.LogError("构建平台:" + platform.ToString() + " 编辑器平台:" + EditorUserBuildSettings.activeBuildTarget);
                return;
            }
            SwitchRelease();

            // 清理编辑器模式下持久化目录
            FilePathUtils.DirectoryClean(editorPersistentDataPath);

            // 加载构建配置
            buildConfig = AppUtils.ReadConfig<BuildPackConfig>(AppUtils.EditorConfigPath("BuildPackConfig"));
            // bundleVersionCode 自增
            buildConfig.bundleVersionCode++;

            // 加载版本配置
            var path = AppUtils.EditorConfigPath("VersionConfig");
            if (!FilePathUtils.FileExists(path))
            {
                Debug.Log("请填写版本配置");
                Debug.Log("[Build Pack] -> 构建版本配置-失败. 打开菜单(FastEngine-Version -> 打开配置)进行配置.");
                return;
            }
            versionConfig = AppUtils.ReadConfig<VersionConfig>(path);

            // 构建版本资源
            BuildVersionResource();

            // 创建版本配置文件
            versionConfig.compressFileTotalCount = zipCallback.count;
            FilePathUtils.FileWriteAllText(FilePathUtils.Combine(Application.streamingAssetsPath, "VersionConfig.json"), JsonMapper.ToJson(versionConfig));

            if (build)
            {
                // 构建包
                BuildVersionPack();
                // 保存构建配置
                FilePathUtils.FileWriteAllText(AppUtils.EditorConfigPath("BuildPackConfig"), JsonMapper.ToJson(buildConfig));
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 构建版本资源
        /// </summary>
        private static void BuildVersionResource()
        {
            Debug.Log("[Build Pack] -> 构建版本资源 (Clean:" + buildConfig.cleanBuild + ")");
            if (buildConfig.cleanBuild) AssetBundleEditor.CleanBuild();
            else AssetBundleEditor.Build();
            AssetBundleEditor.CopySource();
            // lua bundle
            LuaBuildBundle.Build();

            // 压缩资源
            Debug.Log("[Build Pack] -> 压缩资源包");
            var outPath = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), PlatformUtils.PlatformId() + ".zip");
            FilePathUtils.DeleteFile(outPath);
            ZipUtils.Zip(new string[] { AppUtils.BundleRootDirectory() },
                FilePathUtils.Combine(Application.streamingAssetsPath, PlatformUtils.PlatformId() + ".zip"),
                null,
                zipCallback);
            Debug.Log("[Build Pack] -> 压缩资源完成(File Count: " + zipCallback.count + ")");

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
            Debug.Log("[Build Pack] -> Build " + EditorUserBuildSettings.activeBuildTarget + " Environment Finished!");
        }

        /// <summary>
        /// 构建版本包
        /// </summary>
        public static void BuildVersionPack()
        {
            Debug.Log("[Build Pack] -> 构建包");

            var buildPath = FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "Build", versionConfig.ToVersionString());
            // 清理导出目录
            FilePathUtils.DirectoryClean(buildPath);

            // 构建默认热更新配置
            var hotfixConfig = new HotfixConfig();
            hotfixConfig.version = versionConfig;
            hotfixConfig.hotfix = false;
            FilePathUtils.FileWriteAllText(FilePathUtils.Combine(buildPath, versionConfig.ToResourceString(), "HotfixConfig.json"), JsonMapper.ToJson(hotfixConfig));

            // 设置 PlayerSetting
            PlayerSettings.bundleVersion = versionConfig.ToVersionString();

            // android
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = buildConfig.bundleVersionCode;
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = buildConfig.keystoreName;
                PlayerSettings.Android.keystorePass = buildConfig.keystorePass;
                PlayerSettings.Android.keyaliasName = buildConfig.keyaliasName;
                PlayerSettings.Android.keyaliasPass = buildConfig.keyaliasPass;

                if (buildConfig.andoridIL2CPP)
                {
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                }
                else {
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
                }
            }
            // ios
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                if (buildConfig.iOSIL2CPP) PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
                else PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
            }
            // windows x86/64
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
            {
                if (buildConfig.WindowsIL2CPP) PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
                else PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
            }

            // 构建包
            var outPath = FilePathUtils.Combine(buildPath, versionConfig.ToVersionString());
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) outPath += ".apk";
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) outPath += ".exe";
            var scenes = EditorBuildSettings.scenes.Where(x => x.enabled).ToArray();
            BuildPipeline.BuildPlayer(scenes, outPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

            Debug.Log("[Build Pack] -> Succeed!");
        }

        /// <summary>
        /// 切换到Release模式，重新构建开发环境
        /// </summary>
        private static void SwitchRelease()
        {
            // 修改App配置
            var outPath = FilePathUtils.Combine(Application.dataPath, "Resources", "AppConfig.json");
            var appConfig = AppUtils.ReadConfig<AppConfig>(outPath);
            appConfig.runModel = AppRunModel.Release;
            FilePathUtils.FileWriteAllText(outPath, JsonMapper.ToJson(appConfig));
        }

        /// <summary>
        /// 切换到开发模式，重新构建开发环境
        /// </summary>
        public static void SwitchDevelop()
        {
            // 清理Streaming目录
            FilePathUtils.DirectoryDelete(Application.streamingAssetsPath);
            // 修改App配置
            var outPath = FilePathUtils.Combine(Application.dataPath, "Resources", "AppConfig.json");
            var appConfig = AppUtils.ReadConfig<AppConfig>(outPath);
            appConfig.runModel = AppRunModel.Develop;
            appConfig.enableLog = true;
            appConfig.useSystemLanguage = true;
            FilePathUtils.FileWriteAllText(outPath, JsonMapper.ToJson(appConfig));
            // 打包资源
            AssetBundleEditor.CleanBuild();

            AssetDatabase.Refresh();
        }
    }
}