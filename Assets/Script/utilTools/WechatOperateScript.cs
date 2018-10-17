using LitJson;
using System;
using UnityEngine;

public class WechatOperateScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    /**
	 * 登录，提供给button使用
	 * 
	 */
    public void login()
    {
        // shareSdk.GetUserInfo(PlatformType.WeChat);

        getUserInforCallback(1);

    }

    /**
	 * 获取微信个人信息成功回调,登录
	 *
	 */
    public void getUserInforCallback(int reqID/*ResponseState state, PlatformType type, Hashtable data*/)
    {
        //TipsManagerScript.getInstance ().setTips ("获取个人信息成功");
            LoginVo loginvo = new LoginVo();
            try
            {

                loginvo.openId = "100100104";
                loginvo.nickName = "测试";
                loginvo.headIcon = "-";
                loginvo.unionId = "100100104";
                loginvo.province = "浙江省";
                loginvo.city = "杭州市";
                string sex = "1";
                loginvo.sex = int.Parse(sex);
                loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
                String msg = JsonMapper.ToJson(loginvo);

                CustomSocket.getInstance().sendMsg(new LoginRequest(msg));

                GlobalDataScript.loginVo = loginvo;
                GlobalDataScript.loginResponseData = new AvatarVO();
                GlobalDataScript.loginResponseData.account = new Account();
                GlobalDataScript.loginResponseData.account.city = loginvo.city;
                GlobalDataScript.loginResponseData.account.openId = loginvo.openId;
                Debug.Log(" loginvo.nickName:" + loginvo.nickName);
                GlobalDataScript.loginResponseData.account.nickName = loginvo.nickName;
                GlobalDataScript.loginResponseData.account.headIcon = loginvo.headIcon;
                GlobalDataScript.loginResponseData.account.unionId = loginvo.city;
                GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
                GlobalDataScript.loginResponseData.IP = loginvo.IP;
            }
            catch (Exception e)
            {
                Debug.Log("微信接口有变动！" + e.Message);
                TipsManagerScript.getInstance().setTips("请先打开你的微信客户端");
                return;
            }

    }


    // Update is called once per frame
    void Update () {
		
	}
}
