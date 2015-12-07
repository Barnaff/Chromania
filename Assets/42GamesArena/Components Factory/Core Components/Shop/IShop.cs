using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface IShop  {

	void Initialize(System.Action completionAction);

	List<ShopItemDataModel> GetShopItems(StoreType category);

	void PurchaseItem(ShopItemDataModel shopItem, System.Action <bool> completionAction, System.Action <ErrorObj> failAction);

	bool CanBuyItem(ShopItemDataModel shopItem);

}
