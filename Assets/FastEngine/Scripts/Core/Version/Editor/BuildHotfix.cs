/*
 * @Author: fasthro
 * @Date: 2019-12-02 14:30:11
 * @Description: 构建热更新资源
 */
using System.Collections.Generic;
using System.IO;
using FastEngine.Core;
using FastEngine.Editor.AssetBundle;
using FastEngine.Editor.Lua;
using LitJson;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class BuildHotfix
    {
        // 打包配置
        private static BuildHotfixConfig buildConfig;
        // 修复配置
        private static HotfixConfig fixConfig;
        // 目标配置
        private static HotfixConfig config;

        /// <summary>
        /// 
        /// </summary>
        public static void Run()
        {
            // 加载打包配置
            var path = AppUtils.EditorConfigPath("BuildHotfixConfig");
            if (!FilePathUtils.FileExists(path))
            {
                Debug.Log("请填写版本配置");
                Debug.Log("[Build Hotfix] -> 构建版本配置-失败. 打开菜单(FastEngine-HotfixUpdate -> 打开配置)进行配置.");
                return;
            }
            buildConfig = AppUtils.ReadConfig<BuildHotfixConfig>(path);

            // 构建资源
            if (buildConfig.cleanBuild) AssetBundleEditor.CleanBuild();
            else AssetBundleEditor.Build();
            // lua bundle
            LuaBuildBundle.Build();

            // 构建
            Build();
        }

        /// <summary>
        /// 构建配置
        /// </summary>
        private static void Build()
        {
            // fixConfig
            List<HotfixConfig.FileInfo> fixFileInfos = new List<HotfixConfig.FileInfo>();
            var fixFileDirectory = FilePathUtils.Combine(Application.streamingAssetsPath, PlatformUtils.PlatformId());
            var fixFiles = Directory.GetFiles(fixFileDirectory, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < fixFiles.Length; i++)
            {
                var fp = fixFiles[i];
                var ext = Path.GetExtension(fp);
                var name = Path.GetFileName(fp);
                if (ext.EndsWith(".DS_Store"))
                    continue;
                if (ext.Equals(".meta"))
                    continue;
                if (ext.EndsWith(".manifest") && !name.Equals(PlatformUtils.PlatformId() + ".manifest"))
                    continue;

                var fixFileInfo = new HotfixConfig.FileInfo();
                fixFileInfo.path = fp.Replace(fixFileDirectory + Path.DirectorySeparatorChar, "").Replace(Path.DirectorySeparatorChar, '/'); ;
                fixFileInfo.hash = AppUtils.MD5File(fp);
                fixFileInfo.version = buildConfig.fixVersion.ToResourceString();
                fixFileInfo.size = (int)FilePathUtils.FileSize(fp);
                fixFileInfos.Add(fixFileInfo);
            }

            fixConfig = new HotfixConfig();
            fixConfig.fileInfos = fixFileInfos.ToArray();
            fixConfig.version = buildConfig.fixVersion;

            Debug.Log("[Build Hotfix] -> 最新资源数量:" + fixFileInfos.Count);

            // config
            var configPath = FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath),
                 "Build", buildConfig.version.ToVersionString(), buildConfig.version.ToResourceString(), "HotfixConfig.json");
            config = AppUtils.ReadConfig<HotfixConfig>(configPath);
            if (config.fileInfos == null) config.fileInfos = new HotfixConfig.FileInfo[0];

            Debug.Log("[Build Hotfix] -> 待修复版本资源数量:" + config.fileInfos.Length);

            // 比对查找热更新资源
            Dictionary<string, HotfixConfig.FileInfo> configDic = new Dictionary<string, HotfixConfig.FileInfo>();
            for (int j = 0; j < config.fileInfos.Length; j++)
            {
                var info = config.fileInfos[j];
                configDic.Add(info.path, info);
            }

            List<HotfixConfig.FileInfo> newFileInfos = new List<HotfixConfig.FileInfo>();
            List<HotfixConfig.FileInfo> allFileInfos = new List<HotfixConfig.FileInfo>();
            for (int i = 0; i < fixConfig.fileInfos.Length; i++)
            {
                var fixInfo = fixConfig.fileInfos[i];
                if (configDic.ContainsKey(fixInfo.path))
                {
                    var info = configDic[fixInfo.path];
                    if (!fixInfo.hash.Equals(info.hash))
                    {
                        newFileInfos.Add(fixInfo);
                        allFileInfos.Add(fixInfo);
                    }
                    else allFileInfos.Add(info);
                }
                else
                {
                    newFileInfos.Add(fixInfo);
                    allFileInfos.Add(fixInfo);
                }
            }
            Debug.Log("[Build Hotfix] -> 可修复资源数量:" + newFileInfos.Count);

            // 资源打包zip
            var outPathRoot = FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath),
                "Build", buildConfig.fixVersion.ToVersionString(), buildConfig.fixVersion.ToResourceString());
            var zipPath = FilePathUtils.Combine(outPathRoot, "HotfixRes.zip");
            FilePathUtils.DirectoryClean(outPathRoot);

            List<string> files = new List<string>();
            List<string> fileParents = new List<string>();
            for (int i = 0; i < newFileInfos.Count; i++)
            {
                var filePath = FilePathUtils.ReplaceSeparator(FilePathUtils.Combine(Application.streamingAssetsPath, PlatformUtils.PlatformId(), newFileInfos[i].path), "/");
                files.Add(filePath);
                FileInfo info = new FileInfo(filePath);
                fileParents.Add(FilePathUtils.ReplaceSeparator(info.DirectoryName.Substring(fixFileDirectory.Length), "/").TrimStart('/'));
            }

            ZipUtils.Zip(files.ToArray(), fileParents.ToArray(), zipPath);

            // 生成新的热更新配置
            var newConfig = new HotfixConfig();
            newConfig.fileInfos = allFileInfos.ToArray();
            newConfig.version = buildConfig.fixVersion;
            newConfig.hotfixFileCount = newFileInfos.Count;
            newConfig.hotfixFileSize = (int)FilePathUtils.FileSize(zipPath);
            newConfig.hotfix = true;
            FilePathUtils.FileWriteAllText(FilePathUtils.Combine(outPathRoot, "HotfixConfig.json"), JsonMapper.ToJson(newConfig));

            Debug.Log("[Build Hotfix] -> 热更新资源构建完成");
        }
    }
}