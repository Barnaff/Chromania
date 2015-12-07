using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AssetImageController))]
public class AssetImageControllerInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}

#endif


public class AssetImageController : Image {

	#region Public Properties

	public string BundleName;
	public string AssetName;

	#endregion


	#region Initialize



	override protected void Awake()
	{
		StartCoroutine(LoadAsset());
	}

	#endregion


	#region Private

	IEnumerator LoadAsset()
	{
		if (!string.IsNullOrEmpty(AssetName) && Application.isPlaying)
		{
			AssetManager assetsManager = ComponentFactory.GetAComponent<AssetManager>() as AssetManager;

			if (assetsManager == null)
			{
				Debug.LogError("ERROR - Cant find Asset Manager");
				yield break;
			}
			while (!assetsManager.isReady)
			{
				Debug.Log("waiting for assets manager to be ready");
				yield return null;
			} 
			assetsManager.GetAssetFromBundle(BundleName, AssetName, (asset)=>
			{
				if (asset != null)
				{
					Texture2D texture = asset as Texture2D;
					this.sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), Vector2.zero);
					this.color = Color.white;
				}
				
			});
			yield break;
		}
	}

	#endregion


}



