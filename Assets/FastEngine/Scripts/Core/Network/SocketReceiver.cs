using System;

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
        // 数据池
        private byte[] m_cache;
        // 游标索引
        private int m_cursor = 0;

        // 正在处理包
        private bool m_isProcessing;
        // 包头
        private SocketPackHeader m_header;

        public SocketReceiver()
        {
            m_cache = new byte[0];
            m_header = new SocketPackHeader();
        }

        /// <summary>
        /// 接收的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        public void Push(byte[] data, int dataSize)
        {
            // 首先确保数据可以完全写入到 bytes 中
            // 空闲 Size
            int freeSize = m_cache.Length - m_cursor;
            if (freeSize < dataSize)
            {
                // 先扩大自身长度的2倍，如果还不够就扩大道最终需要的长度的2倍
                int tSize = FixSize(m_cache.Length) * 2;

                // Push新数据之后的 bytes 总长度
                int needSize = m_cursor + dataSize;

                if (needSize > tSize)
                {
                    tSize = FixSize(needSize) * 2;
                }

                // 当前数据Copy到扩容后的 bytes 中
                byte[] newBytes = new byte[tSize];
                Array.Copy(m_cache, 0, newBytes, 0, m_cursor);
                m_cache = newBytes;
            }

            Array.Copy(data, 0, m_cache, m_cursor, dataSize);
            m_cursor += dataSize;
        }

        /// <summary>
        /// 尝试获取包
        /// </summary>
        public SocketPack TryGetPack()
        {
            if (!m_isProcessing)
            {
                if (m_cursor >= SocketPackHeader.HEADER_SIZE)
                {
                    m_header.Read(m_cache);
                    m_isProcessing = true;
                }
            }

            if (m_isProcessing)
            {
                if (m_cursor >= m_header.packSize)
                {
                    // copy pack data
                    byte[] packData = new byte[m_header.packSize];
                    Array.Copy(m_cache, SocketPackHeader.HEADER_SIZE, packData, 0, m_header.packSize - SocketPackHeader.HEADER_SIZE);

                    // delete rec pack data
                    m_cursor -= m_header.packSize;
                    byte[] newBytes = new byte[m_cursor];
                    Array.Copy(m_cache, m_header.packSize, newBytes, 0, m_cursor);
                    m_cache = newBytes;

                    m_isProcessing = false;

                    return SocketPackFactory.CreateReader(m_header.cmd, packData);
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