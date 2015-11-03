using UnityEngine;
using System.Collections;

public class MainLoader : MonoBehaviour {

	#region Public Properties
    
    [SerializeField]
	public GameObject _componentFactoryPrefab;

    [SerializeField]
    public GameObject _loadingSplash;

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
		GameObject componentFactory = Instantiate(_componentFactoryPrefab) as GameObject;
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
        DontDestroyOnLoad(_loadingSplash);
        yield return new WaitForSeconds(1.0f);
        IScreensFlow screenFlowManager = ComponentsFactory.GetAComponent<IScreensFlow>() as IScreensFlow;
        if (screenFlowManager != null)
        {
            Debug.Log("load lobby scene");
            screenFlowManager.DisplayMenuScene();

            GameUtils.StartDelayedCall(1.0f, "", () =>
            {
                Destroy(_loadingSplash);
            });
        }
    }

	#endregion

}
