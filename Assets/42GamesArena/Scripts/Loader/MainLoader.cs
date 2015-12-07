using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.CrashLog;


public class MainLoader : MonoBehaviour {
	
	#region Private Properties

	[SerializeField]
	private GameObject ComponentFactoryPrefab;

	[SerializeField]
	private string NextSceneName;

	private int _internalLoadCount = 0;

	[SerializeField]
	private string _version;

	[SerializeField]
	private string _unityPerfID;

	private const string CURRENT_VERSION_KEY = "currentVersion";

	#endregion


	#region Public

	[SerializeField]
	private Text _progressLabel;

	#endregion


	#region Initialize

	// Use this for initialization
	IEnumerator Start () 
	{
		GameObject componentsFactory = Instantiate(ComponentFactoryPrefab) as GameObject;

		if (!string.IsNullOrEmpty(_unityPerfID))
		{
			string authToken = "";
			if (PlayerPrefsUtil.HasKey(ServerRequestKeys.SERVER_KEY_AUTH_TOKEN))
			{
				authToken = PlayerPrefsUtil.GetString(ServerRequestKeys.SERVER_KEY_AUTH_TOKEN);
			}
			CrashReporting.Init(_unityPerfID, _version, authToken);
		}

		if (_progressLabel != null)
		{
			_progressLabel.gameObject.SetActive(false);
		}

		yield return null;
		if (componentsFactory != null)
		{
			string currentVersion = "0";
			if (PlayerPrefsUtil.HasKey(CURRENT_VERSION_KEY))
			{
				currentVersion = PlayerPrefsUtil.GetString(CURRENT_VERSION_KEY);
			}

			if (currentVersion != _version)
			{
				Debug.Log("new version avalable!");
			}
		
			Hash128 versionHash = Hash128.Parse(_version);
			StartCoroutine(LoadAssetBundles(versionHash));
			StartCoroutine(PerformInitialLogin());

		}

		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics ,AnalyticsEvents.ANALYTICS_CATEGORY_APPLICATION, AnalyticsEvents.ANALYTICS_EVENT_APP_LAUNCH);

	}
	
	#endregion


	#region Startup Sequance

	IEnumerator LoadAssetBundles(Hash128 version = new Hash128())
	{
		_internalLoadCount++;
		AssetManager assetsManager = ComponentFactory.GetAComponent<AssetManager>() as AssetManager;
		if (assetsManager != null)
		{
			assetsManager.LoadManifest(version);

			assetsManager.OnProgressUpdate += OnProgressUpdate;

			while (!assetsManager.isReady)
			{
				yield return new WaitForEndOfFrame();
			}

			assetsManager.LoadBundle("lobby/default");

			while (!assetsManager.IsBundleLoaded("lobby/default"))
			{
				yield return new WaitForEndOfFrame();
			}
		}

		Debug.Log("finished loading assets");

		FinishedLoading();
		yield return null;
	}

	IEnumerator PerformInitialLogin()
	{
		_internalLoadCount++;
		IFacebookManager facebookManager = ComponentFactory.GetAComponent<IFacebookManager>();

		if (facebookManager != null && facebookManager.FacebookEnabled)
		{
			IAccount accountManager = ComponentFactory.GetAComponent<IAccount>() as IAccount;
			accountManager.Login(()=>
			                     {
				facebookManager.InitFacebookWithCompletion(()=>
				                                           {

					facebookManager.PerformFacebookLogin((sucsess)=>
					                                     {
						facebookManager.GetFacebookUserDetails((facebookUser)=>
						                                       {

							string facebookAcsessToekn =  FB.AccessToken;
							accountManager.FacebookLogin(facebookAcsessToekn, ()=>
							                             {
								StartCoroutine(LoadPortalConfiguraions());
								
								FinishedLoading();
							});

						}, (error)=>
						{
							StartCoroutine(LoadPortalConfiguraions());
							
							FinishedLoading();

							Debug.LogError(error.ErrorDescription);
						});

					});
				});
			});

		}
		else
		{
			AccountLogin();
			FinishedLoading();
		}

		yield return null;
	}

	IEnumerator LoadPortalConfiguraions()
	{
		_internalLoadCount++;
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;
		if (portalManager != null)
		{
			portalManager.LoadPortalConfiguration((portal)=>
			                                      {

				IHearts heartsManager = ComponentFactory.GetAComponent<IHearts>();
				if (heartsManager != null)
				{
					heartsManager.HeartsEnabled = portal.EnableHearts;
				}

				Debug.Log("portal loaded");

				InitializeAfterLogin();

				FinishedLoading();
			});
		}

		yield return null;
	}

	private void InitializeAfterLogin()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		IShop shopManager = ComponentFactory.GetAComponent<IShop>();
		IAds adsManager = ComponentFactory.GetAComponent<IAds>();

		_internalLoadCount++;
		if (inventoryManager != null)
		{
			inventoryManager.Initialize(()=>
			                            {
				FinishedLoading();
			});
		}

		if (shopManager != null)
		{
			shopManager.Initialize(()=>
			                       {

			});
		}

		if (adsManager != null)
		{
			adsManager.Initialize(()=>
			                      {

			});
		}

	}


	private void FinishedLoading()
	{
		_internalLoadCount--;
		Debug.Log("_internalLoadCount, current load count: " + _internalLoadCount);

		if (_internalLoadCount == 0)
		{
			PlayerPrefsUtil.SetString(CURRENT_VERSION_KEY, _version);
			Application.LoadLevel(NextSceneName);
		}
	}

	private void AccountLogin()
	{
		_internalLoadCount++;
		IAccount accountManager = ComponentFactory.GetAComponent<IAccount>() as IAccount;
		if (accountManager.HasLocalUser)
		{
			Debug.Log("Local User found - performing login (auth token: " + PlayerPrefsUtil.GetString(ServerRequestKeys.SERVER_KEY_AUTH_TOKEN));
			accountManager.UpdateDevice(()=>
			                            {
				Debug.Log("finished loagin");
				StartCoroutine(LoadPortalConfiguraions());

				FinishedLoading();
			});
		}
		else
		{
			Debug.Log("No Local user found - creating a new user");
			accountManager.Login(()=>
			                     {

				StartCoroutine(LoadPortalConfiguraions());

				FinishedLoading();
			});
		}
	}

	private void OnProgressUpdate(float progressValue)
	{
		if (_progressLabel != null)
		{
			_progressLabel.gameObject.SetActive(true);
			_progressLabel.text = progressValue.ToString("F2") + "%";
		}
	}



	#endregion
}
