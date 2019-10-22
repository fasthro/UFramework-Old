/*
 * @Author: fasthro
 * @Date: 2019-10-16 19:25:15
 * @Description: 消息包(小字节序方式读取和写入)
 */

using System;
using System.IO;
using System.Text;
using Google.Protobuf;

namespace FastEngine.Core
{
    /// <summary>
    /// socket pack type
    /// </summary>
    public enum SocketPackType
    {
        Stream,
        Protobuf,
    }

    public class SocketPack
    {
        // 协议号
        public int cmd { get; private set; }

        // 数据
        private byte[] m_data;
        public byte[] data
        {
            get
            {
                // stream
                if (m_data == null && m_writer != null && packType == SocketPackType.Stream)
                {
                    m_stream.Flush();
                    m_data = m_stream.ToArray();
                }
                // protobuf 
                else if (m_data == null && m_message != null && packType == SocketPackType.Protobuf)
                {
                    m_data = m_message.ToByteArray();
                }
                return m_data;
            }
        }

        // 数据长度
        public int dataSize { get { return data.Length; } }

        // socket pack type
        public SocketPackType packType { get; private set; }

        private MemoryStream m_stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;

        // pro
        private IMessage m_message;
        public T GetMessage<T>() where T : class, IMessage
        {
            if (m_message != null)
                return m_message as T;
            return null;
        }

        /// <summary>
        /// 协议包
        /// </summary>
        /// <param name="data"></param>
        public SocketPack(int cmd)
        {
            this.cmd = cmd;
            this.packType = SocketPackType.Stream;
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        /// <summary>
        /// 协议包
        /// </summary>
        /// <param name="data"></param>
        public SocketPack(int cmd, byte[] data)
        {
            this.cmd = cmd;
            this.m_data = data;
            this.packType = SocketPackType.Stream;
            m_stream = new MemoryStream(data);
            m_reader = new BinaryReader(m_stream);
        }

        /// <summary>
        /// 协议包
        /// </summary>
        /// <param name="message">protobuf message</param>
        public SocketPack(int cmd, IMessage message)
        {
            this.cmd = cmd;
            this.m_message = message;
            this.packType = SocketPackType.Protobuf;
        }

        /// <summary>
        /// 释放包
        /// </summary>
        public void Release()
        {
            if (m_stream != null) m_stream.Close();
            m_stream = null;

            if (m_writer != null) m_writer.Close();
            m_writer = null;

            if (m_reader != null) m_reader.Close();
            m_reader = null;

            this.m_data = null;
        }

        /// <summary>
        /// 判断字节序是否需要翻转
        /// </summary>
        private byte[] EndianReverse(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data;
        }

        #region write
        public void WriteByte(byte data)
        {
            m_writer.Write(data);
        }

        public void WriteBytes(byte[] data, bool alone = true)
        {
            if (alone)
                m_writer.Write(data.Length);
            m_writer.Write(data);
        }

        public void WriteInt(int data)
        {
            m_writer.Write(data);
        }

        public void WriteUint(uint data)
        {
            m_writer.Write(data);
        }

        public void WriteShort(short data)
        {
            m_writer.Write(data);
        }

        public void WriteUshort(ushort data)
        {
            m_writer.Write(data);
        }

        public void WriteLong(long data)
        {
            m_writer.Write(data);
        }

        public void WriteUlong(ulong data)
        {
            m_writer.Write(data);
        }

        public void WriteBoolean(bool data)
        {
            m_writer.Write(data);
        }

        public void WriteFloat(float data)
        {
            byte[] temp = EndianReverse(BitConverter.GetBytes(data));
            m_writer.Write(temp.Length);
            m_writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double data)
        {
            byte[] temp = EndianReverse(BitConverter.GetBytes(data));
            m_writer.Write(temp.Length);
            m_writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string data)
        {
            byte[] temp = Encoding.UTF8.GetBytes(data);
            m_writer.Write(temp.Length);
            m_writer.Write(temp);
        }

        public void WriteUnicodeString(string data)
        {
            byte[] temp = Encoding.Unicode.GetBytes(data);
            m_writer.Write(temp.Length);
            m_writer.Write(temp);
        }
        #endregion

        #region read
        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public byte[] ReadBytes()
        {
            return m_reader.ReadBytes(ReadInt());
        }

        public int ReadInt()
        {
            return m_reader.ReadInt32();
        }

        public uint ReadUint()
        {
            return m_reader.ReadUInt32();
        }

        public short ReadShort()
        {
            return m_reader.ReadInt16();
        }

        public ushort ReadUshort()
        {
            return m_reader.ReadUInt16();
        }

        public long ReadLong()
        {
            return m_reader.ReadInt64();
        }

        public ulong ReadUlong()
        {
            return m_reader.ReadUInt64();
        }

        public bool ReadBoolean()
        {
            return m_reader.ReadBoolean();
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(EndianReverse(ReadBytes()), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(EndianReverse(ReadBytes()), 0);
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes());
        }

        public string ReadUnicodeString()
        {
            return Encoding.Unicode.GetString(ReadBytes());
        }
        #endregion
    }
}
