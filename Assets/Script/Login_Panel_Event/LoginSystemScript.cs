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
    
    public Toggle agreeProtocol;

    public Text versionText;

    public GameObject watingPanel;

    // Use this for initialization
    void Start()
    {
        versionText.text = "版本号：" + Application.version;
    }

    public void login() {
        Debug.Log("set button onclick event.");

        if(agreeProtocol.isOn){
            
        }else{
            Debug.Log("请先同意协议！");
            TipsManagerScript.getInstance().setTips("请先同意用户协议");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
