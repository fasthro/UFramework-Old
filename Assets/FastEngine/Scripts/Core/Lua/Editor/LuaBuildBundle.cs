/*
 * @Author: fasthro
 * @Date: 2019-11-27 19:38:30
 * @Description: Lua 构建Bundle
 */
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FastEngine.Core;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Lua
{
    public class LuaBuildBundle
    {
        // lua 临时目录
        static string tempDirectory { get { return FilePathUtils.Combine(Application.dataPath, "LuaTemp"); } }
        // lua bundle build list
        static List<AssetBundleBuild> luaBundleBuilds = new List<AssetBundleBuild>();
        // 过滤掉无效文件
        static string[] invalids = new string[] { "UnityApi.lua" };


        /// <summary>
        /// 构建Lua Bundle
        /// </summary>
        public static void Build()
        {
            // 清理临时目录
            FilePathUtils.DirectoryClean(tempDirectory);

            // lua byte
            for (int i = 0; i < LuaConfig.luaDirectorys.Length; i++)
            {
                if (LuaConfig.luaByteMode)
                {
                    string sourceDir = LuaConfig.luaDirectorys[i];
                    string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
                    int len = sourceDir.Length;

                    if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
                    {
                        --len;
                    }
                    for (int j = 0; j < files.Length; j++)
                    {
                        string str = files[j].Remove(0, len);
                        string dest = tempDirectory + str + ".bytes";
                        string dir = Path.GetDirectoryName(dest);
                        Directory.CreateDirectory(dir);
                        EncodeLuaFile(files[j], dest);
                    }
                }
                else
                {
                    ToLuaMenu.CopyLuaBytesFiles(LuaConfig.luaDirectorys[i], tempDirectory);
                }
            }

            // 删除文件无用文件
            for (int i = 0; i < invalids.Length; i++)
            {
                var fp = FilePathUtils.Combine(tempDirectory, invalids[i] + ".bytes");
                FilePathUtils.DeleteFile(fp);
            }

            // create assetbundle
            luaBundleBuilds.Clear();
            // 所有目录单独打包
            string[] dirs = Directory.GetDirectories(tempDirectory, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirs.Length; i++)
            {
                string name = dirs[i].Replace(tempDirectory, string.Empty);
                name = name.Replace('\\', '_').Replace('/', '_');
                name = "lua/lua" + name.ToLower() + ".unity3d";

                string path = "Assets" + dirs[i].Replace(Application.dataPath, "");
                CreateLuaAb(name, "*.bytes", path);
            }
            // 单个文件单独打到一个包内
            var ts = FilePathUtils.GetTopDirectory(Application.dataPath);
            CreateLuaAb("lua/lua.unity3d", "*.bytes", tempDirectory.Substring(ts.Length, tempDirectory.Length - ts.Length));

            AssetDatabase.Refresh();

            // build bundle
            var outPath = FilePathUtils.Combine(Application.streamingAssetsPath, "luaBundle");
            FilePathUtils.DirectoryClean(outPath);

            BuildPipeline.BuildAssetBundles(outPath,
                luaBundleBuilds.ToArray(),
                BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle,
                EditorUserBuildSettings.activeBuildTarget);
            // copy to bundle directory
            FilePathUtils.DirectoryDelete(FilePathUtils.Combine(AppUtils.BundleRootDirectory(), "lua"));
            FilePathUtils.DirectoryCopy(outPath, AppUtils.BundleRootDirectory(), false);
            FilePathUtils.DeleteFile(FilePathUtils.Combine(AppUtils.BundleRootDirectory(), "luaBundle"));
            FilePathUtils.DeleteFile(FilePathUtils.Combine(AppUtils.BundleRootDirectory(), "luaBundle.manifest"));
            // 清理临时目录
            FilePathUtils.DirectoryDelete(outPath);
            FilePathUtils.DirectoryDelete(tempDirectory);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建 Lua AssetBundleBuild
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="pattern"></param>
        /// <param name="path"></param>
        private static void CreateLuaAb(string bundleName, string pattern, string path)
        {
            string[] files = Directory.GetFiles(path, pattern);
            if (files.Length == 0) return;

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetNames = files;
            luaBundleBuilds.Add(build);
        }

        public static void EncodeLuaFile(string srcFile, string outFile)
        {
            if (!srcFile.ToLower().EndsWith(".lua"))
            {
                File.Copy(srcFile, outFile, true);
                return;
            }
            bool isWin = true;
            string luaexe = string.Empty;
            string args = string.Empty;
            string exedir = string.Empty;
            string currDir = Directory.GetCurrentDirectory();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                isWin = true;
                luaexe = "luajit.exe";
                args = "-b " + srcFile + " " + outFile;
                exedir = FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "Tools/LuaEncoder/luajit/");
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                isWin = false;
                luaexe = "./luac";
                args = "-o " + outFile + " " + srcFile;
                exedir = FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath), "Tools/LuaEncoder/luavm/");
            }
            Directory.SetCurrentDirectory(exedir);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = luaexe;
            info.Arguments = args;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.ErrorDialog = true;
            info.UseShellExecute = isWin;

            Process pro = Process.Start(info);
            pro.WaitForExit();
            Directory.SetCurrentDirectory(currDir);
        }
    }
}