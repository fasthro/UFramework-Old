/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:33:05
 * @Description: socket client
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using FastEngine.Common;

namespace FastEngine.Core
{
    public delegate void SocketConnectedCallback();
    public delegate void SocketSendCallback();
    public delegate void SocketReceiveCallback();
    public delegate void SocketCloseCallback();

    public class SocketClient
    {
        private string m_ip;
        public string ip { get { return m_ip; } }

        private int m_port;
        public int port { get { return m_port; } }

        private bool m_isConnected;
        public bool isConnected { get { return m_isConnected; } }

        private Socket m_clientSocket;

        private byte[] m_receiveBuffer = new byte[4096];

        // 接收数据缓冲区
        private SocketReceiveBuffer m_receiveDataBuffer = new SocketReceiveBuffer();
        // 接收的协议数据
        private SocketReceiveProtocal m_receiveData = null;

        private Thread m_receiveThread = null;

        // 事件回调
        public event SocketConnectedCallback connectedEventCallback;
        public event SocketSendCallback sendEventCallback;
        public event SocketReceiveCallback receiveEventCallback;
        public event SocketCloseCallback closeEventCallback;

        public SocketClient()
        {

        }

        public void Connect(string ip, int port)
        {
            this.m_ip = ip;
            this.m_port = port;

            try
            {
                string newIp = "";
                string connetIp = ip;
                // ipv6 & ipv4
                AddressFamily newAddressFamily = AddressFamily.InterNetwork;
                IPv6SupportMidleware.getIPType(ip, port.ToString(), out newIp, out newAddressFamily);
                if (!string.IsNullOrEmpty(newIp)) { connetIp = newIp; }

                // 解析IP地址
                IPAddress ipAddress = IPAddress.Parse(connetIp);
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

                // 创建 Socket
                m_clientSocket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // 异步连接
                IAsyncResult result = m_clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(OnConnetSucceed), m_clientSocket);

                // 连接超时
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                if (!success)
                {
                    OnConnetTimeout();
                }
            }
            catch (System.Exception e)
            {
                OnConnetFailed();
            }
        }

        public void ReConnect()
        {
            OnClose();
            Connect(ip, port);
        }

        private void OnConnetSucceed(IAsyncResult iar)
        {
            try
            {
                ((Socket)iar.AsyncState).EndConnect(iar);

                m_isConnected = true;

                Debug.Log("Socket Client OnConnetSucceed!");

                m_receiveThread = new Thread(new ThreadStart(OnReceive));
                m_receiveThread.IsBackground = true;
                m_receiveThread.Start();

                connectedEventCallback.InvokeGracefully();

            }
            catch (Exception e)
            {
                Close();
            }
        }

        private void OnConnetFailed()
        {
            OnClose();
        }

        private void OnConnetTimeout()
        {
            OnClose();
        }

        public void Send(int protocal, byte[] data)
        {
            if (!m_isConnected || m_clientSocket == null || !m_clientSocket.Connected)
            {
                ReConnect();
                return;
            }

            m_clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSend), m_clientSocket);
        }

        private void OnSend(IAsyncResult iar)
        {
            try
            {
                ((Socket)iar.AsyncState).EndSend(iar);
                Debug.Log("Socket Client OnSend");
                sendEventCallback.InvokeGracefully();
            }
            catch (Exception e)
            {
                Debug.Log("Socket Client Send Exception: " + e.StackTrace);
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                if (!m_clientSocket.Connected)
                {
                    m_isConnected = false;
                    ReConnect();
                    break;
                }
                try
                {
                    // 接受数据
                    int receiveLength = m_clientSocket.Receive(m_receiveBuffer);
                    if (receiveLength > 0)
                    {
                        Debug.Log("Socket Client OnReceive " + receiveLength);
                        // 把接受的数据添加到接受缓冲区内
                        m_receiveDataBuffer.Push(m_receiveBuffer, receiveLength);
                        // 在缓冲区内获取到完成协议数据
                        while (m_receiveDataBuffer.Get(out m_receiveData))
                        {
                            // 添加到数据处理队列
                            // lock ()
                            // {
                            //    receiveEventCallback.InvokeGracefully();
                            // }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    m_clientSocket.Disconnect(true);
                    m_clientSocket.Shutdown(SocketShutdown.Both);
                    m_clientSocket.Close();

                    OnClose();
                    break;
                }
            }
        }

        public void Close()
        {
            if (!m_isConnected) return;
            OnClose();
        }

        private void OnClose()
        {
            m_isConnected = false;

            if (m_receiveThread != null) m_receiveThread.Abort();
            m_receiveThread = null;

            if (m_clientSocket != null && m_clientSocket.Connected) m_clientSocket.Close();
            m_clientSocket = null;

            Debug.Log("Socket Client OnClose!");

            closeEventCallback.InvokeGracefully();
        }
    }
}