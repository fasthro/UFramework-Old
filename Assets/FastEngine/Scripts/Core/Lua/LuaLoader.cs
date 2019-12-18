/*
 * @Author: fasthro
 * @Date: 2019-11-28 10:07:41
 * @Description: Lua 加载，集成自LuaFileUtils，重写里面的ReadFile，
 */
using UnityEngine;
using System.IO;
using LuaInterface;

namespace FastEngine.Core
{
    public class LuaLoader : LuaFileUtils
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            beZip = App.runModel == AppRunModel.Release;
            if (beZip)
            {
                var path = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), "lua");
                var files = Directory.GetFiles(path, "*.unity3d", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++)
                {
                    AddBundle("lua/" + FilePathUtils.GetFileName(files[i], true));
                }
            }
            else
            {
                for (int i = 0; i < LuaConfig.luaDirectorys.Length; i++)
                {
                    var directory = LuaConfig.luaDirectorys[i];

                    if (!Directory.Exists(directory))
                    {
                        string msg = string.Format("lua path not exists: {0}, configer it in FastEngine LuaConfig.cs", directory);
                        throw new LuaException(msg);
                    }

                    AddSearchPath(ToPackagePath(directory));
                }
            }
        }

        /// <summary>
        /// 添加打入Lua代码的AssetBundle
        /// </summary>
        /// <param name="bundle"></param>  
        public void AddBundle(string bundleName)
        {
            string url = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), bundleName.ToLower());
            if (File.Exists(url))
            {
                var bytes = File.ReadAllBytes(url);
                AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
                if (bundle != null)
                {
                    bundleName = bundleName.Replace("lua/", "").Replace(".unity3d", "");
                    base.AddSearchBundle(bundleName.ToLower(), bundle);
                }
            }
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName)
        {
            return base.ReadFile(fileName);
        }


        static string ToPackagePath(string path)
        {
            using (CString.Block())
            {
                CString sb = CString.Alloc(256);
                sb.Append(path);
                sb.Replace('\\', '/');

                if (sb.Length > 0 && sb[sb.Length - 1] != '/')
                {
                    sb.Append('/');
                }

                sb.Append("?.lua");
                return sb.ToString();
            }
        }
    }
}