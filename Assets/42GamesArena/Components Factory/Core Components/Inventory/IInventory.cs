using UnityEngine;
using System.Collections;

public delegate void InventoryUpdatedDelegate(IInventory inventoryManager);

public interface IInventory  
{
	event InventoryUpdatedDelegate OnInventoryUpdate;

	void Initialize(System.Action completionAction);

	bool HasInventoryItem(string itemID);

	bool IsTileUnlocked(int tileId);

	InventoryItemDataModel GetInventoryItem(string itemID);
	
	int CoinsCount { get; }	

	void ConsumeFromInventory(InventoryItemDataModel inventoryItem, int amount, System.Action <bool> completionAction);

	void AddInventoryItem(InventoryItemDataModel inventoryItem, System.Action <bool> completionAction);

	void AddToCoinsCount(int amount, bool publishInventoryUpdate, bool notifyServer, System.Action <bool> completionAction = null);

	void PublishCoinsDeltaChange(System.Action <bool> completionAction = null);

	void RefreshInventory(bool checkForInventoryAdditions = false, System.Action completionAction = null);
	

}
