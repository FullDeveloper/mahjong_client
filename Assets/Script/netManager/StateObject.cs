using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class StateObject {

    public int totalBytesRead = 0;
    public const int BufferSize = 1024 * 1024 * 2;
    public string readType = null;
    public byte[] buffer = new byte[BufferSize];

    public TcpClient client = null;

}
