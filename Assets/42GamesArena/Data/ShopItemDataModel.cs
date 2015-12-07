using UnityEngine;
using System.Collections;


public enum ShopCategoryType
{
	All,
	Hearts,
	Coins,
	Games,
}

[System.Serializable]
public class ShopItemDataModel  {

	[SerializeField]
	public int ItemID;

	[SerializeField]
	public string ItemName;

	[SerializeField]
	public PriceDataModel Price;

	[SerializeField]
	public ProductDataModel Product;

	[SerializeField]
	public StoreType Store;

	[SerializeField]
	public string IconURL;
	

	public ShopItemDataModel(Hashtable data)
	{
		ItemID = int.Parse(data[ServerRequestKeys.SERVER_RESPONSE_KEY_SHOP_ITEM_ID].ToString());
		Price = new PriceDataModel((Hashtable)data[ServerRequestKeys.SERVER_RESPONSE_KEY_SHOP_ITEM_PRICE]);

		if (data.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_SHOP_ITEM_PRODUCT))
		{
			if ((Hashtable)data[ServerRequestKeys.SERVER_RESPONSE_KEY_SHOP_ITEM_PRODUCT] != null)
			{
				Product = new ProductDataModel((Hashtable)data[ServerRequestKeys.SERVER_RESPONSE_KEY_SHOP_ITEM_PRODUCT]);
			}

		}


	}

}
