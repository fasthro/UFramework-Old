/*
 * @Author: fasthro
 * @Date: 2019-10-23 19:29:07
 * @Description: TCPSession Example
 */
using System.Collections;
using System.Collections.Generic;
using FastEngine.Core;
using FastEngine.Protos;
using UnityEngine;

namespace FastEngine.Example
{
    public class TCPSessionExample : MonoBehaviour
    {
        bool isSend = false;
        // Start is called before the first frame update
        void Start()
        {
            // ip & port 在 App.cs 里初始化设置

            // 连接服务器
            TCPSession.Connecte();

            // 注册协议服务
            TCPSessionService.AddListener(1001, (pack) =>
            {
                // -- proto --
                // var msg = pack.GetMessage<C2S_Login>();
                // Debug.Log(msg.Account);

                // -- 流 --
                // Debug.Log(pack.ReadInt());

                // 读完之后释放 pack
                // pack.Release();
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (!isSend && TCPSession.isConnected)
            {
                // -- 发送无参协议 -- 
                // TCPSession.Send(1001);

                // -- 发送 proto 协议 --
                // var pack = SocketPackFactory.CreateWriter<C2S_Login>(1001);

                // var msg = pack.GetMessage<C2S_Login>();
                // msg.Account = 1;
                // msg.Password = 2;
                // 发送
                // pack.Send();
                // 或者
                // TCPSession.Send(pack);

                // -- 流发送方式 -- 
                // var pack = SocketPackFactory.CreateWriter(1001);
                // pack.WriteInt(1);
                // pack.Send();
                // 或者
                // TCPSession.Send(pack);
            }
        }
    }
}