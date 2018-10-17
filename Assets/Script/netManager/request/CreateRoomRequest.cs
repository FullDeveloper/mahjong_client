using UnityEngine;
using UnityEditor;

public class CreateRoomRequest : ClientRequest
{
    public CreateRoomRequest(string sendMsg)
    {
        headCode = APIS.CREATEROOM_REQUEST;
        messageContent = sendMsg;
    }

}