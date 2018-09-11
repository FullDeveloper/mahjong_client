using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManagerScript
{


    private static TipsManagerScript _instance;
    public Transform parent;

    private static string TIPS_PREFAB = "Prefab/TipPanel";

    public static TipsManagerScript getInstance()
    {
        if (_instance == null)
        {
            _instance = new TipsManagerScript();
        }
        return _instance;
    }

    public void setTips(string tips)
    {
        GameObject temp = Object.Instantiate(Resources.Load(TIPS_PREFAB) as GameObject);
        temp.transform.parent = parent;
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = new Vector3(0, -300);
        temp.GetComponent<TipPanelScript>().setText(tips);
        temp.GetComponent<TipPanelScript>().startAction();

    }
}
