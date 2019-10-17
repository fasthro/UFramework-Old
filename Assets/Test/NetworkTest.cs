using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FastEngine.Core;

public class NetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    SocketClient client;
    int i = 0;
    void Start()
    {
        client = new SocketClient();
        client.connectedEventCallback += OnConnectedHandler;
        client.sendEventCallback += OnSendHandler;
        // client.Connect("127.0.0.1", 8080);
        // client.Connect("192.168.1.48", 8080);
        client.Connect("192.168.1.41", 8080);

    }

    void OnApplicationQuit()
    {
        client.Close();
    }

    void OnConnectedHandler()
    {
        Send();
    }

    void Send()
    {

        // 协议总厂
        // byte[] size = new byte[4];
        // Array.Copy(BitConverter.GetBytes(132), 0, size, 0, sizeof(int));
        // buff.WriteBytes(size, false);

        // 特殊标志
        // byte[] flag = new byte[4];
        // int nIndex = 0;
        // Array.Copy(BitConverter.GetBytes('R'), 0, flag, nIndex, sizeof(byte));
        // nIndex += sizeof(byte);
        // Array.Copy(BitConverter.GetBytes('H'), 0, flag, nIndex, sizeof(byte));
        // nIndex += sizeof(byte);
        // Array.Copy(BitConverter.GetBytes('E'), 0, flag, nIndex, sizeof(byte));
        // nIndex += sizeof(byte);
        // Array.Copy(BitConverter.GetBytes('A'), 0, flag, nIndex, sizeof(byte));

        // buff.WriteBytes(flag, false);

        // 协议号
        // byte[] protocal = new byte[4];
        // Array.Copy(BitConverter.GetBytes(1001), 0, protocal, 0, sizeof(int));
        // buff.WriteBytes(protocal, false);

        // session
        // byte[] session = new byte[4];
        // Array.Copy(BitConverter.GetBytes(1), 0, session, 0, sizeof(int));
        // buff.WriteBytes(session, false);

        // buff.WriteString("cc");
        // buff.WriteString("android");
        // buff.WriteString("google-play");
        // buff.WriteString("en");
        // buff.WriteString("dw-android-googleplay-en-01");
        // buff.WriteString("59d8b89d754d6bc773c68ebe41f40416");
        // buff.WriteString("US");
        // buff.WriteByte(1);
        // buff.WriteString("");
        // buff.WriteInt(4);

        CNetMessage msg = new CNetMessage();
        msg.MessageID = 1001;
        msg.SessionID = 1;
        msg.AddString("cc");
        msg.AddString("android");
        msg.AddString("google-play");
        msg.AddString("en");
        msg.AddString("dw-android-googleplay-en-01");
        msg.AddString("59d8b89d754d6bc773c68ebe41f40416");
        msg.AddString("US");
        msg.AddByte(1);
        msg.AddString("");
        msg.AddInt(4);
 
        // Debug.Log("msg: " + msg.Bytes.Length);
        // Debug.Log("buff: " + buff.ToArray().Length);
        
        client.Send(1001, msg.Bytes);
    }

    private void OnSendHandler()
    {

    }

    // Update is called once per frame
    float a = 5;
    void Update()
    {
        a -= Time.deltaTime;
        if (a <= 0)
        {
            a = 5;
            // Send();
        }
    }
}
