using System;
using FastEngine.Common;
using UnityEngine;

/*
 * @Author: fasthro
 * @Date: 2019-08-08 17:11:51
 * @Description: 接受者
 */
namespace FastEngine.Core
{
    [System.Serializable]
    public class SocketReceiver
    {
        // 接收到的数据
        private byte[] m_bytes;
        // 游标索引
        private int m_bytesCursor = 0;
        
        // 正在处理包
        private bool m_isProcessing;
        // 包长度
        private int m_packSize;
        // 命令
        private int m_packCmd;

        public SocketReceiver() { m_bytes = new byte[0]; }

        /// <summary>
        /// 接收的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        public void Push(byte[] data, int dataSize)
        {
            // 首先确保数据可以完全写入到 bytes 中
            // 空闲 Size
            int freeSize = m_bytes.Length - m_bytesCursor;
            if (freeSize < dataSize)
            {
                // 先扩大自身长度的2倍，如果还不够就扩大道最终需要的长度的2倍
                int tSize = FixSize(m_bytes.Length) * 2;

                // Push新数据之后的 bytes 总长度
                int needSize = m_bytesCursor + dataSize;

                if (needSize > tSize)
                {
                    tSize = FixSize(needSize) * 2;
                }

                // 当前数据Copy到扩容后的 bytes 中
                byte[] newBytes = new byte[tSize];
                Array.Copy(m_bytes, 0, newBytes, 0, m_bytesCursor);
                m_bytes = newBytes;
            }

            Array.Copy(data, 0, m_bytes, m_bytesCursor, dataSize);
            m_bytesCursor += dataSize;
        }

        /// <summary>
        /// 尝试获取包
        /// </summary>
        public SocketPack TryGetPack()
        {
            if (!m_isProcessing)
            {
                if (m_bytesCursor >= SocketPack.PACK_HEAD_SIZE)
                {
                    // 长度
                    byte[] tempBytes = new byte[SocketPack.PACK_HEAD_SPLIT_SIZE];
                    Array.Copy(m_bytes, 0, tempBytes, 0, SocketPack.PACK_HEAD_SPLIT_SIZE);
                    m_packSize = BitConverter.ToInt32(tempBytes, 0);

                    // 头
                    tempBytes = new byte[SocketPack.PACK_HEAD_SPLIT_SIZE];
                    Array.Copy(m_bytes, SocketPack.PACK_HEAD_SPLIT_SIZE, tempBytes, 0, SocketPack.PACK_HEAD_SPLIT_SIZE);

                    // 命令
                    tempBytes = new byte[SocketPack.PACK_HEAD_SPLIT_SIZE];
                    Array.Copy(m_bytes, SocketPack.PACK_HEAD_SPLIT_SIZE * 2, tempBytes, 0, SocketPack.PACK_HEAD_SPLIT_SIZE);
                    m_packCmd = BitConverter.ToInt32(tempBytes, 0);

                    m_isProcessing = true;
                }
            }

            if (m_isProcessing)
            {
                if (m_bytesCursor >= m_packSize)
                {
                    // copy pack data
                    byte[] packData = new byte[m_packSize];
                    Array.Copy(m_bytes, SocketPack.PACK_HEAD_SIZE, packData, 0, m_packSize - SocketPack.PACK_HEAD_SIZE);
                    
                    // delete rec pack data
                    m_bytesCursor -= m_packSize;
                    byte[] newBytes = new byte[m_bytesCursor];
                    Array.Copy(m_bytes, m_packSize, newBytes, 0, m_bytesCursor);
                    m_bytes = newBytes;

                    m_isProcessing = false;

                    return SocketPackFactory.CreateReader(m_packCmd, packData);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据value，确定大于此Size的最近的2次方数，如size=7，则返回值为8；size=12，则返回16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int FixSize(int value)
        {
            if (value == 0) return 1;
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }
    }
}