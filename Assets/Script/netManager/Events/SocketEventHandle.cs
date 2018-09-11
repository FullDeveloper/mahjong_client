using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SocketEventHandle : MonoBehaviour
{

    private static SocketEventHandle _instance;

    public delegate void ServerCallBackEvent(ClientResponse response);


    public ServerCallBackEvent LoginCallBack;//登录回调


    private bool isDisconnet = false;

    private List<ClientResponse> callBackResponseList;
        
    public SocketEventHandle()
    {
        callBackResponseList = new List<ClientResponse>();
    }

    // 讲事件脚本挂在到一个游戏对象上 游戏运行每一帧都会判断是否有服务器响应信息
    public static SocketEventHandle getInstance()
    {
        if (_instance == null)
        {
            GameObject temp = new GameObject();
            _instance = temp.AddComponent<SocketEventHandle>();
        }
        return _instance;
    }

    void FixedUpdate()
    {
        while (callBackResponseList.Count > 0)
        {
            ClientResponse response = callBackResponseList[0];
            callBackResponseList.RemoveAt(0);
            dispatchHandle(response);
        }

        if (isDisconnet)
        {
            isDisconnet = false;
            //disConnetNotice();
        }

    }

    private void dispatchHandle(ClientResponse response)
    {
        switch (response.headCode)
        {
            // 登录成功服务器响应
            case APIS.LOGIN_RESPONSE:
                if (LoginCallBack != null)
                {
                    LoginCallBack(response);
                }
                break;
        }
    }

    public void addResponse(ClientResponse response)
    {
        callBackResponseList.Add(response);
    }

}