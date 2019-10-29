/*
 * @Author: fasthro
 * @Date: 2019-10-28 17:36:15
 * @Description: 文件/路径工具
 */
using System.IO;

namespace FastEngine.Utils
{
    public static class FilePath
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 路径连接
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            if (paths.Length == 2) return Path.Combine(paths[0], paths[1]);
            else if (paths.Length == 3) return Path.Combine(paths[0], paths[1], paths[2]);
            else if (paths.Length == 4) return Path.Combine(paths[0], paths[1], paths[2], paths[3]);
            else return Path.Combine(paths);
        }
    }
}