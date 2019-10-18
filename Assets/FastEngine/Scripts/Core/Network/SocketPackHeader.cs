/*
 * @Author: fasthro
 * @Date: 2019-10-18 14:02:12
 * @Description: 消息包头(小字节序方式读取和写入)
 */
using System;
using System.IO;
using UnityEngine;

namespace FastEngine.Core
{
    public class SocketPackHeader
    {
        #region config
        /// <summary>
        /// 包头总大小[协议长度(4个字节)][协议头(4个字节)][协议号(4个字节)]
        /// </summary>
        public readonly static int HEADER_SIZE = 12;

        /// <summary>
        /// 包头分割大小
        /// </summary>
        public readonly static int SPLIT_SIZE = 4;
        #endregion


        // 包长度
        public int packSize { get; private set; }
        // 命令
        public int cmd { get; private set; }
        // 预留字节
        public byte[] reserved { get; private set; }

        private MemoryStream m_stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;

        /// <summary>
        /// 读取头
        /// </summary>
        /// <param name="data"></param>
        public void Read(byte[] data)
        {
            m_stream = new MemoryStream(data);
            m_reader = new BinaryReader(m_stream);

            packSize = m_reader.ReadInt32();
            reserved = m_reader.ReadBytes(4);
            cmd = m_reader.ReadInt32();

            m_stream.Flush();
            m_stream.Close();
            m_reader.Close();
        }

        /// <summary>
        /// 写入头
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        public byte[] Write(int cmd, byte[] data)
        {
            this.packSize = HEADER_SIZE + data.Length;
            this.cmd = cmd;

            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);

            // 包长度(总长度不包含前4个字节)
            m_writer.Write(this.packSize - SPLIT_SIZE);

            m_writer.Write('F');
            m_writer.Write('A');
            m_writer.Write('S');
            m_writer.Write('T');

            m_writer.Write(this.cmd);

            m_writer.Write(data);

            m_stream.Flush();
            var nbs = m_stream.ToArray();
            m_stream.Close();
            m_writer.Close();
            
            return nbs;
        }
    }
}