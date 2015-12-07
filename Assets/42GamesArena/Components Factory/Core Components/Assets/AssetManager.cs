
#define ASSET_MANAGER_DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ProgressUpdateDelegate(float value);
public delegate void DownloadCompleteDelegaete(string bundleName);

public class AssetManager : MonoBehaviour 
{
	#region Public Properties

	/// <summary>
	/// The instance.
	/// </summary>
	public static AssetManager instance;

	/// <summary>
	/// Notify the progress of asst downloading
	/// </summary>
	public ProgressUpdateDelegate OnProgressUpdate;

	/// <summary>
	/// Notify when downloading asset completes.
	/// </summary>
	public DownloadCompleteDelegaete OnDownloadComplete;
	
	/// <summary>
	/// Gets a value indicating whether this <see cref="AssetManager"/> is ready.
	/// </summary>
	/// <value><c>true</c> if is ready; otherwise, <c>false</c>.</value>
	public bool isReady
	{
		get
		{
			return !object.ReferenceEquals(manifest, null);
		}
	}

	#endregion

	#region Private Properties

	/// <summary>
	/// The bundle variants.
	/// </summary>
	[SerializeField] 
	private string[] bundleVariants;

	/// <summary>
	/// The path to bundles.
	/// </summary>
	[SerializeField] 
	private string pathToBundles;
	
	/// <summary>
	/// If marked true, will clean the cache everytime the script is loaded.
	/// </summary>
	[SerializeField] 
	private bool CleanCache;

	#endregion

	#region Private Properties

	private Dictionary<string, AssetBundle> bundles;
	private AssetBundleManifest manifest = null;
	private string platform;
	private List<string> loadingAssetsBundles = new List<string>();

	#endregion


	#region Lifecycle

	void Awake()
	{
		if (object.ReferenceEquals (instance, null)) 
		{
			instance = this;
		}
		else if (!object.ReferenceEquals (instance, this))
		{
			//Destroy (gameObject);
			return;
		}
		
		DontDestroyOnLoad (gameObject);
		
		#if UNITY_IOS
		platform = "iOS";
		#elif UNITY_ANDROID
		platform = "Android";
		#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		platform = "PC";
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		platform = "OSX";
		#elif UNITY_WEBPLAYER
		platform = "Web";
		#else
		platform = "error";
		Debug.LogError("unsupported platform");
		#endif

		if (CleanCache)
		{
			#if ASSET_MANAGER_DEBUG
				Debug.Log("space before clean: " + Caching.spaceOccupied);
			#endif
			Caching.CleanCache();
			#if ASSET_MANAGER_DEBUG
				Debug.Log("Cleaned Cache, size after clean: " + Caching.spaceOccupied);
			#endif
		}

		pathToBundles += platform + "/";
		bundles = new Dictionary<string, AssetBundle> ();
	//	StartCoroutine ("LoadManifest");

	}

//	void OnDisable()
//	{
//		#if DEBUG
//		Debug.Log ("Unloading Bundles");
//		#endif
//		
//		if (bundles != null)
//		{
//			foreach(KeyValuePair<string, AssetBundle> entry in bundles)
//			{
//				entry.Value.Unload(false);
//			}
//			bundles.Clear ();
//		}
//		
//		#if DEBUG
//		Debug.Log ("Bundles Unloaded");
//		#endif
//	}

	#endregion


	#region Public

	public void LoadManifest(Hash128 versionHash, System.Action completionAction = null)
	{
		StartCoroutine(LoadManifestCorutine(versionHash, completionAction));
	}

	/// <summary>
	/// Determines whether this instance is bundle loaded the specified bundleName.
	/// </summary>
	/// <returns><c>true</c> if this instance is bundle loaded the specified bundleName; otherwise, <c>false</c>.</returns>
	/// <param name="bundleName">Bundle name.</param>
	public bool IsBundleLoaded(string bundleName)
	{
		return bundles.ContainsKey (bundleName);
	}

	/// <summary>
	/// Gets the asset from bundle.
	/// </summary>
	/// <returns>The asset from bundle.</returns>
	/// <param name="bundleName">Bundle name.</param>
	/// <param name="assetName">Asset name.</param>
	public Object GetAssetFromBundle(string bundleName, string assetName)
	{
		if (!IsBundleLoaded (bundleName))
		{
			return null;
		}
		
		return bundles [bundleName].LoadAsset (assetName);
	}

	/// <summary>
	/// Gets the asset from bundle (async)
	/// Will download the Bundle if needed
	/// </summary>
	/// <param name="bundleName">Bundle name.</param>
	/// <param name="assetName">Asset name.</param>
	/// <param name="callBackAction">Call back action.</param>
	public void GetAssetFromBundle(string bundleName, string assetName, System.Action <object> callBackAction)
	{
		StartCoroutine(GetAssetFromBundleCorutine(bundleName, assetName, callBackAction));
	}

	/// <summary>
	/// Loads the bundle.
	/// </summary>
	/// <returns><c>true</c>, if bundle was loaded, <c>false</c> otherwise.</returns>
	/// <param name="bundleName">Bundle name.</param>
	public bool LoadBundle(string bundleName)
	{
		//See if bundle is already loaded
		if (IsBundleLoaded(bundleName))
		{
			return true;
		}
		
		//If bundle isn't loaded, load it
		StartCoroutine(LoadBundleCoroutine(bundleName));
		return false;
	}

	public void UnloadAllAssets()
	{
		foreach (string bundleName in bundles.Keys)
		{
			bundles[bundleName].Unload(true);
		}
		bundles.Clear();
		Caching.CleanCache();
	}

	#endregion


	#region Private

	IEnumerator LoadManifestCorutine (Hash128 version, System.Action completionAction = null) 
	{
		#if ASSET_MANAGER_DEBUG
		Debug.Log( "Loading Manifest: " + pathToBundles + platform + " version: " + version.ToString());

		if (Caching.IsVersionCached(pathToBundles + platform, version))
		{
			Debug.Log("manifest verion is avalable on cache");
		}
		#endif

		WWW www = WWW.LoadFromCacheOrDownload(pathToBundles + platform, version);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
			yield break;
		}
		manifest = (AssetBundleManifest)www.assetBundle.LoadAsset("AssetBundleManifest", typeof(AssetBundleManifest));
		#if ASSET_MANAGER_DEBUG
		Debug.Log("loaded manifest: " + manifest.ToString());
		#endif
		yield return null;
		www.assetBundle.Unload(false);
		#if ASSET_MANAGER_DEBUG
		if (!isReady)
		{
			Debug.LogError ("There was an error loading manifest");
		}
		else
		{
			Debug.Log ("Manifest loaded successfully");
			if (completionAction != null)
			{
				completionAction();
			}
		}
		#endif
	}
	



	IEnumerator GetAssetFromBundleCorutine(string bundleName, string assetName, System.Action <object> callBackAction)
	{
		if (!IsBundleLoaded (bundleName))
		{
			yield return StartCoroutine(LoadBundleCoroutine(bundleName));
		}

		while (loadingAssetsBundles.Contains(bundleName))
		{
			yield return new WaitForEndOfFrame();
		}

		if (callBackAction != null && bundles.ContainsKey(bundleName))
		{
			callBackAction(bundles [bundleName].LoadAsset (assetName));
		}
	}
	
	IEnumerator LoadBundleCoroutine(string bundleName)
	{
		//Bundle has already been loaded
		if (IsBundleLoaded (bundleName) || loadingAssetsBundles.Contains(bundleName))
		{
			yield break;
		}
		
		string[] dependencies = manifest.GetAllDependencies (bundleName);
		for (int i = 0; i < dependencies.Length; i++)
		{
			yield return StartCoroutine (LoadBundleCoroutine (dependencies [i]));
		}
		
		bundleName = RemapVariantName (bundleName);
		string url = pathToBundles + bundleName;
		#if ASSET_MANAGER_DEBUG
		Debug.Log ("Beginning to load " + bundleName + " / " + manifest.GetAssetBundleHash(bundleName));
		Debug.Log("caching enabled: " + Caching.enabled);
		Debug.Log("caching space: " + Caching.spaceOccupied);
		Debug.Log("expirationDelay: " + Caching.expirationDelay);
		#endif
		loadingAssetsBundles.Add(bundleName);
		if (Caching.IsVersionCached(url, manifest.GetAssetBundleHash(bundleName)))
		{
			#if ASSET_MANAGER_DEBUG
			Debug.Log("got cached version");
			#endif
		}

		WWW www = WWW.LoadFromCacheOrDownload(url, manifest.GetAssetBundleHash(bundleName));
		while (!www.isDone)
		{
			#if ASSET_MANAGER_DEBUG
			//Debug.Log("progress: " + www.progress);
			#endif
			if (OnProgressUpdate != null)
			{
				OnProgressUpdate(www.progress);
			}
			yield return null;
		}
		yield return www;
		if (loadingAssetsBundles.Contains(bundleName))
		{
			loadingAssetsBundles.Remove(bundleName);
		}
		if(!string.IsNullOrEmpty(www.error))
		{
			#if ASSET_MANAGER_DEBUG
			Debug.LogError(www.error);
			#endif
			yield break;
		}
		bundles.Add(bundleName, www.assetBundle);
		#if ASSET_MANAGER_DEBUG
		Debug.Log ("Finished loading " + bundleName);
		#endif

		if (OnDownloadComplete != null)
		{
			OnDownloadComplete(bundleName);
		}
	}

	private string RemapVariantName(string assetBundleName)
	{
		string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();
		if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0 )
		{	
			return assetBundleName;
		}	
		string[] splitBundleName = assetBundleName.Split('.');
		string[] candidateBundles = System.Array.FindAll (bundlesWithVariant, element => element.StartsWith(splitBundleName [0]));
		int index = -1;
		for(int i = 0; i < bundleVariants.Length; i++)
		{
			index = System.Array.IndexOf(candidateBundles, splitBundleName[0] + "." + bundleVariants[i]);
			if(index != -1)
			{
				return candidateBundles[index];
			}
		}
		return assetBundleName;
	}

	#endregion

}