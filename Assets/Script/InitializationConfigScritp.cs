﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationConfigScritp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TipsManagerScript.getInstance().parent = gameObject.transform;

        ServiceErrorListener seriveError = new ServiceErrorListener();


    }

    // Update is called once per frame
    void Update () {
		
	}
}
