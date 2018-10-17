using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SocketEventHandle : MonoBehaviour
{

    private static SocketEventHandle _instance;

    public delegate void ServerCallBackEvent(ClientResponse response);


    public ServerCallBackEvent LoginCallBack;//登录回调

    public ServerCallBackEvent CreateRoomCallBack;//创建房间回调

    public ServerCallBackEvent StartGameNotice;//开始游戏通知

    public ServerCallBackEvent gameReadyNotice;//准备游戏通知返回

    public ServerCallBackEvent otherUserJointRoomCallBack; // 其他人加入房间

    public ServerCallBackEvent JoinRoomCallBack;//加入房间回调

    public ServerCallBackEvent serviceErrorNotice;//错误信息返回





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
            // 服务端错误提示
            case APIS.ERROR_RESPONSE:
                if (serviceErrorNotice != null)
                {
                    serviceErrorNotice(response);
                }
                break;
            // 登录成功服务器响应
            case APIS.LOGIN_RESPONSE:
                if (LoginCallBack != null)
                {
                    LoginCallBack(response);
                }
                break;
            // 准备游戏服务器响应
            case APIS.PrepareGame_MSG_RESPONSE:
                if (gameReadyNotice != null)
                {
                    gameReadyNotice(response);
                }
                break;
            // 创建房间响应
            case APIS.CREATEROOM_RESPONSE:
                if (CreateRoomCallBack != null)
                {
                    CreateRoomCallBack(response);
                }
                break;
            // 其他人加入房间
            case APIS.JOIN_ROOM_NOICE:
                if (otherUserJointRoomCallBack != null)
                {
                    otherUserJointRoomCallBack(response);
                }
                break;
            // 自己加入房间回调
            case APIS.JOIN_ROOM_RESPONSE:
                if (JoinRoomCallBack != null)
                {
                    JoinRoomCallBack(response);
                }
                break;

        }
    }

    public void addResponse(ClientResponse response)
    {
        callBackResponseList.Add(response);
    }

}