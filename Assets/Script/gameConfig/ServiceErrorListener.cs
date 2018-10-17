using UnityEngine;
using UnityEditor;

public class ServiceErrorListener : MonoBehaviour
{
    public ServiceErrorListener()
    {
        SocketEventHandle.getInstance().serviceErrorNotice += serviceErrorNotice;
    }

    public void serviceErrorNotice(ClientResponse response)
    {
        TipsManagerScript.getInstance().setTips(response.message);
    }
}