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
            TCPSession.Send(cmd);
            cmd++;
        }
    }
}
