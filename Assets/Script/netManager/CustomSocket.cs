using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class CustomSocket
{

    TcpClient tcpClient = new TcpClient();

    NetworkStream networkStream;

    public byte[] databuffer = new byte[StateObject.BufferSize];

    public int offset = 0;

    public int end = 0;

    byte[] sources;
    byte[] headBytes;

    bool isWait = false;
    int waitLen = 0;

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
            Debug.Log("开始连接服务端程序 =>" + APIS.SOCKET_URL +":"+ APIS.SOCKET_PORT);
            tcpClient = new TcpClient();
            // 防止延时，即使发送。
            tcpClient.NoDelay = true;
            // 开始连接远程客户端
            tcpClient.BeginConnect(APIS.SOCKET_URL, APIS.SOCKET_PORT, new AsyncCallback(ConnectCallback),tcpClient);
        }
        catch (Exception e)
        {
            // 设置连接标记为false
            showMessageTip("服务器断开连接,请重新运行程序或稍后重试");
            Debug.Log("connect remote server error.");
            Debug.Log(e.ToString());
            isConnected = false;
        }

    }

    /// <summary>
    /// 关闭服务器连接
    /// </summary>
    public void disConnect() {
        if (tcpClient != null)
        {
            tcpClient.Close();
            tcpClient = null;
        }
        if (networkStream != null)
        {
            networkStream.Close();
            networkStream = null;
        }
    }

    private void showMessageTip(string message)
    {
        ClientResponse temp = new ClientResponse();
        temp.headCode = APIS.TIP_MESSAGE;
        temp.message = message;
        SocketEventHandle.getInstance().addResponse(temp);
    }

    /// <summary>
    /// 异步回调的函数
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallback(IAsyncResult ar) {
        //connectDone.Set();
        if ((tcpClient != null) && (tcpClient.Connected))
        {
            networkStream = tcpClient.GetStream();
            asyncread(tcpClient);
            isConnected = true;
            Debug.Log("服务器已经连接!");
        }
        TcpClient t = (TcpClient)ar.AsyncState;
        try
        {
            t.EndConnect(ar);
        }
        catch (Exception ex)
        {
            //设置标志,连接服务端失败!
            Debug.Log(ex.ToString());
            //	tcpclient.BeginConnect(APIS.socketUrl, 1101, new AsyncCallback(ConnectCallback), tcpclient);


        }
    }

    /// <summary>
    /// 异步读取TCP数据
    /// </summary>
    /// <param name="tcpClient"></param>
    private void asyncread(TcpClient socket)
    {
        Debug.Log("建立连接 开始异步读取数据");
        StateObject state = new StateObject();
        state.client = socket;
        NetworkStream stream;
        try
        {
            stream = socket.GetStream();
            if (stream.CanRead)
            {
                Debug.Log("接收到消息,进行读取");
                try
                {
                    // 开始异步读从 NetworkStream
                    // 用来接收从networkStream中读取到的数据
                    // 从networkStream中什么位置开始读起
                    // 要从NetworkStream中读取的字节长度
                    IAsyncResult ar = stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
                            new AsyncCallback(TCPReadCallBack), state);

                }
                catch (Exception ex)
                {
                    //设置标志,连接服务端失败!

                    Debug.Log(ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            //设置标志,连接服务端失败!
            // NetManaged.isConnectServer = false;
            // NetManaged.surcessstate = 0;
            Debug.Log(ex.ToString());
        }
    }
    /// <summary>
    /// TCP读数据的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void TCPReadCallBack(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        //主动断开时
        if ((state.client == null) || (!state.client.Connected))
        {
            closeSocket();
            return;
        }
        int numberOfBytesRead;
        // 从socket中获取网络流对象
        NetworkStream mas = state.client.GetStream();
        // 从流中读取的字节数，介于零 (0) 和所请求的字节数之间。 流仅在流结尾返回零 (0)，否则在至少有 1 个字节可用之前应一直进行阻止。
        // 读取到的所有字节长度
        numberOfBytesRead = mas.EndRead(ar);
        Debug.Log("当前已读字节数组长度-->" + numberOfBytesRead);
        state.totalBytesRead += numberOfBytesRead;
        if (numberOfBytesRead > 0)
        {
            // 定义一个字节数组 长度为从网络流中读取到的字节长度
            byte[] dd = new byte[numberOfBytesRead];
            // 将buffer中的数据 从0个字节开始 拷贝到 dd字节数组 从0开始 到numberOfBytesRead长度
            Array.Copy(state.buffer, 0, dd, 0, numberOfBytesRead);
            // 是否需要等待
            if (isWait)
            {
                byte[] temp = new byte[sources.Length + dd.Length];
                sources.CopyTo(temp, 0);
                dd.CopyTo(temp, sources.Length);
                sources = temp;
                if (sources.Length >= waitLen)
                {
                    ReceiveCallBack(sources.Clone() as byte[]);
                    isWait = false;
                    waitLen = 0;
                }
            }
            else
            {
                sources = null;
                ReceiveCallBack(dd);
            }
            mas.BeginRead(state.buffer, 0, StateObject.BufferSize,
                    new AsyncCallback(TCPReadCallBack), state);
        }
        else
        {
            //被动断开时 
            mas.Close();
            state.client.Close();
            mas = null;
            state = null;
            //设置标志,连接服务端失败!
            Debug.Log("客户端被动断开");
        }
    }

    private void ReceiveCallBack(byte[] m_receiveBuffer)
    {
        //通知调用端接收完毕
        try
        {
            //	MyDebug.Log("m_receiveBuffer======"+m_receiveBuffer.Length);
            Debug.Log("读取数据成功，字节数组长度为->" + m_receiveBuffer.Length);
            // 定义一个内存流
            MemoryStream ms = new MemoryStream(m_receiveBuffer);
            // 使用二进制读取对象去读取
            BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
            readBuffer(buffers);
        }
        catch (Exception ex)
        {
            Debug.Log("socket exception:" + ex.Message);
            throw new Exception(ex.Message);
        }
    }



    private void readBuffer(BinaryReader buffers)
    {
        // 协议标记
        byte flag = buffers.ReadByte();
        // 数据总长度
        int lens = ReadInt(buffers.ReadBytes(4));
        disConnectCount = 0;
        if (!hasStartTimer && lens == 16)
        {
            // startTimer(); TODO 超时判断
            hasStartTimer = true;
        }
        if (lens > buffers.BaseStream.Length)
        {
            waitLen = lens;
            isWait = true;
            buffers.BaseStream.Position = 0;
            byte[] dd = new byte[buffers.BaseStream.Length];
            byte[] temp = buffers.ReadBytes((int)buffers.BaseStream.Length);
            Array.Copy(temp, 0, dd, 0, (int)buffers.BaseStream.Length);
            if (sources == null)
            {
                sources = dd;
            }
            return;
        }
        // 相应的操作码
        int headcode = ReadInt(buffers.ReadBytes(4));
        // 服务器状态码 成功1 失败-1
        int status = ReadInt(buffers.ReadBytes(4));
        short messageLen = ReadShort(buffers.ReadBytes(2));

        Debug.Log("开始解析数据");
        Debug.Log("flag =>" + flag + "lens =>" + lens + "headcode =>" + headcode + "status =>" + status + "messageLen =>" + messageLen);

        if (flag == 1)
        {
            string message = Encoding.UTF8.GetString(buffers.ReadBytes(messageLen));
            ClientResponse response = new ClientResponse();
            response.status = status;
            response.message = message;
            response.headCode = headcode;
            Debug.Log("response.headCode = " + response.headCode + "  response.message =   " + message);
            SocketEventHandle.getInstance().addResponse(response);
        }
        if (buffers.BaseStream.Position < buffers.BaseStream.Length)
        {
            readBuffer(buffers);
        }
    }

    private void closeSocket()
    {
        disConnect();
    }


    /// <summary>
	/// 读取大端序的int
	/// </summary>
	/// <param name="value"></param>
	public int ReadInt(byte[] intbytes)
    {
        Array.Reverse(intbytes);
        return BitConverter.ToInt32(intbytes, 0);
    }

    public short ReadShort(byte[] intbytes)
    {
        Array.Reverse(intbytes);
        return BitConverter.ToInt16(intbytes, 0);
    }

}

