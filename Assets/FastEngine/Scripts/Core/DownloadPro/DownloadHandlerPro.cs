/*
 * @Author: fasthro
 * @Date: 2019-11-30 14:21:29
 * @Description: download handler pro
 */
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace FastEngine.Core
{
    public delegate void DownloadEventCallback(float progress);
    public class DownloadHandlerPro : DownloadHandlerScript
    {
        public string url { get; private set; }
        public string savePath { get; private set; }
        public bool waiting { get; protected set; }
        public bool downloading { get; protected set; }
        public bool done { get; private set; }
        public float progress
        {
            get
            {
                if (receiveTotalSize > 0 && totalSize > 0)
                {
                    return ((float)receiveTotalSize / (float)totalSize);
                }
                return 0f;
            }
        }

        // 总大小
        public int totalSize { get; protected set; }
        // 剩余大小
        public int contentSize { get; protected set; }
        // 已经下载总大小
        public int receiveTotalSize { get; protected set; }
        // 本次下载总大小（如果未下载完成中断，此值与receiveTotalSize不相等）
        public int receiveSize { get; protected set; }

        // 文件流
        private FileStream m_fileStream;
        // 下载临时路径
        private string m_tempPath;
        // 文件名不带扩展
        private string m_fileName;
        // 文件扩展名
        private string m_fileExt;

        // 下载事件回调
        public event DownloadEventCallback eventCallback;

        // 断线续传 Rang Value
        public string headerRangeValue { get { return string.Format("bytes={0}-", receiveTotalSize); } }

        /// <summary>
        /// download handler
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="savePath">保存路径</param>
        public DownloadHandlerPro(string url, string savePath)
        {
            this.url = url;
            this.savePath = savePath;
            this.waiting = true;
            this.downloading = false;
            this.done = false;
            FilePathUtils.DeleteFile(savePath);
            FileInfo info = new FileInfo(savePath);
            this.m_fileName = FilePathUtils.GetFileName(info, false);
            this.m_fileExt = info.Extension;
            this.m_tempPath = FilePathUtils.Combine(info.DirectoryName, m_fileName + ".download");
            this.m_fileStream = new FileStream(m_tempPath, FileMode.Append, FileAccess.Write);
            this.receiveSize = 0;
            this.receiveTotalSize = (int)m_fileStream.Length;
            this.totalSize = receiveTotalSize;
        }

        /// <summary>
        /// 开始相下载
        /// </summary>
        public void Download()
        {
            this.waiting = false;
            this.downloading = true;
        }

        [System.Obsolete]
        protected override void ReceiveContentLength(int contentLength)
        {
            contentSize = contentLength;
            totalSize += contentLength;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || data.Length == 0) return false;
            receiveSize += dataLength;
            contentSize -= dataLength;
            receiveTotalSize += dataLength;
            m_fileStream.Write(data, 0, dataLength);
            return true;
        }

        protected override void CompleteContent()
        {
            Release();
            waiting = false;
            downloading = false;
            FilePathUtils.DeleteFile(savePath);
            File.Move(m_tempPath, savePath);
            done = true;
        }

        public void Release()
        {
            m_fileStream.Close();
            m_fileStream.Dispose();
        }
    }
}