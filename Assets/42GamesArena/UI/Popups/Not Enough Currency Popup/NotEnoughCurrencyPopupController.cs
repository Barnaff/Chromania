using UnityEngine;
using System.Collections;

public class NotEnoughCurrencyPopupController : PopupBaseController {

	#region Private Properties

	//[SerializeField]
	//private ShopItemDataModel _shopItem;

	#endregion


	#region Public

	public void SetShopItem(ShopItemDataModel shopItem)
	{
		//_shopItem = shopItem;
	}

	#endregion


	#region User Interaction

	public void OpenShopButtonAction()
	{
		IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
		if (popupsManager != null)
		{
			CoinsShopPopupController coinsShopPopup = popupsManager.DisplayPopup<CoinsShopPopupController>();
			coinsShopPopup.PopupClosedAction = ()=>
			{
				ClosePopup();
			};
		}
	}

	#endregion
}
