/*
 * @Author: fasthro
 * @Date: 2019-11-28 19:13:24
 * @Description: 解压任务
 */
using System.IO;

namespace FastEngine.Core
{
    public class UnzipTreadTask : IThreadTask
    {
        private string m_sourcePath;
        private Stream m_stream;
        private string m_outPath;
        private ZipUtils.UnzipCallback m_callback;

        /// <summary>
        /// 解压任务
        /// </summary>
        /// <param name="sourcePath">资源路径</param>
        /// <param name="outPath">输出目录</param>
        /// <param name="callback">回调</param>
        public UnzipTreadTask(string sourcePath, string outPath, ZipUtils.UnzipCallback callback)
        {
            m_sourcePath = sourcePath;
            m_outPath = outPath;
            m_callback = callback;
        }

        /// <summary>
        /// 解压任务
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="outPath"></param>
        /// <param name="callback"></param>
        public UnzipTreadTask(Stream stream, string outPath, ZipUtils.UnzipCallback callback)
        {
            m_stream = stream;
            m_outPath = outPath;
            m_callback = callback;
        }

        public void OnExecute()
        {
            if (m_stream != null) ZipUtils.UnzipFile(m_stream, m_outPath, null, m_callback);
            else ZipUtils.UnzipFile(m_sourcePath, m_outPath, null, m_callback);
        }
    }
}