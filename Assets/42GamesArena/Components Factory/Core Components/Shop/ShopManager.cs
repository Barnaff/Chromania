using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour, IShop {

	#region Private
	 
	[SerializeField]
	private List<ShopItemDataModel> _shopItems;

	private bool _incentActivated;

	private bool _isPurchasing = false;

	private System.Action <bool> _completionAction;

	private bool _inappShopInitialized = false;

	private ShopItemDataModel _currentBuyngShopItem = null;

	[SerializeField]
	private bool _enableLogging;

	private LoadingIndicatorPopupController _loadingIndicator;

	private List<StoredPurchaseDataObject> _storedPurchases = null;

	#endregion


	#region Initialization

	void Start () 
	{
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;
		if (portalManager != null)
		{
			portalManager.OnLobbyUpdated += HandleOnLobbyUpdated;
		}

		if (PlayerPrefsUtil.HasKey("_storedPurchases"))
		{
			_storedPurchases = PlayerPrefsUtil.GetObject("_storedPurchases") as List<StoredPurchaseDataObject>;
		}
	}

	void OnDisable()
	{
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;
		if (portalManager != null)
		{
			portalManager.OnLobbyUpdated -= HandleOnLobbyUpdated;
		}
	}

	void OnApplicationPause(bool pauseStatus) 
	{
		if (!pauseStatus)
		{
			if (_incentActivated)
			{
				Debug.Log("check incent");


				IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
				if (inventoryManager != null)
				{
					inventoryManager.RefreshInventory(true, ()=>
					                                  {
						if (_loadingIndicator != null)
						{
							_loadingIndicator.ClosePopup();
							_loadingIndicator = null;
						}
					});
				}
				AnalyticsUtil.SendEventHit((AnalyticsServiceType.GoogleAnalytics | AnalyticsServiceType.Appsflyer), 
				                           AnalyticsEvents.ANALYTICS_CATEGORY_SHOP, AnalyticsEvents.ANALYTICS_EVENT_SHOP_OPEN_RETURN_AFTER_INCENT, 
				                           _currentBuyngShopItem.ItemName , _currentBuyngShopItem.ItemID);

				_incentActivated = false;
				_currentBuyngShopItem = null;
			}
		}
	}

	#endregion


	#region IShop implementation

	public void Initialize(System.Action completionAction)
	{

	}

	public List<ShopItemDataModel> GetShopItems (StoreType storeType)
	{
		if (_shopItems != null)
		{
			List<ShopItemDataModel> shopItemsList = new List<ShopItemDataModel>();
			
			foreach (ShopItemDataModel shopItem in _shopItems)
			{
				if (shopItem.Store == storeType)
				{
					shopItemsList.Add(shopItem);
				} 
				else if (storeType == StoreType.CoinsStore)
				{
					if (shopItem.Product != null)
					{
						if (shopItem.Product.Type == "1")
						{
							shopItemsList.Add(shopItem);
						}
					}
				}
			}
			
			return shopItemsList;
		}

		Debug.LogError("ERROR - Shopitems list for lobby is empty!");
		return null;
	}

	public void PurchaseItem (ShopItemDataModel shopItem, System.Action<bool> completionAction, System.Action<ErrorObj> failAction)
	{
		if (_enableLogging)
		{
			Debug.Log("<color=lightblue> Starting purchase sequanse for: " + shopItem.ItemID + " - " + shopItem.ItemName + "</color>");
		}

		if (!IsItemValid(shopItem))
		{
			ErrorObj error = ErrorObj.CreateWithError(0, "Invalid item for purchase");
			failAction(error);
			return;
		}

		switch (shopItem.Price.ActionType)
		{
		case ePriceAction.BUY_ITEM:
		case ePriceAction.IAP:
		{
			PurchaseWithIAP(shopItem, completionAction, failAction);
			break;
		}
		case ePriceAction.COINS:
		{
			PurchaseWithCoins(shopItem, completionAction, failAction);
			break;
		}
		case ePriceAction.INCENT:
		{
			PurchaseWithIncent(shopItem, completionAction, failAction);
			break;
		}
		case ePriceAction.SHARE_LINK:
		{
			PurchaseWithShare(shopItem, completionAction, failAction);
			break;
		}
		case ePriceAction.WATCH:
		{
			PurchaseWithWatchVideo(shopItem, completionAction, failAction);
			break;
		}
		default:
		{
			break;
		}

		}

	}

	public bool CanBuyItem (ShopItemDataModel shopItem)
	{
		return false;
	}

	#endregion


	#region Private
	 
	private bool IsItemValid(ShopItemDataModel shopItem)
	{
		return true;
	}

	private bool IsIAP(ShopItemDataModel shopItem)
	{
		if (shopItem.Price.ActionType == ePriceAction.IAP)
		{
			return true;
		}
		return false;
	}
	
	private void HandleOnLobbyUpdated (LobbyDataModel lobbyData)
	{
		if (lobbyData.LobbyShopItems != null)
		{
			_shopItems = lobbyData.LobbyShopItems;
			SetupIAPProducts();
		}
	}


	private void SetupIAPProducts()
	{
		foreach (ShopItemDataModel shopItem in _shopItems)
		{
			if (shopItem.Price.ActionType == ePriceAction.IAP)
			{
				UM_InAppProduct product = UltimateMobileSettings.Instance.GetProductById(shopItem.ItemID.ToString());
				if (product != null)
				{
					UltimateMobileSettings.Instance.RemoveProduct(product);
				}
				product = new UM_InAppProduct();
				product.id = shopItem.ItemID.ToString();
				product.IsConsumable = true;
				product.IOSId = shopItem.Price.StoreLink;
				product.AndroidId = shopItem.Price.StoreLink;

				UltimateMobileSettings.Instance.AddProduct(product);
			}
		}

		if (!_inappShopInitialized)
		{
			UM_InAppPurchaseManager.OnBillingConnectFinishedAction += HandleOnBillingConnectFinishedAction;
			UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += HandleOnPurchaseFlowFinishedAction;
			UM_InAppPurchaseManager.OnPurchasesRestoreFinishedAction += HandleOnPurchasesRestoreFinishedAction;


			UM_InAppPurchaseManager.Instance.Init();
			_inappShopInitialized = true;

			Debug.Log("IAP Initialized");
		}
	}

	
	#endregion


	#region Purchasing Methods

	private void PurchaseWithIAP(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action<ErrorObj> failAction)
	{
		if (DebugManager.OverrideIAP)
		{
			Debug.Log("purchase with IAP  - Override purchase");
			IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
			if (inventoryManager.HasInventoryItem(shopItem.Product.Type))
			{
				InventoryItemDataModel inventoryItem = inventoryManager.GetInventoryItem(shopItem.Product.Type);
				inventoryManager.ConsumeFromInventory(inventoryItem, -shopItem.Product.Amount, completionAction);
			}
			else
			{
				InventoryItemDataModel inventoryItem = new InventoryItemDataModel(null);
				inventoryItem.ItemType = shopItem.Product.Type;
				inventoryItem.Amount = shopItem.Product.Amount;
				inventoryManager.AddInventoryItem(inventoryItem, completionAction);
			}
		}
		else
		{
			StartIAPSequance(shopItem, completionAction);
		}
	}

	private void PurchaseWithCoins(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action<ErrorObj> failAction)
	{
		if (_loadingIndicator == null)
		{
			IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
			if (popupsManager != null)
			{
				_loadingIndicator = popupsManager.DisplayPopup<LoadingIndicatorPopupController>();
			}
		}

		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			if (inventoryManager.CoinsCount >= shopItem.Price.Value)
			{
				// have coins

				// buy the item from the server
				PurchaseItemFromServer(shopItem.ItemID, null ,completionAction, failAction);

			}
			else
			{

				if (_loadingIndicator != null)
				{
					_loadingIndicator.ClosePopup();
					_loadingIndicator = null;
				}

				// not enough coins!
				IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
				if (popupsManager != null)
				{
					NotEnoughCurrencyPopupController notEnoughCurrencyPopup = popupsManager.DisplayPopup<NotEnoughCurrencyPopupController>();
					notEnoughCurrencyPopup.SetShopItem(shopItem);
					notEnoughCurrencyPopup.PopupClosedAction = ()=>
					{

					};
				}
			}
		}
	}

	private void PurchaseWithIncent(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action<ErrorObj> failAction)
	{
		if (!string.IsNullOrEmpty(shopItem.Price.Link))
		{
			_incentActivated = true;
			_currentBuyngShopItem = shopItem;

			if (_loadingIndicator == null)
			{
				IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
				if (popupsManager != null)
				{
					_loadingIndicator = popupsManager.DisplayPopup<LoadingIndicatorPopupController>();
				}
			}


			completionAction(true);
			Application.OpenURL(shopItem.Price.Link);

			AnalyticsUtil.SendEventHit((AnalyticsServiceType.GoogleAnalytics | AnalyticsServiceType.Appsflyer), 
			                           AnalyticsEvents.ANALYTICS_CATEGORY_SHOP, AnalyticsEvents.ANALYTICS_EVENT_SHOP_OPEN_INCENT_LINK, 
			                           shopItem.ItemName , shopItem.ItemID);
		}
		else
		{
			ErrorObj error = ErrorObj.CreateWithError(0, "ERROR - Incent link is not set!");
			failAction(error);
		}
	}

	private void PurchaseWithShare(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action<ErrorObj> failAction)
	{
		if (_enableLogging)
		{
			Debug.Log("<color=lightblue> Share to purchase </color>");
		}

		if (_loadingIndicator == null)
		{
			IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
			if (popupsManager != null)
			{
				_loadingIndicator = popupsManager.DisplayPopup<LoadingIndicatorPopupController>();
			}
		}

		IFacebookManager facebookManager = ComponentFactory.GetAComponent<IFacebookManager>();
		if (facebookManager != null)
		{
			IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
			if (popupsManager != null)
			{

			}
			if (facebookManager.IsLoggedIn())
			{
				facebookManager.ShareOnWall("share test title", "share caption", shopItem.Price.Link, (sucsess)=>
				                            {
					if (sucsess)
					{
						PurchaseItemFromServer(shopItem.ItemID, null, completionAction, failAction);
					}


				});
			}
			else
			{
				string errorMessage = "ERROR - trying to share when user is not logged in to facebbok!";
				ErrorObj error = ErrorObj.CreateWithError(0, errorMessage);
				failAction(error);

				if (_loadingIndicator != null)
				{
					_loadingIndicator.ClosePopup();
					_loadingIndicator = null;
				}
			}
		}
	}
	
	private void PurchaseWithWatchVideo(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action<ErrorObj> failAction)
	{
		IAds adsManager = ComponentFactory.GetAComponent<IAds>();
		if (adsManager != null)
		{
			adsManager.DisplayVideoAd((completed)=>
			                          {
				if (completed)
				{
					PurchaseItemFromServer(shopItem.ItemID, null, (sucsess)=>
					{
						completionAction(sucsess);
					}, (error)=>
					{
						failAction(error);
					});

				}
				else
				{
					completionAction(false);
				}
			});
		}
	}

	private void PurchaseItemFromServer(int shopItemId, string recipt, System.Action <bool> completionAction, System.Action <ErrorObj> failAction = null)
	{
		if (_loadingIndicator == null)
		{
			IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
			if (popupsManager != null)
			{
				_loadingIndicator = popupsManager.DisplayPopup<LoadingIndicatorPopupController>();
			}
		}

		string command = "";
		Hashtable data = new Hashtable();
		

		if (!string.IsNullOrEmpty(recipt))
		{
			command = ServerCommands.SERVER_COMMAND_VERIFY_PURCHASE;
			data.Add(ServerRequestKeys.SERVER_KEY_PURCHASE_DATA, recipt);
			data.Add(ServerRequestKeys.SERVER_KEY_SHOP_ITEM_ID, shopItemId);
		}
		else
		{
			command = ServerCommands.SERVER_COMMAND_BUY_SHOP_ITEM;
			data.Add(ServerRequestKeys.SERVER_KEY_SHOP_ITEM_ID, shopItemId);
		}

		if (string.IsNullOrEmpty(command))
		{
			Debug.LogError("ERROR - No command for purchasing shop item!");
			return;
		}
		
		IServerRequests serverRequestsManager = ComponentFactory.GetAComponent<IServerRequests>();
		
		serverRequestsManager.SendServerRequest(command, data, (response)=>
		                                        {
			_isPurchasing = false;

			if (response != null)
			{
				if (_storedPurchases != null)
				{
					foreach (StoredPurchaseDataObject storedPurchase in _storedPurchases)
					{
						if (storedPurchase.ShopitemId == shopItemId && storedPurchase.Recipt == recipt)
						{
							_storedPurchases.Remove(storedPurchase);
							PlayerPrefsUtil.SetObject("_storedPurchases", _storedPurchases);
							break;
						}
					}
				}

				if (completionAction != null)
				{
					completionAction(true);
				}
			}
			else
			{
				if (completionAction != null)
				{
					completionAction(false);
				}
			}

			if (_loadingIndicator != null)
			{
				_loadingIndicator.ClosePopup();
				_loadingIndicator = null;
			}


		}, (error)=>
		{
			_isPurchasing = false;
			Debug.LogError(error.ErrorDescription);
			if (completionAction != null)
			{
				completionAction(false);
			}

			if (_loadingIndicator != null)
			{
				_loadingIndicator.ClosePopup();
				_loadingIndicator = null;
			}

			if (failAction != null)
			{
				ErrorObj errorObj = ErrorObj.CreateWithError(0, error.ErrorDescription);
				failAction(errorObj);
			}


				
		}, true);
	}

	#endregion


	#region IAP Managment

	private void StartIAPSequance(ShopItemDataModel shopItem, System.Action <bool> completionAction)
	{
		if (_inappShopInitialized)
		{
			if (_loadingIndicator == null)
			{
				IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
				if (popupsManager != null)
				{
					_loadingIndicator = popupsManager.DisplayPopup<LoadingIndicatorPopupController>();
				}
			}

			if (_isPurchasing)
			{
				Debug.LogError("ERROR - Purchasing sequance already in progress, please wait untill it is finished");
				completionAction(false);
				return;
			}
			_currentBuyngShopItem = shopItem;
			_completionAction = completionAction;
			UM_InAppPurchaseManager.Instance.Purchase(shopItem.ItemID.ToString());
			_isPurchasing = true;


			AnalyticsUtil.SendEventHit((AnalyticsServiceType.GoogleAnalytics | AnalyticsServiceType.Appsflyer), 
			                           AnalyticsEvents.ANALYTICS_CATEGORY_SHOP, AnalyticsEvents.ANALYTICS_EVENT_SHOP_START_IAP, 
			                           shopItem.Price.StoreLink , shopItem.ItemID);

		}
		else
		{
			if (_loadingIndicator != null)
			{
				_loadingIndicator.ClosePopup();
				_loadingIndicator = null;
			}

			Debug.LogError("ERROR - IAP Shop is not initialized!");
		}
	}
	

	void HandleOnPurchasesRestoreFinishedAction (UM_BaseResult obj)
	{
		
	}
	
	void HandleOnPurchaseFlowFinishedAction (UM_PurchaseResult obj)
	{
		Debug.Log("HandleOnPurchaseFlowFinishedAction result: " + obj.isSuccess);

		if (obj.isSuccess)
		{
			// purchase sucsess
			string recipt = string.Empty;
			#if UNITY_IOS && !UNITY_EDITOR
			recipt = obj.IOS_PurchaseInfo.Receipt;
			#endif
			
			#if UNITY_ANDROID && !UNITY_EDITOR
			recipt = obj.Google_PurchaseInfo.token;
			#endif

			if (!string.IsNullOrEmpty(recipt))
			{
				if (_storedPurchases == null)
				{
					_storedPurchases = new List<StoredPurchaseDataObject>();
				}

				StoredPurchaseDataObject storedPurchaseData = new StoredPurchaseDataObject(_currentBuyngShopItem.ItemID, recipt);
				_storedPurchases.Add(storedPurchaseData);

				PlayerPrefsUtil.SetObject("_storedPurchases", _storedPurchases);
			}
			if (_currentBuyngShopItem != null)
			{
				PurchaseItemFromServer(_currentBuyngShopItem.ItemID, recipt, (sucsess)=>
				                       {
					_currentBuyngShopItem = null;
					if (_completionAction != null)
					{
						_completionAction(sucsess);
					}
					_isPurchasing = false;
				});
			}

			AnalyticsUtil.SendEventHit((AnalyticsServiceType.GoogleAnalytics | AnalyticsServiceType.Appsflyer), 
			                           AnalyticsEvents.ANALYTICS_CATEGORY_SHOP, AnalyticsEvents.ANALYTICS_EVENT_SHOP_FINISHED_IAP, 
			                           _currentBuyngShopItem.Price.StoreLink , _currentBuyngShopItem.ItemID);
		}
		else
		{
			AnalyticsUtil.SendEventHit((AnalyticsServiceType.GoogleAnalytics | AnalyticsServiceType.Appsflyer), 
			                           AnalyticsEvents.ANALYTICS_CATEGORY_SHOP, AnalyticsEvents.ANALYTICS_EVENT_SHOP_CANCLED_IAP, 
			                           _currentBuyngShopItem.Price.StoreLink , _currentBuyngShopItem.ItemID);

			if (_loadingIndicator != null)
			{
				_loadingIndicator.ClosePopup();
				_loadingIndicator = null;
			}

			// failed purchase
			if (_completionAction != null)
			{
				_completionAction(false);
			}
			_isPurchasing = false;
			_currentBuyngShopItem = null;

		}

	}
	
	void HandleOnBillingConnectFinishedAction (UM_BillingConnectionResult obj)
	{
		 //UM_InAppPurchaseManager.OnBillingConnectFinishedAction -= HandleOnBillingConnectFinishedAction;
		Debug.Log("<color=lightblue>HandleOnBillingConnectFinishedAction result: " + obj.isSuccess + "</color>"); 

		Debug.Log("<color=lightblue> message: " + obj.message + "</color>");

		foreach (ShopItemDataModel shopItem in _shopItems)
		{
			if (shopItem.Price.ActionType == ePriceAction.IAP)
			{
				UM_InAppProduct product = UM_InAppPurchaseManager.Instance.GetProductById(shopItem.ItemID.ToString());
				if (product != null && !string.IsNullOrEmpty(product.LocalizedPrice))
				{
					shopItem.Price.PriceString = product.LocalizedPrice;
				}
			}
		}

		if (PlayerPrefsUtil.HasKey("_storedPurchases"))
		{
			_storedPurchases = PlayerPrefsUtil.GetObject("_storedPurchases") as List<StoredPurchaseDataObject>;
		}

		if (_storedPurchases != null)
		{
			List<StoredPurchaseDataObject> tmpPurchasedList = new List<StoredPurchaseDataObject>();
			foreach (StoredPurchaseDataObject storedPurchase in _storedPurchases)
			{
				tmpPurchasedList.Add(storedPurchase);
			}
			
			foreach (StoredPurchaseDataObject storedPurchase in tmpPurchasedList)
			{
				PurchaseItemFromServer(storedPurchase.ShopitemId, storedPurchase.Recipt, (sucsess)=>
				                       {
					
				});
			}
		}

	}


	#endregion
}

[System.Serializable]
public class StoredPurchaseDataObject
{
	public int ShopitemId;
	public string Recipt;

	public StoredPurchaseDataObject(int shopItemId, string recipt)
	{
		ShopitemId = shopItemId;
		Recipt = recipt;
	}
}
