using UnityEngine;
using System.Collections;

public class GameplayWrapper  {


	public static void GameOver(int score = 0)
	{
		//Application.LoadLevel("Game Over Scene");

		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;
		if (popupsManager != null && portalManager != null)
		{
			BaseGameOverPopupController gameOverController = null;
			switch (portalManager.GetPortalType)
			{
			case PortalType.Endless:
			{
				gameOverController = popupsManager.DisplayPopup<EndlessGameOverPopupController>();
				break;
			}
			case PortalType.Saga:
			{
				gameOverController = popupsManager.DisplayPopup<SagaGameOverPopupController>();
				break;
			}
			}
			gameOverController.SetGameScore(null, score);
		}
	}

	public static int GetCoinsCount()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			int coinsCount = inventoryManager.CoinsCount; 
			return coinsCount;
		}
		return 0;
	}


	public static bool CanPayCoins(int coinsAmount)
	{
		// TODO - this is a fix to enable showing of the "not enough coins" popup

		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			int coinsCount = inventoryManager.CoinsCount; 
			return (coinsCount >= coinsAmount);
		}

		return false;

	}

	public static bool PayCoins(int coinsAmount)
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			int coinsCount = inventoryManager.CoinsCount; 
			bool hasEnoughCoins = (coinsCount >= coinsAmount);

			if (hasEnoughCoins)
			{
				inventoryManager.AddToCoinsCount(-coinsAmount, true, false);
				return true;
			}
			else
			{
				IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
				if (popupsManager != null)
				{
					GameplayWrapper.DisplayNotEnoughCoinsPopup(coinsAmount);
				}
			}
		}
		return false;
	}

	public static void AddCoins(int coinsAmount)
	{
		if (coinsAmount != 0)
		{
			IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
			if (inventoryManager != null)
			{
				inventoryManager.AddToCoinsCount(coinsAmount, true, false);
			}
		}
	}

	public static void CommitCoinsChange()
	{
		IInventory inventoryManager = ComponentFactory.GetAComponent<IInventory>();
		if (inventoryManager != null)
		{
			inventoryManager.PublishCoinsDeltaChange((sucsess)=>
			                                         {

			});
		}
	}

	public static void DisplayNotEnoughCoinsPopup(int coinsAmount)
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			popupsManager.DisplayPopup<NotEnoughCurrencyPopupController>(null);

		}
	}
}
