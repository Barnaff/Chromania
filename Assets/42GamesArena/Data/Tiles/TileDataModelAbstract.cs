using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eTileType
{
	GAME,
	AD,
}

[System.Serializable]
public abstract class TileDataModelAbstract  {

	[SerializeField]
	public int TileID;

	[SerializeField]
	public eTileType TileType; 

	[SerializeField]
	public int TileOrder;

	[SerializeField]
	public eTileSize TileSize;

	[SerializeField]
	public List<ShopItemDataModel> ShopItems;

	[SerializeField]
	public bool IsEnabled;


	public virtual TileDataModelAbstract Decode(Hashtable data)
	{
		TileType = (eTileType)System.Enum.Parse(typeof(eTileType), data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_TYPE].ToString());
		TileID = int.Parse( data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_ID].ToString());
		TileSize = (eTileSize)System.Enum.Parse(typeof(eTileSize), data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_SIZE].ToString());
		TileOrder = int.Parse( data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_ORDER].ToString());

		IsEnabled = true;

		ArrayList shopItems = (ArrayList)data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_SHOP_ITEMS];
		if (shopItems != null && shopItems.Count > 0)
		{
			ShopItems = new List<ShopItemDataModel>();
			foreach (Hashtable shopItemData in shopItems)
			{
				ShopItemDataModel shopItem = new ShopItemDataModel(shopItemData);
				ShopItems.Add(shopItem);
			}
		}
		return this;
	}
}
