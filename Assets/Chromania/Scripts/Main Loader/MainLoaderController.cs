using UnityEngine;
using System.Collections;

public class MainLoaderController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    { 
        Application.targetFrameRate = 60;

        StartCoroutine(LoadingSequanceCorutine());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator LoadingSequanceCorutine()
    {
        Fabric.Runtime.Fabric.Initialize();
        
        yield return 0f;

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

        InventoryManager inventoryManager = InventoryManager.Instance;

        yield return null;
        GameSetupManager.Instance.Init();
        ObjectivesManager.Instance.Init();

        while (!finishedAutologin)
        {
            yield return null;
        }
       
        FinishedLoadingSequance();
    }

    private void FinishedLoadingSequance()
    {
        FlowManager.Instance.DisplayFirstScreenAfterLaunch();
    }
}
