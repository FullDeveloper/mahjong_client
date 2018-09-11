using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *
 *  登录事件脚本。  需要将该脚本挂入组件之中，该脚本中的属性需要在组件之中通过拖动进行关联。
 *
*/
public class LoginSystemScript : MonoBehaviour
{

    private GameObject panelCreateDialog;

    public Toggle agreeProtocol;

    public Text versionText;

    public GameObject watingPanel;

    // Use this for initialization
    void Start()
    {
        CustomSocket.hasStartTimer = false;
        // 开始连接服务端
        CustomSocket.getInstance().Connect();
        versionText.text = "版本号：" + Application.version;
        GlobalDataScript.isonLoginPage = true;
        SocketEventHandle.getInstance().LoginCallBack += LoginCallBack;

    }

    public void LoginCallBack(ClientResponse response)
    {
        if (watingPanel != null)
        {
            watingPanel.SetActive(false);
        }

        // SoundCtrl.getInstance().playBGM();
        if (response.status == 1)
        {
            if (GlobalDataScript.homePanel != null)
            {
                // TODO
                GlobalDataScript.homePanel.GetComponent<HomePanelScript>().removeListener();
                Destroy(GlobalDataScript.homePanel);
            }
        }

        if (GlobalDataScript.gamePlayPanel != null)
        {
            // TODO
            GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().exitOrDissoliveRoom();
        }
        Debug.Log("收到服务端返回数据-->" + response.message);
        GlobalDataScript.loginResponseData = JsonMapper.ToObject<AvatarVO>(response.message);
        Debug.Log("对象接受-->" + JsonMapper.ToJson(GlobalDataScript.loginResponseData));

        // 加载游戏主界面 TODO


    }

    public void login() {
        Debug.Log("set button onclick event.");

        if(agreeProtocol.isOn){
            watingPanel.SetActive(true);
            doLogin();
        }
        else{
            Debug.Log("请先同意协议！");
            // TipsManagerScript.getInstance().setTips("请先同意用户协议");
        }

    }

    public void doLogin()
    {
        GlobalDataScript.getInstance().wechatOperate.login();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
