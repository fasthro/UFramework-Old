using System;
using UnityEngine;

/*
 * @Author: fasthro
 * @Date: 2019-08-08 17:11:51
 * @Description: 接收缓冲区
 */
namespace FastEngine.Core
{
    [System.Serializable]
    public class SocketReceiveBuffer
    {
        // 缓存区最小长度
        private int m_minSize;

        // buffer
        private byte[] m_buffer;

        // buffer size
        private int m_bufferSize;

        // 当前位置
        private int m_cp = 0;

        // 协议数据长度
        private int m_dataSize;

        // 协议号
        private int m_protocal;

        // 协议 session
        private int m_session;

        public SocketReceiveBuffer(int minSize = 1024)
        {
            if (minSize < 0) minSize = 1024;
            this.m_minSize = minSize;

            this.m_buffer = new byte[this.m_minSize];
        }

        /// <summary>
        /// 向缓冲区中添加数据，要添加的数据长度大于当前缓冲区长度，会自动扩充缓冲区容量
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void Push(byte[] data, int size)
        {
            if (size > m_buffer.Length - data.Length - m_cp)
            {
                byte[] temp = new byte[m_cp + size];
                Array.Copy(m_buffer, 0, temp, 0, m_cp);
                Array.Copy(data, 0, temp, m_cp, size);
                m_buffer = temp;
                temp = null;
            }
            else
            {
                Array.Copy(data, 0, m_buffer, m_cp, size);
            }
            m_cp += size;

            Debug.Log("push ------- :" + m_buffer.Length);

            ByteBuffer bf = ByteBuffer.CreateReader(m_buffer);
            
            Debug.Log("count ------- :" + bf.ReadInt());
            Debug.Log("R ------- :" + bf.ReadByte());
            Debug.Log("H ------- :" + bf.ReadByte());
            Debug.Log("E ------- :" + bf.ReadByte());
            Debug.Log("A ------- :" + bf.ReadByte());
            Debug.Log("Session ------- :" + bf.ReadInt());
        }

        /// <summary>
        /// 获取完整的一条协议数据
        /// </summary>
        /// <param name="srData"></param>
        public bool Get(out SocketReceiveProtocal srData)
        {
            srData = new SocketReceiveProtocal();

            // if (m_bufferSize <= 0)
            // {
            //     if (m_dataSize == 0 && m_cp >= HeadSize())
            //     {
            //         byte[] tempData = new byte[SocketConst.HEAD_BODY_SIZE];
            //         Array.Copy(m_buffer, 0, tempData, 0, SocketConst.HEAD_BODY_SIZE);
            //         m_dataSize = BitConverter.ToInt32(tempData, 0);

            //         byte[] tempHead = new byte[SocketConst.HEAD_FLAG_SIZE];
            //         Array.Copy(m_buffer, SocketConst.HEAD_BODY_SIZE, tempHead, 0, SocketConst.HEAD_FLAG_SIZE);

            //         byte[] tempCommand = new byte[SocketConst.HEAD_COMMAND_SIZE];
            //         Array.Copy(m_buffer, SocketConst.HEAD_FLAG_SIZE, tempCommand, 0, SocketConst.HEAD_COMMAND_SIZE);
            //         m_protocal = BitConverter.ToInt32(tempCommand, 0);

            //         byte[] tempSession = new byte[SocketConst.HEAD_SESSION_SIZE];
            //         Array.Copy(m_buffer, SocketConst.HEAD_COMMAND_SIZE, tempSession, 0, SocketConst.HEAD_SESSION_SIZE);
            //         m_session = BitConverter.ToInt32(tempSession, 0);

            //         m_bufferSize = m_dataSize - HeadSize();
            //     }
            // }

            // if (m_bufferSize > 0 && m_cp >= HeadSize())
            // {
            //     srData.protocal = m_protocal;
            //     srData.session = m_session;
            //     srData.bufferSize = m_bufferSize;
            //     srData.dataSize = m_dataSize;
            //     srData.data = new byte[m_dataSize];
            //     Array.Copy(m_buffer, HeadSize(), srData.data, 0, m_dataSize);

            //     m_cp -= m_bufferSize;
            //     byte[] temp = new byte[m_cp < m_minSize ? m_minSize : m_cp];
            //     Array.Copy(m_buffer, m_bufferSize, temp, 0, m_cp);
            //     m_buffer = temp;

            //     m_bufferSize = 0;
            //     m_dataSize = 0;
            //     m_protocal = 0;
            //     return true;
            // }
            return false;
        }

        /// <summary>
        /// 协议头总长度
        /// </summary>
        private int HeadSize()
        {
            return SocketConst.HEAD_BODY_SIZE + SocketConst.HEAD_FLAG_SIZE + SocketConst.HEAD_COMMAND_SIZE + SocketConst.HEAD_SESSION_SIZE;
        }
    }
}