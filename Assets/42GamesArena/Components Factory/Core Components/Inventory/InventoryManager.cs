using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IInventory 
{

	[SerializeField]
	private string _inventoryResultKey;

	[SerializeField]
	private string _productReceivedKey;

	[SerializeField]
	private string _coinsItemType;

	[SerializeField]
	private bool _auteUpdate;

	[SerializeField]
	private bool _displayAquirePopup;

	[SerializeField]
	private List<InventoryItemDataModel> _inventoryItems;

	private IServerRequests serverRequestsManager;

	private bool _isUpdatingDelta = false;

	private const string COINS_CHANGE_DELTA = "coinsChangeDelta";

	private bool _checkForInventoryAdditions = false;

	#region Initialization

	void Start () 
	{
		INetwork networkManager = ComponentFactory.GetAComponent<INetwork>();
		if (networkManager != null)
		{
			// register to the network manager
			// listen to any result containing an Inventory update
			networkManager.OnNetworkResult += OnNetworkResultHandler;
		}

		serverRequestsManager = ComponentFactory.GetAComponent<IServerRequests>();

//		GetInventoryFromServer(()=>
//		                       {
//
//		});
	}

	#endregion
	

	#region IInventory implementation

	public event InventoryUpdatedDelegate OnInventoryUpdate;
	
	public void Initialize(System.Action completionAction)
	{
		GetInventoryFromServer(()=>
       		                       {
			if (completionAction != null)
			{
				completionAction();
			}
		});
	}

	public bool HasInventoryItem (string itemID)
	{
		foreach (InventoryItemDataModel inventoryItem in _inventoryItems)
		{
			if (inventoryItem.ItemType == itemID)
			{
				return true;
			}
		}
		return false;
	}

	public InventoryItemDataModel GetInventoryItem (string itemID)
	{
		foreach (InventoryItemDataModel inventoryItem in _inventoryItems)
		{
			if (inventoryItem.ItemType == itemID)
			{
				return inventoryItem;
			}
		}
		return null;
	}

	public bool IsTileUnlocked(int tileId)
	{

		foreach (InventoryItemDataModel inventoryItem in _inventoryItems)
		{
			if (inventoryItem.ItemType.Contains("."))
			{
				string[] itemFields = inventoryItem.ItemType.Split("."[0]);
				if (itemFields.Length == 1)
				{
					string portalId = ComponentFactory.GetAComponent<IPortal>().PortalIdentifier;
					if (portalId == itemFields[0])
					{
						return true;
					}
				}

				if (itemFields.Length > 1)
				{
					for (int i=0; i< itemFields.Length ; i++)
					{
						string item = itemFields[i];
						Debug.Log("item: " + item + " tileId.ToString(): " + tileId.ToString());

						if (item == tileId.ToString())
						{
							return true;
						}
					}
				}

			}
		}
		return false;
	}

	public int CoinsCount 
	{
		get
		{
			return GetGoinsCount();
		}
	}

	public void ConsumeFromInventory(InventoryItemDataModel inventoryItem, int amount, System.Action <bool> completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_CONSUME_FROM_INVENTORY;
		Hashtable data = new Hashtable();

		data.Add(ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_CONSUME_ITEM_TYPE, inventoryItem.ItemType);
		data.Add(ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_CONSUME_ITEM_AMOUNT, amount);

		serverRequestsManager.SendServerRequest(command, data, (response)=>
		                                        {

			if (completionAction != null)
			{
				completionAction(true);
			}

		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);
		});
	}

	public void AddInventoryItem(InventoryItemDataModel inventoryItem, System.Action <bool> completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_ADD_INVENTORY_ITEM;
		Hashtable data = new Hashtable();
		
		data.Add(ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_ADD_ITEM_TYPE, inventoryItem.ItemType);
		data.Add(ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_ADD_ITEM_AMOUNT, inventoryItem.Amount);
		
		serverRequestsManager.SendServerRequest(command, data, (response)=>
		                                        {
			
			if (completionAction != null)
			{
				completionAction(true);
			}

		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);
		});
	}

	public void AddToCoinsCount(int amount, bool publishInventoryUpdate, bool notifyServer, System.Action <bool> completionAction = null)
	{
		Debug.Log("add to coins: " + amount);
		InventoryItemDataModel inventoryItem = GetInventoryItem(_coinsItemType);
		if (inventoryItem != null)
		{
			if (notifyServer)
			{
				ConsumeFromInventory(inventoryItem, -amount, completionAction);

				if (completionAction != null)
				{
					completionAction(true);
				}
			}
			else
			{
				int deltaAmount = 0;
				if (PlayerPrefsUtil.HasKey(COINS_CHANGE_DELTA))
				{
					deltaAmount = PlayerPrefsUtil.GetInt(COINS_CHANGE_DELTA);
					Debug.Log("has delta amount: " + deltaAmount);
				}
				deltaAmount += amount;
				PlayerPrefsUtil.SetInt(COINS_CHANGE_DELTA, deltaAmount);

				Debug.Log("updated delta coins:" + deltaAmount);

				inventoryItem.Amount += amount;
			}

			if (publishInventoryUpdate)
			{
				if (OnInventoryUpdate != null)
				{
					OnInventoryUpdate(this);
				}
			}
		}
	}

	public void PublishCoinsDeltaChange(System.Action <bool> completionAction = null)
	{
		Debug.Log("PublishCoinsDeltaChange");
		if (_inventoryItems == null)
		{
			if (completionAction != null)
			{
				Debug.Log("Inventory Manager is not yet initialized... cancling");
				completionAction(false);
			}
			return;
		}

		if (_isUpdatingDelta)
		{
			if (completionAction != null)
			{
				Debug.Log("Delta update already in process, please wait");
				completionAction(false);
			}
			return;
		}

		int deltaAmount = 0;
		if (PlayerPrefsUtil.HasKey(COINS_CHANGE_DELTA))
		{
			deltaAmount = PlayerPrefsUtil.GetInt(COINS_CHANGE_DELTA);
		}

		if (deltaAmount != 0)
		{
			Debug.Log("has coins delta: " + deltaAmount);

			InventoryItemDataModel inventoryItem = GetInventoryItem(_coinsItemType);
			if (inventoryItem != null)
			{
				_isUpdatingDelta = true;
				ConsumeFromInventory(inventoryItem, -deltaAmount, (sucsess)=>
				                     {
					PlayerPrefsUtil.Delete(COINS_CHANGE_DELTA);
					_isUpdatingDelta = false;
					if (completionAction != null)
					{
						completionAction(sucsess);
					}
				});
			}
			else
			{
				Debug.LogError("ERROR - Trying to update delta count but the coins inventory item is missing!");
			}

		}
		else
		{
			if (completionAction != null)
			{
				completionAction(false);
			}
		}
	}

	public void RefreshInventory(bool checkForInventoryAdditions = false, System.Action completionAction = null)
	{
		_checkForInventoryAdditions = checkForInventoryAdditions;
		GetInventoryFromServer(()=>
		                       {
			if (completionAction != null)
			{
				completionAction();
			}
		});
	}

	#endregion


	#region Private

	private void GetInventoryFromServer(System.Action completionAction)
	{
		string command = ServerCommands.SERVER_COMMAND_GET_INVENTORY;
		Hashtable data = new Hashtable();
		
		serverRequestsManager.SendServerRequest(command, data, (response)=>
		                                  {
			
			if (completionAction != null)
			{
				completionAction();
			}

		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);

			if (completionAction != null)
			{
				completionAction();
			}
			
		});
	}

	private int GetGoinsCount()
	{
		InventoryItemDataModel coinInventoryItem = GetInventoryItem(_coinsItemType);
		if (coinInventoryItem != null)
		{
			return coinInventoryItem.Amount;
		}
		return 0;
	}

	#endregion


	#region Event Handling

	private void OnNetworkResultHandler(Hashtable result)
	{
		string pductRecivedType = "";
		int productRecivedAmount = 0;
		if (_auteUpdate && result.ContainsKey(_inventoryResultKey))
		{
			ArrayList inventoryItemsArray = result[ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_ITEM] as ArrayList;
			foreach (Hashtable inventoryItemData in inventoryItemsArray)
			{
				InventoryItemDataModel newInventoryitem = new InventoryItemDataModel(inventoryItemData);
				bool existsInInventory = false;
				foreach (InventoryItemDataModel inventoryitem in _inventoryItems)
				{
					if (inventoryitem.ItemType == newInventoryitem.ItemType)
					{
						if (inventoryitem.Amount < newInventoryitem.Amount && _checkForInventoryAdditions)
						{
							pductRecivedType = inventoryitem.ItemType;
							productRecivedAmount = newInventoryitem.Amount - inventoryitem.Amount;
						}
						inventoryitem.Amount = newInventoryitem.Amount;
						existsInInventory = true;
						break;
					}
				}
				if (!existsInInventory)
				{
					if (_inventoryItems == null)
					{
						_inventoryItems = new List<InventoryItemDataModel>();
					}
					_inventoryItems.Add(newInventoryitem);

					if (_checkForInventoryAdditions)
					{
						pductRecivedType = newInventoryitem.ItemType;
						productRecivedAmount = newInventoryitem.Amount;
					}
				}
			}
			_checkForInventoryAdditions = false;
			PublishCoinsDeltaChange();

			if (OnInventoryUpdate != null)
			{
				OnInventoryUpdate(this);
			}
		}

		if (_displayAquirePopup)
		{
			if (result.ContainsKey(_productReceivedKey))
			{
				Hashtable productRecivedData = result[_productReceivedKey] as Hashtable;
				if (productRecivedData != null)
				{
					pductRecivedType = productRecivedData[ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_PRODUCT_RECIVED_TYPE].ToString();;
					productRecivedAmount = int.Parse(productRecivedData[ServerRequestKeys.SERVER_RESPONSE_KEY_INVENTORY_PRODUCT_RECIVED_AMOUNT].ToString());
				}
			}

			if (!string.IsNullOrEmpty(pductRecivedType))
			{
				IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
				if (popupsManager != null)
				{
					ItemAquiredPopupController itemAquirePopup = popupsManager.DisplayPopup<ItemAquiredPopupController>();
					itemAquirePopup.DisplayUnlockForItem(pductRecivedType, productRecivedAmount);
				}
			}
		}

	}

	#endregion
}
