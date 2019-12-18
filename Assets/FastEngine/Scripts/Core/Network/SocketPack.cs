/*
 * @Author: fasthro
 * @Date: 2019-10-16 19:25:15
 * @Description: 消息包(小字节序方式读取和写入)
 */

using System;
using System.IO;
using System.Text;
using Google.Protobuf;
using LuaInterface;

namespace FastEngine.Core
{
    /// <summary>
    /// socket pack type
    /// </summary>
    public enum SocketPackType
    {
        Stream,
        Proto,
        LuaProto,
    }

    public class SocketPack
    {
        /// <summary>
        /// 使用 Proto
        /// </summary>
        public readonly static bool UseProto = true;

        // 协议号
        public int cmd { get; private set; }
        public int sessionId { get; private set; }

        // 数据
        private byte[] m_data;
        public byte[] data
        {
            get
            {
                if (!m_isReadPack)
                {
                    // stream
                    if (m_data == null && m_writer != null && packType == SocketPackType.Stream)
                    {
                        m_stream.Flush();
                        m_data = m_stream.ToArray();
                    }
                    // c# protobuf 
                    else if (m_data == null && m_message != null && packType == SocketPackType.Proto)
                    {
                        m_data = m_message.ToByteArray();
                    }
                    // lua protobuf 
                    else if (m_data == null && packType == SocketPackType.LuaProto)
                    {
                        m_data = this.luapb.buffer;
                    }
                }
                return m_data;
            }
        }

        // 数据长度
        public int dataSize { get { return data.Length; } }

        // socket pack type
        public SocketPackType packType { get; private set; }
        // 操作类型(true-读取/false-写入)
        private bool m_isReadPack;

        private MemoryStream m_stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;

        // c# proto
        private IMessage m_message;
        public T GetMessage<T>() where T : class, IMessage, new()
        {
            if (m_isReadPack)
            {
                m_message = new T();
                m_message.MergeFrom(data);

            }
            if (m_message != null) return m_message as T;
            return null;
        }

        // lua pd data
        private LuaByteBuffer m_luapb;
        public LuaByteBuffer luapb
        {
            get
            {
                if (m_isReadPack)
                {
                    m_luapb = new LuaByteBuffer(data);
                }
                return m_luapb;
            }
        }

        // 是否为有效消息(当前收到的消息是否为最后发送的消息,是个标志)
        public bool valid = true;

        // 标志错误消息
        public bool error { get; private set; }
        // 错误码
        public int errorCode { get; private set; }

        /// <summary>
        /// 写入协议包
        /// </summary>
        /// <param name="data"></param>
        public SocketPack(int cmd)
        {
            this.cmd = cmd;
            this.packType = SocketPackType.Stream;
            this.m_isReadPack = false;
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        /// <summary>
        /// C# Proto 写入协议包
        /// </summary>
        /// <param name="message">protobuf message</param>
        public SocketPack(int cmd, IMessage message)
        {
            this.cmd = cmd;
            this.m_message = message;
            this.packType = SocketPackType.Proto;
            this.m_isReadPack = false;
        }

        /// <summary>
        /// Lua Proto 写入协议包
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="luabyte"></param>
        public SocketPack(int cmd, LuaByteBuffer luabyte)
        {
            this.cmd = cmd;
            this.m_luapb = luabyte;
            this.packType = SocketPackType.LuaProto;
            this.m_isReadPack = false;
        }

        /// <summary>
        /// 读取协议包
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sessionId"></param>
        /// <param name="data"></param>
        public SocketPack(int cmd, int sessionId, byte[] data)
        {
            this.cmd = cmd;
            this.sessionId = sessionId;
            this.m_data = data;
            this.packType = UseProto ? SocketPackType.Proto : SocketPackType.Stream;
            this.m_isReadPack = true;
            if (!UseProto)
            {
                m_stream = new MemoryStream(data);
                m_reader = new BinaryReader(m_stream);
            }
        }

        /// <summary>
        /// 创建错误包(游戏需求)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="code"></param>
        public SocketPack(int cmd, int code)
        {
            this.cmd = cmd;
            this.error = true;
            this.errorCode = code;
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
        /// send to server
        /// </summary>
        public void Send()
        {
            if (!m_isReadPack) TCPSession.Send(this);
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
