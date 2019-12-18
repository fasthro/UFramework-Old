/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:22:17
 * @Description: FUI package
 */
using FairyGUI;
using FastEngine.Core;
using FastEngine.Utils;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FastEngine.FUI
{
    /// <summary>
    /// package
    /// </summary>
    public class FPack : SimpleRef
    {
        public string name { get; private set; }
        public UIPackage package { get; private set; }
        private AssetBundleLoader m_assetBundleLoader;

        public FPack(string name)
        {
            this.name = name;
        }

        public void Load()
        {
            if (package != null) return;

            if (App.runModel == AppRunModel.Develop)
            {
#if UNITY_EDITOR
                // 开发模式直接读取UI包
                var assetPath = FilePathUtils.Combine("Assets/Art/UI", string.Format("{0}/{1}", name, name));
                package = UIPackage.AddPackage(assetPath,
                    (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                    {
                        destroyMethod = DestroyMethod.Unload;
                        return AssetDatabase.LoadAssetAtPath(name + extension, type);
                    }
                );
#endif
            }
            else
            {
                // 正式模式使用 assetBundle 加载
                m_assetBundleLoader = AssetBundleLoader.Allocate("ui/" + name, null, null);
                var ready = m_assetBundleLoader.LoadSync();
                if (ready) package = UIPackage.AddPackage(m_assetBundleLoader.bundleRes.assetBundle);
                else Debug.LogError("FPack AddPackage Error! PackageName:" + name);
            }
        }

        protected override void OnZeroRef()
        {
            if (package != null)
                UIPackage.RemovePackage(package.id);
        }
    }

    /// <summary>
    /// package service
    /// </summary>
    public class FPackService
    {
        static Dictionary<string, FPack> map = new Dictionary<string, FPack>();

        public static void Add(string[] packNames)
        {
            for (int i = 0; i < packNames.Length; i++)
            {
                Add(packNames[i]);
            }
        }

        public static UIPackage Add(string packName)
        {
            FPack pack = null;
            if (map.TryGetValue(packName, out pack))
            {
                pack.Retain();
            }
            else
            {
                pack = new FPack(packName);
                pack.Load();
                pack.Retain();
                map.Add(packName, pack);
            }

            return pack.package;
        }

        public static void Remove(string[] packNames)
        {
            for (int i = 0; i < packNames.Length; i++)
            {
                Remove(packNames[i]);
            }
        }

        public static void Remove(string packName)
        {
            FPack pack = null;
            if (map.TryGetValue(packName, out pack))
            {
                pack.Release();
                if (pack.isRefZero) map.Remove(packName);
            }
        }
    }
}