using UnityEngine;
using System.Collections;
using System;

public class LoadScene : MonoBehaviour 
{
	public string sceneBundle;
	public string sceneName;
	
	IEnumerator Start () 
	{
		while (!AssetManager.instance.isReady)
			yield return null;
		
		AssetManager.instance.LoadBundle (sceneBundle);
		
		while (!AssetManager.instance.IsBundleLoaded(sceneBundle))
			yield return null;
		
		Application.LoadLevel (sceneName);
	}
}