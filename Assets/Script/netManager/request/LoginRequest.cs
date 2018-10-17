using LitJson;
using System;

public class LoginRequest : ClientRequest
{
    public LoginRequest(string data)
    {
        headCode = APIS.LOGIN_REQUEST;
        if (data == null)
        {
            LoginVo loginvo = new LoginVo();
            Random random = new Random();
            string str = random.Next(100, 1000) + "for" + random.Next(2000, 5000);
            loginvo.openId = "128";
            loginvo.nickName = "127";
            loginvo.headIcon = "imgicon";
            loginvo.unionId = "128";
            loginvo.province = "21sfsd";
            loginvo.city = "afafsdf";
            loginvo.sex = 1;
            loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
            data = JsonMapper.ToJson(loginvo);

            GlobalDataScript.loginVo = loginvo;
            GlobalDataScript.loginResponseData = new AvatarVO();
            GlobalDataScript.loginResponseData.account = new Account();
            GlobalDataScript.loginResponseData.account.city = loginvo.city;
            GlobalDataScript.loginResponseData.account.openId = loginvo.openId;
            GlobalDataScript.loginResponseData.account.nickName = loginvo.nickName;
            GlobalDataScript.loginResponseData.account.headIcon = loginvo.headIcon;
            GlobalDataScript.loginResponseData.account.unionId = loginvo.city;
            GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
            GlobalDataScript.loginResponseData.IP = loginvo.IP;
        }
        messageContent = data;

    }

    /**用于重新登录使用**/


    //退出登录
    public LoginRequest()
    {
        //headCode = APIS.QUITE_LOGIN;
        if (GlobalDataScript.loginResponseData != null)
        {
            messageContent = GlobalDataScript.loginResponseData.account.uuid + "";
        }

    }
}