using UnityEngine;
using UnityEditor;

public class APIS  
{
    // 服务端IP地址
    public const string SOCKET_URL = "192.168.8.122";
    // 服务端端口
    public const int SOCKET_PORT = 10122;
    public const int TIP_MESSAGE = 0x160016;

   
    public const int LOGIN_REQUEST = 0x000001; // 登陆请求码
    public const int LOGIN_RESPONSE = 0x000002;//登陆返回码

    public const int CREATEROOM_REQUEST = 0x00009;//创建房间请求码
    public const int CREATEROOM_RESPONSE = 0x00010;//创建房间返回吗

    public const int ERROR_RESPONSE = 0xffff09;//错误回调


    public const int PrepareGame_MSG_REQUEST = 0x333333;// 准备游戏请求码
    public const int PrepareGame_MSG_RESPONSE = 0x444444;// 准备游戏返回码

    public const int JOIN_ROOM_REQUEST = 0x000003;//加入房间请求码
    public const int JOIN_ROOM_NOICE = 0x10a004;//其它 人加入房间通知
    public const int JOIN_ROOM_RESPONSE = 0x000004;//加入房间返回码



}