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
        // socket
        private Socket m_clientSocket;
        // 数据接收线程
        private Thread m_recThread = null;

        #region 数据接收
        private byte[] m_recBytes = new byte[4096];
        // 接收数据缓冲区
        private SocketReceiver m_receiver = new SocketReceiver();
        // 接收的数据包
        private SocketPack m_recPack = null;
        #endregion

        public string ip { get; private set; }
        public int port { get; private set; }
        public bool isConnected { get; private set; }

        // 事件回调
        public event SocketConnectedCallback connectedEventCallback;
        public event SocketSendCallback sendEventCallback;
        public event SocketReceiveCallback receiveEventCallback;
        public event SocketCloseCallback closeEventCallback;

        public SocketClient() { }

        public void Connect(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            try
            {
                string newIp = "";
                string connetIp = ip;
                // ipv6 & ipv4
                AddressFamily newAddressFamily = AddressFamily.InterNetwork;
                IPv6SupportMidleware.getIPType(ip, port.ToString(), out newIp, out newAddressFamily);
                if (!string.IsNullOrEmpty(newIp)) { connetIp = newIp; }

                Log.LogInfo("socket connect to server. " + ip + ":" + port + (string.IsNullOrEmpty(newIp) ? " ipv4" : " ipv6"), LogModule.Network);

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
                Log.LogError("socket connect to server exception. " + ip + ":" + port, LogModule.Network);
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
                Log.LogInfo("socket connect to server succeed.", LogModule.Network);
                ((Socket)iar.AsyncState).EndConnect(iar);
                isConnected = true;
                m_recThread = new Thread(new ThreadStart(OnReceive));
                m_recThread.IsBackground = true;
                m_recThread.Start();

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
            if (!isConnected || m_clientSocket == null || !m_clientSocket.Connected)
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
                Log.LogInfo("socket send to server succeed.", LogModule.Network);
                ((Socket)iar.AsyncState).EndSend(iar);
                sendEventCallback.InvokeGracefully();
            }
            catch (Exception e)
            {
                Log.LogError("socket send to server exception." + e.ToString(), LogModule.Network);
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                if (!m_clientSocket.Connected)
                {
                    isConnected = false;
                    ReConnect();
                    break;
                }
                try
                {
                    // 接受数据
                    int bSize = m_clientSocket.Receive(m_recBytes);
                    if (bSize > 0)
                    {
                        // 向接受者Push数据
                        m_receiver.Push(m_recBytes, bSize);
                        // 尝试在向接受者获取Pack
                        while ((m_recPack = m_receiver.TryGetPack()) != null)
                        {
                            Log.LogInfo("socket receive data. cmd: " + m_recPack.cmd + " size: " + m_recPack.size, LogModule.Network);
                            // 添加到数据处理队列
                            // lock ()
                            // {
                            //    receiveEventCallback.InvokeGracefully();
                            // }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (isConnected)
                    {
                        Log.LogError("socket receive form server exception." + e.ToString(), LogModule.Network);
                        Close();
                    }
                    break;
                }
            }
        }

        public void Close()
        {
            if (!isConnected) return;
            OnClose();
        }

        private void OnClose()
        {
            Log.LogInfo("socket closed.", LogModule.Network);

            isConnected = false;

            if (m_recThread != null) m_recThread.Abort();
            m_recThread = null;

            if (m_clientSocket != null && m_clientSocket.Connected) m_clientSocket.Close();
            m_clientSocket = null;

            closeEventCallback.InvokeGracefully();
        }
    }
}