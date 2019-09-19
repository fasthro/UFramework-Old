/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:47:44
 * @Description: 二进制流
 */
using System;
using System.IO;
using System.Text;

namespace FastEngine.Core
{
    public class ByteBuffer
    {
        private MemoryStream m_stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;

        /// <summary>
        /// 创建写入二进制流
        /// </summary>
        public static ByteBuffer CreateWriter()
        {
            return new ByteBuffer(null);
        }

        /// <summary>
        /// 创建读取二进制流
        /// </summary>
        /// <param name="data"></param>
        public static ByteBuffer CreateReader(byte[] data)
        {
            return new ByteBuffer(data);
        }

        public ByteBuffer(byte[] data)
        {
            if (data != null) InitReader(data);
            else InitWriter();
        }

        /// <summary>
        /// ToArray
        /// </summary>
        public byte[] ToArray()
        {
            m_stream.Flush();
            return m_stream.ToArray();
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            if (m_stream != null) m_stream.Close();
            m_stream = null;

            if (m_writer != null) m_writer.Close();
            m_writer = null;

            if (m_reader != null) m_reader.Close();
            m_reader = null;
        }

        /// <summary>
        /// 初始化写
        /// </summary>
        private void InitWriter()
        {
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        /// <summary>
        /// 初始化读
        /// </summary>
        private void InitReader(byte[] data)
        {
            m_stream = new MemoryStream(data);
            m_reader = new BinaryReader(m_stream);
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