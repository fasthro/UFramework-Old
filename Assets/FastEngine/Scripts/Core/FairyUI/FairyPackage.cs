/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:22:17
 * @Description: fairy ui package
 */
using FairyGUI;
using FastEngine.Core;
using FastEngine.Ref;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FastEngine.FairyUI
{
    /// <summary>
    /// fairy package
    /// </summary>
    public class Package : SimpleRef
    {
        public string name { get; private set; }
        public UIPackage package { get; private set; }
        private AssetBundleLoader m_assetBundleLoader;

        public Package(string name)
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
            if (package != null) UIPackage.RemovePackage(package.id);
        }
    }

    /// <summary>
    /// fairy package
    /// </summary>
    public class FairyPackage
    {
        static Dictionary<string, Package> map = new Dictionary<string, Package>();

        /// <summary>
        /// add packages
        /// </summary>
        /// <param name="pacagekNames"></param>
        public static void Add(string[] pacagekNames)
        {
            for (int i = 0; i < pacagekNames.Length; i++)
            {
                Add(pacagekNames[i]);
            }
        }

        /// <summary>
        /// add package
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static UIPackage Add(string packageName)
        {
            Package pack = null;
            if (map.TryGetValue(packageName, out pack))
                pack.Retain();
            else
            {
                pack = new Package(packageName);
                pack.Load();
                pack.Retain();
                map.Add(packageName, pack);
            }
            return pack.package;
        }

        /// <summary>
        /// remove packages
        /// </summary>
        /// <param name="packageNames"></param>
        public static void Remove(string[] packageNames)
        {
            for (int i = 0; i < packageNames.Length; i++)
            {
                Remove(packageNames[i]);
            }
        }

        /// <summary>
        /// remove package
        /// </summary>
        /// <param name="packageName"></param>
        public static void Remove(string packageName)
        {
            Package pack = null;
            if (map.TryGetValue(packageName, out pack))
            {
                pack.Release();
                if (pack.isRefZero) map.Remove(packageName);
            }
        }
    }
}