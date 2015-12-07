using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsShopPopupController : BaseShopPopupController {

	[SerializeField]
	private Text _coinsLabel;

	#region Initialization

	// Use this for initialization
	void Start () {
	
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			inventoryManager.OnInventoryUpdate += HandleOnInventoryUpdate;
			_coinsLabel.text = inventoryManager.CoinsCount.ToString();
		}
	}

	void OnDisable()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			inventoryManager.OnInventoryUpdate -= HandleOnInventoryUpdate;
		}
	}

	#endregion


	#region Events Hanlers

	void HandleOnInventoryUpdate (IInventory inventoryManager)
	{
		_coinsLabel.text = inventoryManager.CoinsCount.ToString();	
	}

	#endregion
}
