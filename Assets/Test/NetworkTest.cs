using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FastEngine.Core;
using FastEngine.Protos;

public class NetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    SocketClient client;
    int i = 0;
    void Start()
    {
        client = new SocketClient("192.168.1.41", 8080, (args) =>
        {
            switch (args.socketState)
            {
                case SocketState.Connected:
                    break;
            }
        });
        client.Connect();
    }

    void OnApplicationQuit()
    {
        client.Disconnecte();
    }

    void Send()
    {
        for (int i = 0; i < 1; i++)
        {
            // stream
            // var pack = SocketPackFactory.CreateWriter(1000 + i);
            // for (int k = 0; k < 100; k++)
            // {
            //     pack.WriteInt(i);
            // }
            // client.Send(pack);

            // protobuf
            var pack = SocketPackFactory.CreateWriter<C2S_Login>(1000 + i);
            var message = pack.GetMessage<C2S_Login>();
            message.Account = 99;
            message.Password = 88;
            client.Send(pack);
        }
    }


    // Update is called once per frame
    bool isSend = false;

    float t = 5;
    void Update()
    {
        t -= Time.deltaTime;

        if (t <= 0 && !isSend && client.isConnected)
        {
            isSend = true;
           Send();
        }

        client.Update();
    }
}
