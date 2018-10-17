using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerItemScript : MonoBehaviour
{

    // 当前座位玩家信息
    private AvatarVO avatarvo;
    // 是否准备
    public Image readyImg;
    // 是否为庄家
    public Image bankerImg;
    // 玩家的名称信息
    public Text nameText;
    // 玩家的成绩信息
    public Text scoreText;
    // 离线图片
    public Image offlineImage;
    // 玩家的头像
    public Image headerIcon;

    // Use this for initialization
    void Start()
    {

    }

    public void setAvatarVo(AvatarVO value)
    {
        if (value != null)
        {
            avatarvo = value;
            readyImg.enabled = avatarvo.ready;
            bankerImg.enabled = avatarvo.main;
            nameText.text = avatarvo.account.nickName;
            scoreText.text = avatarvo.scores + "";
            offlineImage.transform.gameObject.SetActive(!avatarvo.onLine);
            // StartCoroutine(LoadImg());

        }
        else
        {
            nameText.text = "";
            readyImg.enabled = false;
            bankerImg.enabled = false;
            scoreText.text = "";
            readyImg.enabled = false;            
            headerIcon.sprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
