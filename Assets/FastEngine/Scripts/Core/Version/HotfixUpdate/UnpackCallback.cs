/*
 * @Author: fasthro
 * @Date: 2019-11-29 16:49:15
 * @Description: 解包回调
 */
using FastEngine.Utils;

namespace FastEngine.Core
{
    public class UnpackCallback : ZipUtils.UnzipCallback
    {
        // 要解压的文件最大数量
        private int m_maxCount;
        // 已经解压完成的文件数量
        private int m_count;
        // zip 资源路径
        private string m_sourcePath;
        // 解压完成删除zip资源
        private bool m_deleteSource;

        public float progress { get; private set; }
        public bool done { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCount">解压文件最大数量</param>
        public UnpackCallback(int maxCount)
        {
            m_maxCount = maxCount;
            this.m_deleteSource = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxCount"></param>
        /// <param name="sourcePath"></param>
        public UnpackCallback(int maxCount, string sourcePath)
        {
            this.m_maxCount = maxCount;
            this.m_sourcePath = sourcePath;
            this.m_deleteSource = true;
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="max">最大数量</param>
        public void Reset(int max)
        {
            this.m_maxCount = max;
            this.m_count = 0;
            this.progress = 0;
            this.done = false;
        }

        public override void OnPostUnzip(ICSharpCode.SharpZipLib.Zip.ZipEntry _entry)
        {
            m_count++;
            if (m_maxCount > 0) progress = ((float)m_count / (float)m_maxCount);
        }

        public override void OnFinished(bool _result)
        {
            done = true;
            progress = 1f;
            if (m_deleteSource) FilePathUtils.DeleteFile(m_sourcePath);
        }
    }
}
