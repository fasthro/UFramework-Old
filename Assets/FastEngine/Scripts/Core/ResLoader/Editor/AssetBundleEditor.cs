/*
 * @Author: fasthro
 * @Date: 2019-11-09 13:54:50
 * @Description: AssetBundle Editor
 */

using System.Collections.Generic;
using System.IO;
using AssetBundleBrowser.AssetBundleDataSource;
using FastEngine.Core;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.AssetBundle
{
    public class AssetBundleEditor
    {
        [MenuItem("FastEngine/AssetBundle -> 打开配置" , false, 100)]
        static void OpenConfig()
        {
            ConfigEditorWindow.OpenWindow();
        }

        [MenuItem("FastEngine/AssetBundle -> 打包", false, 101)]
        static void Build()
        {
            GenMapping();

            string outPath = AssetBundlePath.RootDirectory();
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
        static void CleanBuild()
        {
            string outPath = AssetBundlePath.RootDirectory();
            if (Directory.Exists(outPath))
                Directory.Delete(outPath, true);

            Build();
        }

        [MenuItem("FastEngine/AssetBundle -> 生成映射配置", false, 103)]
        static void GenMapping()
        {
            StartGenMapping();
        }

        static bool StartGenMapping()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            List<Pack> packs = ConfigEditorWindow.LoadConfig();
            Dictionary<string, AssetBundleMappingData> mapingDic = new Dictionary<string, AssetBundleMappingData>();
            for (int i = 0; i < packs.Count; i++)
            {
                var pack = packs[i];
                pack.Build();
                Debug.Log(pack.mapping.Count);
                if (pack.genMapping == GenerateMapping.Generate)
                {
                    foreach (KeyValuePair<string, AssetBundleMappingData> dataItem in pack.mapping)
                    {
                        mapingDic.Add(dataItem.Key, dataItem.Value);
                    }
                }
            }

            // 映射配置写入文件
            var mp = AssetBundlePath.MappingFilePath();
            if (File.Exists(mp)) File.Delete(mp);
            if (!Directory.Exists(AssetBundlePath.RootDirectory())) Directory.CreateDirectory(AssetBundlePath.RootDirectory());
            File.WriteAllText(mp, JsonMapper.ToJson(mapingDic));
            return true;
        }
    }
}
