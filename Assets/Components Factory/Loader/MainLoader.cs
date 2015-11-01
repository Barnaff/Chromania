﻿using UnityEngine;
using System.Collections;

public class MainLoader : MonoBehaviour {

	#region Public Properties

	public GameObject ComponentsFactoryPrefab;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () 
	{
		StartLoadingSequance();
	}

	#endregion


	#region Private

	private void StartLoadingSequance()
	{
		GameObject componentFactory = Instantiate(ComponentsFactoryPrefab) as GameObject;
		if (componentFactory != null)
		{
			IGameDataLoader gameDataLoader = ComponentsFactory.GetAComponent<IGameDataLoader>() as IGameDataLoader;
			if (gameDataLoader != null)
			{
				gameDataLoader.LoadGameData(()=>
				{
                    IScreensFlow screenFlowManager = ComponentsFactory.GetAComponent<IScreensFlow>() as IScreensFlow;
                    if (screenFlowManager != null)
                    {
                        StartCoroutine(StartGameScene());
                    }
                });
			}
		}
		else
		{
			Debug.LogError("ERROR: Components Factory could not be loaded!");
		}

  
    }

    IEnumerator StartGameScene()
    {
        yield return new WaitForSeconds(1.0f);
        IScreensFlow screenFlowManager = ComponentsFactory.GetAComponent<IScreensFlow>() as IScreensFlow;
        if (screenFlowManager != null)
        {
            Debug.Log("load lobby scene");
           screenFlowManager.DisplayMenuScene();
        }
    }

	#endregion

}
