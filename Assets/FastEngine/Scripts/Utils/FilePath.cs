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

        /// <summary>
        /// 获取路径上的第几个位置内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetPathSection(string path, int index)
        {
            if (index == 0)
                return "";

            path = ReplaceSeparator(path);
            char separator = Path.AltDirectorySeparatorChar;
            string[] ps = path.Split(separator);

            if (index < 0)
            {
                index = ps.Length + index + 1;
            }

            if (ps.Length >= index)
            {
                return ps[index - 1];
            }

            return "";
        }

        /// <summary>
        /// 替换路径分隔符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplaceSeparator(string path, string separator = "")
        {
            if (string.IsNullOrEmpty(separator))
            {
                separator = Path.AltDirectorySeparatorChar.ToString();
            }
            return path.Replace("\\", separator).Replace("//", separator);
        }
    }
}