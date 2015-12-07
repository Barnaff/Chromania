using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(IServerRequests))]
public class AdsManager : MonoBehaviour, IAds {

	[SerializeField]
	private string _unityAdsID;

	[SerializeField]
	private bool _useLocalData;

	[SerializeField]
 	private List<AdDataModel> _adsList;
	
	private List<AdDataModel> _usedAds;

	private IServerRequests requestsManager;


	#region Initialization

	// Use this for initialization
	void Start ()
	{	
		requestsManager = ComponentFactory.GetAComponent<IServerRequests>();

		//Advertisement.Initialize(_unityAdsID);
	}

	#endregion


	#region IAds implementation

	public event AdPresentedDelegate OnAdPresented;

	public event AdClickedDelegate OnAdClicked;

	public void Initialize(System.Action completionAction)
	{
		if (!_useLocalData)
		{
//			GetAdsListFromServer((adsList)=>
//			                     {
//				_adsList = adsList;
//				if (completionAction != null)
//				{
//					completionAction();
//				}
//			});
		}
		else
		{
			if (completionAction != null)
			{
				completionAction();
			}
		}

		if (completionAction != null)
		{
			completionAction();
		}
	}

	public bool AdsReady 
	{ 
		get
		{
			return (_adsList != null);
		}
	}
	 
	public void GetAdsListFromServer (System.Action <List<AdDataModel>> completionAction)
	{
		if (_useLocalData)
		{
			completionAction(_adsList);
			return;
		}
		string command = ServerCommands.SERVER_COMMAND_GET_INCENT_LIST;
		Hashtable data = new Hashtable();

		requestsManager.SendServerRequest(command, data, (response)=>
		                                  {

			List<AdDataModel> adsList = new List<AdDataModel>();
			ArrayList adsArray = response[ServerRequestKeys.SERVER_RESPONSE_KEY_ADS_LIST] as ArrayList;

			foreach (Hashtable adData in adsArray)
			{
				AdDataModel ad = new AdDataModel(adData);
				ad.PresenetationType = AdPresentationType.Tile;
				adsList.Add(ad);
			}

			_adsList = adsList;
			if (completionAction != null)
			{
				completionAction(adsList);
			}

		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);

		});
	}

	public void GetAdForShopitem(ShopItemDataModel shopItem, System.Action <AdDataModel> completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_GET_INCENT;
		Hashtable data = new Hashtable();

		data.Add(ServerRequestKeys.SERVER_AD_SHOP_ITEM, shopItem.ItemID);

		requestsManager.SendServerRequest(command, data, (response)=>
		                                  {

			Hashtable adObject = response[ServerRequestKeys.SERVER_RESPONSE_KEY_AD_OBJECT] as Hashtable;
			if (adObject != null)
			{
				AdDataModel ad = new AdDataModel(adObject);
				completionAction(ad);
			}
			else
			{
				if (completionAction != null)
				{
					completionAction(null);
				}
			}

			
		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);
		});
	}

	public void GetAdForTile(AdTileDataModel tile, System.Action <AdDataModel> completionAction)
	{

		AdDataModel ad = GetAdForPresentation(AdPresentationType.Tile);

		if (completionAction != null)
		{
			completionAction(ad);
		}


	}

	public AdDataModel GetAdForPresentation (AdPresentationType adPresentationType)
	{
		List<AdDataModel> avalableAdsForType = new List<AdDataModel>();
		foreach (AdDataModel ad in _adsList)
		{
			if (ad.PresenetationType == adPresentationType)
			{
				avalableAdsForType.Add(ad);
			}
		}

		AdDataModel selectedAd = null;

		if (avalableAdsForType.Count > 0)
		{
			selectedAd = avalableAdsForType[Random.Range(0,avalableAdsForType.Count -1)];
		}

		if (selectedAd == null)
		{
			if (_usedAds != null)
			{
				_adsList.AddRange(_usedAds);
				_usedAds = null;

				selectedAd = GetAdForPresentation(adPresentationType);
			}
		} 


		if (selectedAd != null)
		{
			if (_usedAds == null)
			{
				_usedAds = new List<AdDataModel>();
			}
			_usedAds.Add(selectedAd);
			_adsList.Remove(selectedAd);
		}

		return selectedAd;
	}

	public void ClickAdFromTile (AdDataModel ad, AdTileDataModel tile)
	{
		if (string.IsNullOrEmpty(ad.AdTargetURL))
		{
			throw new System.Exception("Ad Target URL is Empty!");
		}
		Debug.Log("activate ad URL: " + ad.AdTargetURL);
		Application.OpenURL(ad.AdTargetURL);
	}

	public void DisplayVideoAd(System.Action <bool> completionAction)
	{
		Debug.LogError("ERROR - Video ads are not implementd in the project!");
//		Advertisement.Show(null, new ShowOptions {
//			resultCallback = result => {
//				Debug.Log(result.ToString());
//
//				switch (result)
//				{
//				case ShowResult.Finished:
//				{
//					completionAction(true);
//					break;
//				}
//				case ShowResult.Failed:
//				case ShowResult.Skipped:
//				default:
//				{
//					completionAction(false);
//					break;
//				}
//				}
//			}
//		});
	}

	#endregion
}
