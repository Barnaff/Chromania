using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PortalType
{
	Endless,
	Saga,
}

[System.Serializable]
public class LobbyDataModel  {
	
	[SerializeField]
	public string LobbyName;

	[SerializeField]
	public string LobbyID;

	[SerializeField]
	public float Version;

	[SerializeField]
	public bool EnableHearts;

	[SerializeField]
	public bool EnableStars;

	[SerializeField]
	public string LocalCode;
	
	[SerializeField]
	public string AssetBundleKey;

	[SerializeField]
	public string LobbyTitle;
	
	[SerializeField]
	public string LobbyBundle;
	
	[SerializeField]
	public string LobbyScene;
	
	[SerializeField]
	public bool UseLobbyScene;

	[SerializeField]
	public string TermsOfUseURL;

	[SerializeField]
	public string PrivacyPolicyURL;
	
	[SerializeField]
	public List<TileDataModelAbstract> LobbyTilesList;

	[SerializeField]
	public List<ShopItemDataModel> LobbyShopItems;

	[SerializeField]
	private List<GameTileDataModel> _localGamesTiles;

	[SerializeField]
	private List<AdTileDataModel> _localAdTiles;


	public void SetLocalTileList()
	{
		LobbyTilesList = new List<TileDataModelAbstract>();
		foreach (GameTileDataModel tile in _localGamesTiles)
		{
			LobbyTilesList.Add(tile);
		}
		foreach (AdTileDataModel tile in _localAdTiles)
		{
			LobbyTilesList.Add(tile);
		}
	}

}
