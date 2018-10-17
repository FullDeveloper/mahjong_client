using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HomePanelScript : MonoBehaviour
{

    public Image headIconImg;//头像路径

    public Text noticeText;

    public Text nickNameText;//昵称
    public Text cardCountText;//房卡剩余数量
    public Text IpText;
    private string headIcon;

    private GameObject panelCreateDialog;//界面上打开的dialog


    // Use this for initialization
    void Start()
    {
        //初始化用户数据
        initUI();
        // 标记当前已经不是登录面板
        GlobalDataScript.isonLoginPage = false;
    }

    private void initUI()
    {
        if (GlobalDataScript.loginResponseData != null)
        {
            headIcon = GlobalDataScript.loginResponseData.account.headIcon;
            string nickName = GlobalDataScript.loginResponseData.account.nickName;
            int roomCardcount = GlobalDataScript.loginResponseData.account.roomCard;
            cardCountText.text = roomCardcount + "";
            nickNameText.text = nickName;
            IpText.text = "ID:" + GlobalDataScript.loginResponseData.account.uuid;
            GlobalDataScript.loginResponseData.account.roomCard = roomCardcount;
        }
    }

    /***
	 * 打开创建房间的对话框
	 * 
	 */
    public void openCreateRoomDialog()
    {
        if (GlobalDataScript.loginResponseData == null || GlobalDataScript.loginResponseData.roomId == 0)
        {
            loadPerfab("Prefab/Panel_Create_Dialog");
        }
        else
        {
            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法创建房间");
        }
    }

    /***
	 * 打开进入房间的对话框
	 * 
	 */
    public void openEnterRoomDialog()
    {

        if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0)
        {
            loadPerfab("Prefab/Panel_Enter_Room");
        }
        else
        {
            TipsManagerScript.getInstance().setTips("当前正在房间状态，无法加入新的房间");
        }
    }

    private void loadPerfab(string perfabName)
    {
        panelCreateDialog = Instantiate(Resources.Load(perfabName)) as GameObject;
        panelCreateDialog.transform.parent = gameObject.transform;
        panelCreateDialog.transform.localScale = Vector3.one;
        //panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void removeListener()
    {
      
    }

}
