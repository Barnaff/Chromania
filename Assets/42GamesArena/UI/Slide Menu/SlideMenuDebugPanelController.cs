using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideMenuDebugPanelController : MonoBehaviour {

	#region Private Properties

	[SerializeField]
	private Toggle _lockAllTilesToggle;

	[SerializeField]
	private Toggle _unlockAllTilesToggle;

	[SerializeField]
	private Toggle _toggleConsole;

	[SerializeField]
	private Toggle _toggleUpdateInventory;

	#endregion


	#region Initialization

	// Use this for initialization
	void Start () 
	{
		_lockAllTilesToggle.isOn = DebugManager.LockAllTiles;
		_unlockAllTilesToggle.isOn = DebugManager.UnLockAllTiles;

		Console console = FindObjectOfType<Console>() as Console;
		if (console != null)
		{
			_toggleConsole.isOn = console.EnableConsole;
			if (_toggleConsole.isOn)
			{
				console.EnableToggleButton = true;
			}
		}
	}

	#endregion


	#region User Interactions

	public void ToggleLockAllTiles(bool value)
	{
		DebugManager.LockAllTiles = _lockAllTilesToggle.isOn;
	}

	public void ToggleUnlockAllTiles(bool value)
	{
		DebugManager.UnLockAllTiles = _unlockAllTilesToggle.isOn;
	}

	public void ReloadPortal()
	{
		AppReloader.ReloadApp();
	}

	public void ToggleConsole(bool value)
	{
		Console console = FindObjectOfType<Console>() as Console;
		if (console != null)
		{
			console.EnableConsole = _toggleConsole.isOn;
			console.EnableToggleButton = _toggleConsole.isOn;
		}
	}

	public void AddCoins(int value)
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
		if (inventoryManager != null)
		{
			InventoryItemDataModel coinsItem = inventoryManager.GetInventoryItem("1");

			if (coinsItem != null)
			{
				if (_toggleUpdateInventory.isOn)
				{
					inventoryManager.ConsumeFromInventory(coinsItem, -value, (sucsess)=>
					                                      {

					});
				}
				else
				{
					coinsItem.Amount += value;
				}
			}
			else
			{
				coinsItem = new InventoryItemDataModel(null);
				coinsItem.ItemType = "1";
				coinsItem.Amount = value;
				if (coinsItem.Amount < 0)
				{
					coinsItem.Amount = 0;
				}

				if (_toggleUpdateInventory.isOn)
				{
					inventoryManager.AddInventoryItem(coinsItem, (sucsess)=>
					                                  {

					});
				}
				else
				{

				}
			}
		}
	}

	#endregion
}
