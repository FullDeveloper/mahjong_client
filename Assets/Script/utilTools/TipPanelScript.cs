using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanelScript : MonoBehaviour {

    public Text label;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startAction(){
         Invoke("TipsMoveCompleted", 4f);
    }

    public void TipsMoveCompleted(){
        Destroy(gameObject);
    }

    public void setText(string tips)
	{
        label.text = tips;
    }
}
