using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;

public class MyMahjongScript : MonoBehaviour
{

    public List<AvatarVO> avatarList;

    // 玩家信息
    public List<PlayerItemScript> playerItems;

    // 房间信息
    public Text roomRemark;

    // Use this for initialization
    void Start()
    {
        addListener();
        GlobalDataScript.isonLoginPage = false;
    }

    public void addListener()
    {
        SocketEventHandle.getInstance().gameReadyNotice += gameReadyNotice;
        SocketEventHandle.getInstance().otherUserJointRoomCallBack += otherUserJointRoom;
    }

    public void otherUserJointRoom(ClientResponse response)
    {
        Debug.Log("其他人加入房间=>" + response.message);
        AvatarVO avatar = JsonMapper.ToObject<AvatarVO>(response.message);
        addAvatarVOToList(avatar);
    }

    public void joinToRoom(List<AvatarVO> avatars)
    {
        avatarList = avatars;
        for (int i = 0; i < avatars.Count; i++)
        {
            setSeat(avatars[i]);
        }
        setRoomRemark();
        readyGame();
        markselfReadyGame();
    }

    public void gameReadyNotice(ClientResponse response)
    {

        //===============================================
        JsonData json = JsonMapper.ToObject(response.message);

        Debug.Log("其他用户进入房间==>" + response.message);

        int avatarIndex = Int32.Parse(json["avatarIndex"].ToString());
        Debug.Log("索引为==>" + avatarIndex);
        int myIndex = getMyIndexFromList();
        int seatIndex = avatarIndex - myIndex;
        if (seatIndex < 0)
        {
            seatIndex = 4 + seatIndex;
        }
        playerItems[seatIndex].readyImg.enabled = true;
        avatarList[avatarIndex].ready = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void exitOrDissoliveRoom()
    {
         
    }

    public void createRoomAddAvatarVO(AvatarVO avatar)
    {
        // 设置分数默认为1000
        avatar.scores = 1000;
        addAvatarVOToList(avatar);
        setRoomRemark();
        // 执行准备游戏操作
        readyGame();
        // 显示自己已经准备游戏
        markselfReadyGame();
    }

    private void markselfReadyGame()
    {
        playerItems[0].readyImg.transform.gameObject.SetActive(true);
    }

    private void readyGame()
    {
        CustomSocket.getInstance().sendMsg(new GameReadyRequest());
    }

    public void setRoomRemark()
    {
        RoomCreateVo roomvo = GlobalDataScript.roomVo;
        GlobalDataScript.totalTimes = roomvo.roundNumber;
        GlobalDataScript.surplusTimes = roomvo.roundNumber;
        //	LeavedRoundNumText.text = GlobalDataScript.surplusTimes + "";
        string str = "房间号：\n" + roomvo.roomId + "\n";
        str += "圈数：" + roomvo.roundNumber + "\n";

        if (roomvo.roomType == 3)
        {
            str += "长沙麻将\n";
        }
        else
        {
            // TODO
        }
        if (roomvo.magnification > 0)
        {
            str += "倍率：" + roomvo.magnification + "";
        }
        roomRemark.text = str;
    }

    private void addAvatarVOToList(AvatarVO avatar)
    {
        if (avatarList == null)
        {
            avatarList = new List<AvatarVO>();
        }
        avatarList.Add(avatar);
        setSeat(avatar);

    }

    /// <summary>
	/// 设置当前角色的座位
	/// </summary>
	/// <param name="avatar">Avatar.</param>
	private void setSeat(AvatarVO avatar)
    {
        //游戏结束后用的数据，勿删！！！

        //GlobalDataScript.palyerBaseInfo.Add (avatar.account.uuid, avatar.account);
        if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            playerItems[0].setAvatarVo(avatar);
        }
        else
        {
            int myIndex = getMyIndexFromList();
            int curAvaIndex = avatarList.IndexOf(avatar);
            Debug.Log("curAvaIndex=>" + curAvaIndex);
            int seatIndex = curAvaIndex - myIndex;
            if (seatIndex < 0)
            {
                seatIndex = 4 + seatIndex;
            }
            Debug.Log("seatIndex=>" + seatIndex);
            playerItems[seatIndex].setAvatarVo(avatar);
        }

    }

    /// <summary>
	/// Gets my index from list.
	/// </summary>
	/// <returns>The my index from list.</returns>
	private int getMyIndexFromList()
    {
        if (avatarList != null)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid || avatarList[i].account.openId == GlobalDataScript.loginResponseData.account.openId)

                {
                    GlobalDataScript.loginResponseData.account.uuid = avatarList[i].account.uuid;
                    Debug.Log("数据正常返回" + i);
                    return i;
                }

            }
        }

        Debug.Log("数据异常返回0");
        return 0;
    }

}
