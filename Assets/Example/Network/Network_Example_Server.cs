/*
 * @Author: fasthro
 * @Date: 2019-09-17 17:25:17
 * @Description: Network Example Server
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace FastEngine.Example
{
    public class Network_Example_Server : MonoBehaviour
    {
        private static byte[] result = new byte[1024];
        private static Socket socket;
        private Thread m_thread;

        void Start()
        {
            //服务器IP地址  
            IPAddress ip = IPAddress.Parse(GameConst.IP);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(ip, GameConst.Port));  //绑定IP地址：端口  
            socket.Listen(10);    //设定最多10个排队连接请求  
                                  //通过Clientsoket发送数据  
            m_thread = new Thread(ListenClientConnect);
            m_thread.Start();
            Console.ReadLine();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = socket.Accept();
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
                Debug.Log(clientSocket.RemoteEndPoint.ToString());
            }
        }

        /// <summary>  
        /// 接收消息
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber > 0)
                    {
                        myClientSocket.Send(result, receiveNumber, 0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}

