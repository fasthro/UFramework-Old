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
    /// <summary>
    /// socket event callback delegate
    /// </summary>
    public delegate void SocketEventCallback(SocketEventArgs args);

    /// <summary>
    /// socket state
    /// </summary>
    public enum SocketState
    {
        Connected,
        Disconnected,
        Send,
        Received,
    }

    public enum SocketException
    {
        Timeout,
        Connect,
        Receive,
        Send,
        Exception,
    }

    /// <summary>
    /// socket event callback args
    /// </summary>
    public class SocketEventArgs
    {
        // socket state
        public SocketState socketState { get; private set; }

        // socket pack
        public SocketPack socketPack { get; private set; }

        // cmd
        public int cmd { get; private set; }

        // exception
        public SocketException exception { get; private set; }

        // error
        public string error { get; private set; }

        public SocketEventArgs(SocketState socketState)
        {
            this.socketState = socketState;
        }

        public SocketEventArgs(SocketState socketState, SocketPack socketPack) : this(socketState)
        {
            this.socketPack = socketPack;
        }

        public SocketEventArgs(SocketState socketState, int cmd) : this(socketState)
        {
            this.cmd = cmd;
        }

        public SocketEventArgs(SocketState socketState, SocketException exception, string error) : this(socketState)
        {
            this.exception = exception;
            this.error = error;
        }
    }

    public class SocketClient : LogUser
    {
        #region config
        /// <summary>
        /// 连接超时时间
        /// </summary>
        private readonly static int ConnectedTimeout = 5000;

        /// <summary>
        /// 接收数据池大小
        /// </summary>
        private readonly static int ReceiveCacheSize = 4096;
        #endregion

        // socket
        private Socket m_clientSocket;

        // 数据接收线程
        private Thread m_recThread;

        // 事件回调
        private event SocketEventCallback m_eventCallback;

        #region receive
        // 数据接收池
        private byte[] m_recCache;
        // 数据接收处理器
        private SocketReceiver m_receiver;
        // 接收的数据包
        private SocketPack m_recPack;
        #endregion

        #region send
        // 发送包头
        public SocketPackHeader m_sendPackHeader;
        #endregion

        public string ip { get; private set; }
        public int port { get; private set; }
        public bool isConnected { get; private set; }

        /// <summary>
        /// socket client
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public SocketClient(string ip, int port, SocketEventCallback callback)
        {
            this.ip = ip;
            this.port = port;
            this.m_eventCallback = callback;
            this.m_recCache = new byte[ReceiveCacheSize];
            this.m_receiver = new SocketReceiver();
            this.m_sendPackHeader = new SocketPackHeader();

            InitializeLogUser("SocketClient", true);
        }

        public void Connect()
        {
            try
            {
                string newIp = "";
                string connetIp = ip;
                // ipv6 & ipv4
                AddressFamily newAddressFamily = AddressFamily.InterNetwork;
                IPv6SupportMidleware.getIPType(ip, port.ToString(), out newIp, out newAddressFamily);
                if (!string.IsNullOrEmpty(newIp)) { connetIp = newIp; }

                Log("socket connect to server. " + ip + ":" + port + (string.IsNullOrEmpty(newIp) ? " ipv4" : " ipv6"));

                // 解析IP地址
                IPAddress ipAddress = IPAddress.Parse(connetIp);
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

                // 创建 Socket
                m_clientSocket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // 异步连接
                IAsyncResult result = m_clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(OnConnetSucceed), m_clientSocket);

                // 连接超时
                bool success = result.AsyncWaitHandle.WaitOne(ConnectedTimeout, true);
                if (!success)
                {
                    Exception(SocketException.Timeout, null);
                }
            }
            catch (System.Exception e)
            {
                LogError("socket connect to server exception. " + e.ToString());
                Exception(SocketException.Connect, e);
            }
        }

        public void ReConnect()
        {
            OnDisconnected();
            Connect();
        }

        private void OnConnetSucceed(IAsyncResult iar)
        {
            try
            {
                Log("socket connect to server succeed.");
                ((Socket)iar.AsyncState).EndConnect(iar);
                isConnected = true;
                m_recThread = new Thread(new ThreadStart(OnReceive));
                m_recThread.IsBackground = true;
                m_recThread.Start();

                BroadcastConnected();
            }
            catch (Exception e)
            {
                Exception(SocketException.Connect, e);
            }
        }

        public void Send(SocketPack pack)
        {
            if (!isConnected || m_clientSocket == null || !m_clientSocket.Connected)
            {
                ReConnect();
                return;
            }

            var data = m_sendPackHeader.Write(pack.cmd, pack.data);
            m_clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSend), m_clientSocket);
        }

        private void OnSend(IAsyncResult iar)
        {
            try
            {
                Log("socket send to server succeed.");
                ((Socket)iar.AsyncState).EndSend(iar);
                BroadcastSend(0);
            }
            catch (Exception e)
            {
                LogError("socket send to server exception." + e.ToString());
                Exception(SocketException.Send, e);
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
                    int bSize = m_clientSocket.Receive(m_recCache);
                    if (bSize > 0)
                    {
                        // 向接受者Push数据
                        m_receiver.Push(m_recCache, bSize);
                        // 尝试在向接受者获取Pack
                        while ((m_recPack = m_receiver.TryGetPack()) != null)
                        {
                            Log("socket receive data. cmd: " + m_recPack.cmd + " size: " + m_recPack.dataSize);
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
                        LogError("socket receive form server exception." + e.ToString());
                        Exception(SocketException.Receive, e);
                    }
                    break;
                }
            }
        }

        private void Exception(SocketException exception, Exception e = null)
        {
            OnDisconnected();
            BroadcastException(exception, e.ToString());
        }

        public void Disconnecte()
        {
            if (!isConnected) return;
            OnDisconnected();
            BroadcastDisconnected();
        }

        private void OnDisconnected()
        {
            Log("socket disconnected && closed.");

            isConnected = false;

            if (m_recThread != null)
                m_recThread.Abort();
            m_recThread = null;

            if (m_clientSocket != null && m_clientSocket.Connected)
            {
                m_clientSocket.Disconnect(false);
                m_clientSocket.Close();
            }
            m_clientSocket = null;
        }

        #region broadcast
        private void BroadcastConnected()
        {
            m_eventCallback.InvokeGracefully(new SocketEventArgs(SocketState.Connected));
        }

        private void BroadcastSend(int cmd)
        {
            m_eventCallback.InvokeGracefully(new SocketEventArgs(SocketState.Send, cmd));
        }

        private void BroadcastReceived(SocketPack pack)
        {
            m_eventCallback.InvokeGracefully(new SocketEventArgs(SocketState.Received, pack));
        }

        private void BroadcastDisconnected()
        {
            m_eventCallback.InvokeGracefully(new SocketEventArgs(SocketState.Disconnected));
        }

        private void BroadcastException(SocketException exception, string error)
        {
            m_eventCallback.InvokeGracefully(new SocketEventArgs(SocketState.Disconnected, exception, error));
        }
        #endregion
    }
}