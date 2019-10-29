/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:22:17
 * @Description: FUI package
 */
using FairyGUI;
using FastEngine.Utils;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FastEngine.FUI
{
    /// <summary>
    /// package
    /// </summary>
    public class FPack : SimpleRef
    {
        public string name { get; private set; }
        public UIPackage package { get; private set; }

        public FPack(string name)
        {
            this.name = name;
        }

        public void Load()
        {
            if (this.package != null) return;

#if UNITY_EDITOR
            this.package = UIPackage.AddPackage(Path.Combine(FUIConfig.pakageExportDirectory, string.Format("{0}/{1}", name, name)),
                        (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                        {
                            destroyMethod = DestroyMethod.Unload;
                            return AssetDatabase.LoadAssetAtPath(name + extension, type);
                        }
                    );
#else
            // TODO Assetbundle
#endif
        }
    }

    /// <summary>
    /// package service
    /// </summary>
    public class FPackService
    {
        static Dictionary<string, FPack> map = new Dictionary<string, FPack>();

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