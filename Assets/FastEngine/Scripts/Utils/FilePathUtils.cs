/*
 * @Author: fasthro
 * @Date: 2019-10-28 17:36:15
 * @Description: 文件/路径工具
 */
using System.IO;

namespace FastEngine.Utils
{
    public static class FilePathUtils
    {
        #region 路径

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="path"></param>
        public static void DirectoryCreate(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// Delete Directory
        /// </summary>
        /// <param name="directory"></param>
        public static void DirectoryDelete(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        /// <summary>
        /// Clean Directory
        /// </summary>
        /// <param name="directory"></param>
        public static void DirectoryClean(string directory)
        {
            DirectoryDelete(directory);
            DirectoryCreate(directory);
        }

        /// <summary>
        /// Copy Directory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="clean">是否清理目标目录</param>
        public static void DirectoryCopy(string source, string dest, bool clean = true)
        {
            if (clean) DirectoryClean(dest);

            var files = Directory.GetFiles(source, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], files[i].Replace(source, dest));
            }

            var dirs = Directory.GetDirectories(source, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                var dir = dirs[i];
                var newDir = dest + dir.Replace(source, "");
                DirectoryCopy(dir, newDir);
            }
        }

        /// <summary>
        /// 路径连接
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            if (paths.Length == 2) return ReplaceSeparator(Path.Combine(paths[0], paths[1]), "/");
            else if (paths.Length == 3) return ReplaceSeparator(Path.Combine(paths[0], paths[1], paths[2]), "/");
            else if (paths.Length == 4) return ReplaceSeparator(Path.Combine(paths[0], paths[1], paths[2], paths[3]), "/");
            else return ReplaceSeparator(Path.Combine(paths), "/");
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
        /// 获取上级目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="index">默认上一级目录</param>
        /// <returns></returns>
        public static string GetTopDirectory(string directory, int index = 1)
        {
            directory = ReplaceSeparator(directory);
            char separator = Path.AltDirectorySeparatorChar;
            string[] ps = directory.Split(separator);

            if (ps.Length >= index)
            {
                string newDir = "";
                for (int i = 0; i < ps.Length - index; i++)
                {
                    newDir += ps[i] + Path.DirectorySeparatorChar;
                }
                return newDir;
            }
            return directory;
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

        /// <summary>
        /// 获取文件路径上的文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath, bool extension)
        {
            FileInfo info = new FileInfo(filePath);
            return extension ? info.Name : info.Name.Substring(0, info.Name.Length - info.Extension.Length);
        }

        /// <summary>
        /// 获取文件路径上的文件名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetFileName(FileInfo info, bool extension)
        {
            return extension ? info.Name : info.Name.Substring(0, info.Name.Length - info.Extension.Length);
        }

        #endregion

        #region 文件
        /// <summary>
        /// File WriteAllText
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool FileWriteAllText(string path, string context)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (!info.Directory.Exists) info.Directory.Create();
                if (info.Exists) info.Delete();

                File.WriteAllText(path, context);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// File ReadAllText
        /// </summary>
        /// <param name="path"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        public static string FileReadAllText(string path, out bool succeed)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    succeed = true;
                    return File.ReadAllText(path);
                }
            }
            catch
            {

            }
            succeed = false;
            return "";
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 文件 Exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 文件 Copy
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool FileCopy(string source, string dest)
        {
            try
            {
                FileInfo info = new FileInfo(dest);
                if (!Directory.Exists(info.DirectoryName))
                {
                    Directory.CreateDirectory(info.DirectoryName);
                }
                File.Copy(source, dest);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long FileSize(string path)
        {
            return (new FileInfo(path)).Length;
        }
        #endregion
    }
}