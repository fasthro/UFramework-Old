/*
 * @Author: fasthro
 * @Date: 2019-11-09 13:54:50
 * @Description: AssetBundle Editor
 */

using System.Collections.Generic;
using System.IO;
using AssetBundleBrowser.AssetBundleDataSource;
using FastEngine.Core;
using FastEngine.Utils;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.AssetBundle
{
    public class AssetBundleEditor
    {
        [MenuItem("FastEngine/AssetBundle -> 打开配置", false, 100)]
        static void OpenConfig()
        {
            AssetBundleCEW.Open<AssetBundleCEW>();
        }

        [MenuItem("FastEngine/AssetBundle -> 打包", false, 101)]
        public static void Build()
        {
            GenMapping();
            CopySource();
            string outPath = AppUtils.BundleRootDirectory();
            if (!Directory.Exists(outPath))
                Directory.CreateDirectory(outPath);

            ABBuildInfo buildInfo = new ABBuildInfo();
            buildInfo.outputDirectory = outPath;
            buildInfo.options = BuildAssetBundleOptions.ChunkBasedCompression;
            buildInfo.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            AssetBundleBrowser.AssetBundleModel.Model.DataSource.BuildAssetBundles(buildInfo);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        [MenuItem("FastEngine/AssetBundle -> 清理打包", false, 102)]
        public static void CleanBuild()
        {
            string outPath = AppUtils.BundleRootDirectory();
            if (Directory.Exists(outPath))
                Directory.Delete(outPath, true);

            Build();
        }

        [MenuItem("FastEngine/AssetBundle -> 生成映射配置", false, 103)]
        static void GenMapping()
        {
            StartGenMapping();
        }

        [MenuItem("FastEngine/AssetBundle -> Copy Source", false, 104)]
        public static void CopySource()
        {
            AssetBundleConfig config = AppUtils.LoadConfig<AssetBundleConfig>(AppUtils.EditorConfigPath("AssetBundleConfig"));
            for (int i = 0; i < config.sources.Count; i++)
            {
                var source = config.sources[i];
                if (File.Exists(source.source)) FilePathUtils.FileCopy(source.source, source.dest);
                else FilePathUtils.DirectoryCopy(source.source, FilePathUtils.Combine(AppUtils.BundleRootDirectory(), source.dest));
            }
        }

        static bool StartGenMapping()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetBundleConfig config = AppUtils.LoadConfig<AssetBundleConfig>(AppUtils.EditorConfigPath("AssetBundleConfig"));
            Dictionary<string, AssetBundleMappingData> mapingDic = new Dictionary<string, AssetBundleMappingData>();
            for (int i = 0; i < config.packs.Count; i++)
            {
                var pack = config.packs[i];
                pack.Build();
                if (pack.genMapping == GenerateMapping.Generate)
                {
                    foreach (KeyValuePair<string, AssetBundleMappingData> dataItem in pack.mapping)
                    {
                        mapingDic.Add(dataItem.Key, dataItem.Value);
                    }
                }
            }

            // 映射配置写入文件
            var mp = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), PlatformUtils.PlatformId() + ".json");
            if (File.Exists(mp))
                File.Delete(mp);
            if (!Directory.Exists(AppUtils.BundleRootDirectory()))
                Directory.CreateDirectory(AppUtils.BundleRootDirectory());
            File.WriteAllText(mp, JsonMapper.ToJson(mapingDic));
            return true;
        }
    }
}
