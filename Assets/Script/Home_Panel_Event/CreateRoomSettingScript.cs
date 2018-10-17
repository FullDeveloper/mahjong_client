using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomSettingScript : MonoBehaviour {


    // 长沙麻将设置界面
    public GameObject panelChangshaSetting;

    // 长沙麻将房卡数
    public List<Toggle> changshaRoomCards;

    // 长沙麻将玩法
    public List<Toggle> changshaGameRule;

    // 长沙麻将抓码个数
    public List<Toggle> changshaZhuama;

    // 房卡数
    private int roomCardCount;

    private RoomCreateVo sendVo;//创建房间的信息

    // Use this for initialization
    void Start () {
        panelChangshaSetting.SetActive(true);

        // 绑定服务端返回创建房间响应委托函数
        SocketEventHandle.getInstance().CreateRoomCallBack += onCreateRoomCallback;
    }

    /**
	 * 创建长沙麻将房间
	 */
    public void createChangshaRoom()
    {
        int roundNumber = 4;//房卡数量
        bool isZimo = false;//自摸
        int maCount = 0;
        for (int i = 0; i < changshaRoomCards.Count; i++)
        {
            Toggle item = changshaRoomCards[i];
            if (item.isOn)
            {
                if (i == 0)
                {
                    roundNumber = 8;
                }
                else if (i == 1)
                {
                    roundNumber = 16;
                }
                break;
            }
        }
		if (changshaGameRule [0].isOn) {
			isZimo = true;
		}
        for (int i = 0; i < changshaZhuama.Count; i++)
        {
            if (changshaZhuama[i].isOn)
            {
                // pow(x,y) 返回值 x 的 y 次幂。
                maCount = (int)Math.Pow(2, i);
                break;
            }
        }
        sendVo = new RoomCreateVo();
        // 游戏倍数
        sendVo.magnification = maCount;
        // 游戏局数
        sendVo.roundNumber = roundNumber;
        // 设置游戏类型
        sendVo.roomType = GameConfig.GAME_TYPE_CHANGSHA;
        // 设置是否只允许自摸才能胡牌
        sendVo.ziMo = isZimo?1:0;
        string sendMessage = JsonMapper.ToJson(sendVo);
        Debug.Log(sendMessage);
        if (GlobalDataScript.loginResponseData.account.roomCard > 0)
        {
            CustomSocket.getInstance().sendMsg(new CreateRoomRequest(sendMessage));
        }
        else
        {
            TipsManagerScript.getInstance().setTips("你的房卡数量不足，不能创建房间");
        }

    }

    public void closeDialog()
    {
        Debug.Log("closeDialog");
        SocketEventHandle.getInstance().CreateRoomCallBack -= onCreateRoomCallback;
        Destroy(this);
        Destroy(gameObject);
    }

    private void onCreateRoomCallback(ClientResponse response)
    {
        Debug.Log("创建房间返回数据=> " +response.message);
        if (response.status == 1)
        {
            int roomId = Int32.Parse(response.message);
            sendVo.roomId = roomId;
            // 全局数据设置房间信息
            GlobalDataScript.roomVo = sendVo;
            // 设置房间ID
            GlobalDataScript.loginResponseData.roomId = roomId;
            // 设置房主为true
            GlobalDataScript.loginResponseData.main = true;
            // 设置在线状态
            GlobalDataScript.loginResponseData.onLine = true;
            // 设置游戏界面
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_Game_Play");

            GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().createRoomAddAvatarVO(GlobalDataScript.loginResponseData);

            closeDialog();
        }
        else
        {
            TipsManagerScript.getInstance().setTips(response.message);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
