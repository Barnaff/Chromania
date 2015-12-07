using UnityEngine;
using System.Collections;

public class SceneLoaderUtil  {

	#region Public

	public static void LoadScene(string sceneName, System.Action sceneLoadedAction)
	{
		GameObject sceneLoaderContainer = new GameObject();
		SceneLoader sceneLoader = sceneLoaderContainer.AddComponent<SceneLoader>() as SceneLoader;
		sceneLoader.LoadScene(sceneName, sceneLoadedAction);
	}

	#endregion
}


public class SceneLoader : MonoBehaviour
{
	private string _sceneName;
	private System.Action _sceneLoadedAction;

	#region Public
	
	public void LoadScene(string sceneName, System.Action sceneLoadedAction)
	{
		_sceneName = sceneName;
		_sceneLoadedAction = sceneLoadedAction;
		this.gameObject.name = _sceneName + " Scene Loader";
		Application.LoadLevelAsync(_sceneName);
	}

	#endregion


	#region Initialize
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	#endregion


	#region Event Handler

	void OnLevelWasLoaded(int level) {
		string loadedSceneName = Application.loadedLevelName;
		if (loadedSceneName == _sceneName)
		{
			if (_sceneLoadedAction != null)
			{
				_sceneLoadedAction();
			}
			_sceneLoadedAction = null;
			Destroy(this.gameObject);
		}
	}

	#endregion
}
