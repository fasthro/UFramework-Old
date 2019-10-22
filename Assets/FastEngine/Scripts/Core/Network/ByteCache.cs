/*
 * @Author: fasthro
 * @Date: 2019-10-22 17:31:00
 * @Description: byte 池(自动扩容)
 */

using System;

namespace FastEngine.Core
{
    public class ByteCache
    {
        private readonly static int DEFAULT_CACHE_SIZE = 256;
        // 池
        private byte[] m_cache;
        // 游标索引
        private int m_cursor = 0;
        // 读取的位置
        private int m_readCursor = 0;
        // 池大小
        public int size { get { return m_cursor; } }

        public ByteCache() { m_cache = new byte[DEFAULT_CACHE_SIZE]; }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        public void Write(byte[] data, int dataSize)
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
        /// 读取
        /// </summary>
        /// <param name="readSize"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        public byte[] Read(int readSize, ref bool succeed)
        {
            succeed = m_readCursor + readSize <= size;
            if (!succeed) return null;

            byte[] temp = new byte[readSize];
            Array.Copy(this.m_cache, m_readCursor, temp, 0, readSize);

            this.m_readCursor += readSize;
            this.m_cursor -= readSize;

            return temp;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            m_cache = new byte[DEFAULT_CACHE_SIZE];
            m_cursor = 0;
            m_readCursor = 0;
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