using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoomScript : MonoBehaviour {

    // 房间号
    public List<Text> inputTexts;

    // 确认按钮
    public Button buttonSure;

    // 删除按钮
    public Button buttonDelete;

    // 数字按钮
    public List<GameObject> btnList;
    // 输入的字符
    private List<String> inputChars;


    // Use this for initialization
    void Start () {

        SocketEventHandle.getInstance().JoinRoomCallBack += onJoinRoomCallBack;
        inputChars = new List<String>();
        for (int i = 0; i < btnList.Count; i++)
        {
            GameObject gobj = btnList[i];
            btnList[i].GetComponent<Button>().onClick.AddListener(delegate () {
                this.OnClickHandle(gobj);
            });
        }

    }

    // 删除数字
    public void deleteNumber()
    {
        if (inputChars != null && inputChars.Count > 0)
        {
            inputChars.RemoveAt(inputChars.Count - 1);
            inputTexts[inputChars.Count].text = "";
        }
    }

    // 确认加入房间
    public void sureRoomNumber()
    {
        if (inputChars.Count != 6)
        {
            Debug.Log("请先完整输入房间号码！");
            TipsManagerScript.getInstance().setTips("请先完整输入房间号码！");
            return;
        }

        String roomNumber = inputChars[0] + inputChars[1] + inputChars[2] + inputChars[3] + inputChars[4] + inputChars[5];
        Debug.Log(roomNumber);
        RoomJoinVo roomJoinVo = new RoomJoinVo();
        roomJoinVo.roomId = int.Parse(roomNumber);
        string sendMsg = JsonMapper.ToJson(roomJoinVo);
        CustomSocket.getInstance().sendMsg(new JoinRoomRequest(sendMsg));

    }

    public void OnClickHandle(GameObject gobj)
    {
        //if(eventData.button)
        Debug.Log(gobj);
        // 传递该button组件上的子组件Text的值
        clickNumber(gobj.GetComponentInChildren<Text>().text);
    }

    private void clickNumber(string number)
    {
        Debug.Log(number.ToString());
        // 如果输入的字符长度大于等于6 不可继续输入
        if (inputChars.Count >= 6)
        {
            return;
        }
        // 加到字符中
        inputChars.Add(number);
        // 获取数组的长度
        int index = inputChars.Count;
        // 下表赋值 数组下表0开始
        inputTexts[index - 1].text = number.ToString();
    }


    public void closeDialog()
    {
        Debug.Log("closeDialog");
        //GlobalDataScript.homePanel.SetActive (false);
        removeListener();
        Destroy(this);
        Destroy(gameObject);
    }

    private void removeListener()
    {
        SocketEventHandle.getInstance().JoinRoomCallBack -= onJoinRoomCallBack;
    }

    private void onJoinRoomCallBack(ClientResponse response)
    {
        Debug.Log(response);

        if (response.status == 1)
        {
            GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomJoinResponseVo>(response.message);
            GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
            GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
            GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
            GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
            GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
            GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
            GlobalDataScript.roomVo.magnification = GlobalDataScript.roomJoinResponseData.magnification;
            GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            //loadPerfab("Prefab/Panel_GamePlay");
            GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab("Prefab/Panel_Game_Play");
            GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript>().joinToRoom(GlobalDataScript.roomJoinResponseData.playerList);
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
