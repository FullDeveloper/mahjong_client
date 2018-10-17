using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

public class GlobalDataScript 
{

    public static bool isonLoginPage;//是否在登陆页面
    public static GameObject homePanel;//主界面
    public static GameObject gamePlayPanel;//游戏界面



    /**总局数**/
    public static int totalTimes;

    /**麻将剩余局数**/
    public static int surplusTimes;

    public WechatOperateScript wechatOperate;

    public static LoginVo loginVo;//登录数据

    /**房间游戏规则信息**/
    public static RoomCreateVo roomVo = new RoomCreateVo();


    /**登陆返回数据**/
    public static AvatarVO loginResponseData;
    /**加入房间返回数据**/
    public static RoomJoinResponseVo roomJoinResponseData;

    /// <summary>
	/// 最顶层的容器
	/// </summary>
	public Transform canvsTransfrom;

    public static List<String> messageBoxContents = new List<string>();


    private static GlobalDataScript _instance;
    public static GlobalDataScript getInstance()
    {
        if (_instance == null)
        {
            _instance = new GlobalDataScript();
        }
        return _instance;
    }

    public GlobalDataScript()
    {
        init();
    }

    public void init()
    {
        canvsTransfrom = GameObject.Find("Container").transform;
        TipsManagerScript.getInstance().parent = GameObject.Find("Canvas").transform;
        wechatOperate = GameObject.Find("Canvas").GetComponent<WechatOperateScript>();
        initMessageBox();
    }

    void initMessageBox()
    {
        messageBoxContents.Add("不要吵了，专心玩游戏！");
        messageBoxContents.Add("不要走，决战到天亮");
        messageBoxContents.Add("大家好，很高兴见到各位");
        messageBoxContents.Add("和你合作真是太愉快了");
        messageBoxContents.Add("快点啊，等得你花儿都谢了!");
        messageBoxContents.Add("你的牌打得也太好了");
        messageBoxContents.Add("交个朋友吧");
        messageBoxContents.Add("下次再玩吧，我要走了");
    }


    public string getIpAddress()
    {
        string tempip = "";
        try
        {
            WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]");
            int count = end - start;
            tempip = all.Substring(start, count);
            sr.Close();
            s.Close();
        }
        catch
        {
        }
        return tempip;
    }

}