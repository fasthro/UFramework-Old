using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FastEngine.Core;
using FastEngine.Protos;

public class NetworkTest : MonoBehaviour
{
    void Start()
    {
        TCPSessionService.AddListener(30000, (pack) =>
        {
            var msg = pack.GetMessage<S2C_Login>();
        });

        TCPSession.Connecte();
    }

    float t = 0.1f;
    int cmd = 1000;
    void Update()
    {
        t -= Time.deltaTime;

        if (t <= 0 && TCPSession.isConnected)
        {
            t = 0.1f;
            var pack = SocketPackFactory.CreateWriter<C2S_Login>(30000);
            var msg = pack.GetMessage<C2S_Login>();
            msg.Account = 123;
            msg.Password = 987;
            TCPSession.Send(pack);
            cmd++;
        }
    }
}
