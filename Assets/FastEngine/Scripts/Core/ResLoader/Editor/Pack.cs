/*
 * @Author: fasthro
 * @Date: 2019-11-09 13:54:28
 * @Description: AssetBundle Pack
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FastEngine.Core;
using FastEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.AssetBundle
{
    /// <summary>
    /// 打包方式
    /// </summary>
    public enum BuildModel
    {
        Standard,         // 标准形式
        File,             // 单一文件打包
        Folder,           // 整个文件夹打包
        FolderChild,      // 子文件夹打包
    }

    public enum GenerateMapping
    {
        Generate,
        NotGenerate,
    }

    public class Pack
    {
        // 打包方式
        public BuildModel model = BuildModel.Standard;
        // 是否生成映射
        public GenerateMapping genMapping = GenerateMapping.Generate;
        // 目标路径
        public string target;
        // 匹配规则
        public string pattern = "*.*";
        // bundle path
        public string bundlePath;
        // bundle name
        public string bundleName;
        // 映射配置
        public Dictionary<string, AssetBundleMappingData> mapping = new Dictionary<string, AssetBundleMappingData>();

        // 在编辑器中是否显示
        public bool editorShow = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structure">结构</param>
        /// <param name="target">目标路径</param>
        /// <param name="bundlePath">bundle输出路径</param>
        /// <param name="genMapping">是否生成mapping配置</param>
        /// <param name="pattern">匹配模式</param>
        /// <returns></returns>
        public static Pack Create(BuildModel structure, string target, string bundlePath, GenerateMapping genMapping, string pattern = "*.*")
        {
            var pack = new Pack();
            pack.model = structure;
            pack.target = target;
            pack.bundlePath = bundlePath;
            pack.genMapping = genMapping;
            pack.pattern = pattern;
            return pack;
        }

        /// <summary>
        /// 构建
        /// </summary>
        public void Build()
        {
            mapping.Clear();

            switch (model)
            {
                case BuildModel.Standard:
                    BuildStandard();
                    break;
                case BuildModel.File:
                    BuildFile();
                    break;
                case BuildModel.Folder:
                    BuildFolder();
                    break;
                case BuildModel.FolderChild:
                    BuildFolderChild();
                    break;
                default:
                    break;
            }
        }

        private void BuildStandard()
        {
            string[] typeDirs = Directory.GetDirectories(target, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < typeDirs.Length; i++)
            {
                var typeDir = typeDirs[i];
                var _type = FilePath.GetPathSection(typeDir, -1);
                var dir = Path.Combine(typeDir, "Prefab");
                if (Directory.Exists(dir))
                {
                    string[] files = Directory.GetFiles(dir, pattern, SearchOption.AllDirectories);
                    for (int index = 0; index < files.Length; index++)
                    {
                        var abName = Path.Combine(bundlePath, _type);
                        SetBundleName(files[index], abName);
                    }
                }
            }
        }

        private void BuildFile()
        {
            if (File.Exists(target))
            {
                var abName = Path.Combine(bundlePath, bundleName);
                SetBundleName(target, abName);
            }
        }

        private void BuildFolder()
        {
            if (Directory.Exists(target))
            {
                string[] files = Directory.GetFiles(target, pattern, SearchOption.AllDirectories);
                for (int index = 0; index < files.Length; index++)
                {
                    var abName = Path.Combine(bundlePath, bundleName);
                    SetBundleName(files[index], abName);
                }
            }
        }

        private void BuildFolderChild()
        {
            string[] typeDirs = Directory.GetDirectories(target, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < typeDirs.Length; i++)
            {
                var typeDir = typeDirs[i];
                var _type = FilePath.GetPathSection(typeDir, -1);
                string[] files = Directory.GetFiles(typeDir, pattern, SearchOption.AllDirectories);
                for (int index = 0; index < files.Length; index++)
                {
                    var abName = Path.Combine(bundlePath, _type);
                    SetBundleName(files[index], abName);
                }
            }
        }

        private void SetBundleName(string filePath, string bundleName)
        {
            AssetImporter import = AssetImporter.GetAtPath(filePath);
            if (import != null)
            {
                Debug.Log("AssetBundle -> " + import.assetPath + " -> " + bundleName + " -> " + Path.GetFileName(import.assetPath));
                import.assetBundleName = bundleName;

                // 添加配置
                var rp = import.assetPath.Substring("Assets/".Length);
                var pxIndex = rp.LastIndexOf('.');
                if (pxIndex > 0)
                {
                    rp = rp.Substring(0, pxIndex);
                }
                if (!mapping.ContainsKey(rp))
                {
                    var data = new AssetBundleMappingData();
                    data.bundleName = import.assetBundleName;
                    data.resName = Path.GetFileName(import.assetPath);
                    mapping.Add(rp, data);
                }
            }
        }
    }
}