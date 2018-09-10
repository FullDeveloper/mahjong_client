using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CustomSocket
{

    TcpClient tcpClient = new TcpClient();

    NetworkStream networkStream;

    public byte[] databuffer = new byte[StateObject.BufferdSize];

    public int offset = 0;

    public int end = 0;

    byte[] sources;
    byte[] headBytes;

    private int disConnectCount = 0;

    public static bool hasStartTimer;

    public bool isConnected = false;

    private static CustomSocket _instance;

    public static CustomSocket getInstance()
    {
        if (_instance == null)
        {
            _instance = new CustomSocket();
        }
        return _instance;
    }

    /// <summary>
    /// Connect romote server.
    /// </summary>
    public void Connect()
    {
        try
        {
            tcpClient = new TcpClient();
            // 防止延时，即使发送。
            tcpClient.NoDelay = true;
        }
        catch (Exception e)
        {
            
        }

    }
}

