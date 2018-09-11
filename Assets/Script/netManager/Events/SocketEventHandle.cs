using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SocketEventHandle : MonoBehaviour
{

    private static SocketEventHandle _instance;

    private List<ClientResponse> callBackResponseList;

    public SocketEventHandle()
    {
        callBackResponseList = new List<ClientResponse>();
    }

    public static SocketEventHandle getInstance()
    {
        if (_instance == null)
        {
            GameObject temp = new GameObject();
            _instance = temp.AddComponent<SocketEventHandle>();
        }
        return _instance;
    }

    public void addResponse(ClientResponse response)
    {
        callBackResponseList.Add(response);
    }

}