﻿using UnityEngine;
using System.Collections;

public class MainLoaderController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine(LoadingSequanceCorutine());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator LoadingSequanceCorutine()
    {
        bool finishedAutologin = false;
        ServerRequestsManager.Instance.Init(() =>
        {
            AccountManager.Instance.Autologin(() =>
            {
                finishedAutologin = true;
            }, () =>
            {
                finishedAutologin = true;
                Debug.Log("ERROR ");
            });

        }, ()=>
        {
            Debug.LogError("ERROR creating connection");
            finishedAutologin = true;
        });

        yield return null;
        GameSetupManager.Instance.Init();

        while (!finishedAutologin)
        {
            yield return null;
        }
       
        FinishedLoadingSequance();
    }

    private void FinishedLoadingSequance()
    {
        FlowManager.Instance.DisplayMainMenu();
    }
}
