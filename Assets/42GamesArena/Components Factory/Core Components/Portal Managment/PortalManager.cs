using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(IServerRequests))]
public class PortalManager : MonoBehaviour, IPortal {

	#region Private Properties

	private const string PORTAL_DATA_CACHE_KEY = "portalData";

	private IServerRequests _serverRequestsManager;

	[SerializeField]
	private string _portalIdentifier;

	[SerializeField]
	private bool _isTurnementEnabled;

	[SerializeField]
	private PortalType _portalType;

	[SerializeField]
	private bool _useEditorData;

	[SerializeField]
	private LobbyDataModel _lobbyData;

	#endregion


	#region IPortal implementation

	public event LobbyUpdatedDelegate OnLobbyUpdated;

	public string PortalIdentifier {
		get {
			return _portalIdentifier;
		}
	}

	public bool IsTurnementEnabled { 
		get
		{
			return _isTurnementEnabled;
		}
	}

	public PortalType GetPortalType
	{
		get
		{
			return _portalType;
		}
	}

	public string TermsOfUseURL
	{
		get
		{
			return _lobbyData.TermsOfUseURL;
		}
	}


	public void LoadPortalConfiguration(System.Action <LobbyDataModel> completionAction)
	{
		if (_useEditorData)
		{
			_lobbyData.SetLocalTileList();
			completionAction(_lobbyData);
			return;
		}

		if (_serverRequestsManager == null)
		{
			_serverRequestsManager = ComponentFactory.GetAComponent<IServerRequests>();
		}

		string command = ServerCommands.SERVER_COMMAND_GET_LOBBY;
		Hashtable data = new Hashtable();
		
		data.Add(ServerRequestKeys.SERVER_KEY_PORTAL_KEY, _portalIdentifier);

		_serverRequestsManager.SendServerRequest(command, data, (response)=>
		{

			Hashtable portalData = response[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY] as Hashtable;
			
			_lobbyData = new LobbyDataModel();
			_lobbyData.LobbyName = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY_TITLE].ToString();
			//_lobbyData.Version = float.Parse(portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_VERSION].ToString());
			if (portalData.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_TERMS_OF_USE_URL))
			{
				_lobbyData.TermsOfUseURL = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_TERMS_OF_USE_URL].ToString();
			}


			ArrayList tiles = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_TILES] as ArrayList;
			
			List<TileDataModelAbstract> tilesList = new List<TileDataModelAbstract>();
			foreach (Hashtable tileData in tiles)
			{
				eTileType tileType = (eTileType)System.Enum.Parse(typeof(eTileType), tileData[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_TYPE].ToString());

				TileDataModelAbstract tile = null;
				switch (tileType)
				{
				case eTileType.GAME:
				{
					tile = new GameTileDataModel();
					break;
				}
				case eTileType.AD:
				{
					tile = new AdTileDataModel();
					break;
				}
				default:
				{
					Debug.LogError("ERROR - Unsupported tile type: " + tileData[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_TYPE].ToString());
					break;
				}
				}

				tile.Decode(tileData);

				tilesList.Add(tile);
			}
			
			_lobbyData.LobbyTilesList = tilesList;


			if (portalData.ContainsKey(ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY_SHOP_ITEMS))
			{
				ArrayList lobbyShopItems = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY_SHOP_ITEMS] as ArrayList;
				List<ShopItemDataModel> shopItemsList = new List<ShopItemDataModel>();
				foreach (Hashtable shopItemData in lobbyShopItems)
				{
					ShopItemDataModel shopItem = new ShopItemDataModel(shopItemData);
					shopItemsList.Add(shopItem);
				}

				_lobbyData.LobbyShopItems = shopItemsList;

			}

			if (OnLobbyUpdated != null)
			{
				OnLobbyUpdated(_lobbyData);
			}

			CacheUtil.SaveToCacheAsync(_lobbyData, PORTAL_DATA_CACHE_KEY, 0, ()=>
			                           {
				Debug.Log("saved portal data");
			});
			
			if (completionAction != null)
			{
				completionAction(_lobbyData);
			}

		},  (error) =>
		{
			Debug.LogError(error.ErrorDescription);
			
			CacheUtil.LoadFromCacheAsync(PORTAL_DATA_CACHE_KEY, (portalObject)=>
			                             {
				_lobbyData = (LobbyDataModel)portalObject;
				completionAction(_lobbyData);
			});
		});
	}

	// Depricated
	/*
	public void LoadPortalConfiguration(System.Action <PortalDataModel> completionAction)
	{
		if (_useEditorData)
		{
			completionAction(_portalData);
			return;
		}

		string command = ServerCommands.SERVER_COMMAND_GET_PORTAL;
		Hashtable data = new Hashtable();
		
		data.Add(ServerRequestKeys.SERVER_KEY_ID, _portalIdentifier);

		string osType = "";
#if UNITY_IOS
		osType = ServerRequestKeys.SERVER_KEY_OS_TYPE_IOS;

#elif UNITY_ANDROID
		osType = ServerRequestKeys.SERVER_KEY_OS_TYPE_ANDROID;
#endif
		data.Add(ServerRequestKeys.SERVER_KEY_OS_TYPE, osType);

		data.Add(ServerRequestKeys.SERVER_KEY_ISO_CODE, "");

		IServerRequests requestsManager = ComponentFactory.GetAComponent<IServerRequests>() as IServerRequests;

		requestsManager.SendServerRequest(command, data, (response)=>
		                                 {

			Hashtable portalData = response[ServerRequestKeys.SERVER_RESPONSE_KEY_PORTAL] as Hashtable;

			_portalData = new PortalDataModel();
			_portalData.PortalName = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_LOBBY_TITLE].ToString();
			_portalData.Version = float.Parse(portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_VERSION].ToString());

			ArrayList tiles = portalData[ServerRequestKeys.SERVER_RESPONSE_KEY_TILES] as ArrayList;

			List<GameTileDataModel> tilesList = new List<GameTileDataModel>();
			foreach (Hashtable tileData in tiles)
			{
				GameTileDataModel tile = new GameTileDataModel(tileData);
				tilesList.Add(tile);
			}

			_portalData.LobbyTilesList = tilesList;

			CacheUtil.SaveToCacheAsync(_portalData, PORTAL_DATA_CACHE_KEY, 0, ()=>
			                           {
				Debug.Log("saved portal data");
			});

			if (completionAction != null)
			{
				completionAction(_portalData);
			}

		}, (error) =>
		{
			Debug.LogError(error.ErrorDescription);

			CacheUtil.LoadFromCacheAsync(PORTAL_DATA_CACHE_KEY, (portalObject)=>
			                             {
				_portalData = (PortalDataModel)portalObject;
				completionAction(_portalData);
			});
		});
	}
	*/


	public List<TileDataModelAbstract> LobbyTilesList 
	{ 
		get
		{
			return _lobbyData.LobbyTilesList;
		}
	}

	public TileDataModelAbstract GetTileForKey(string key)
	{
		if (key.Contains("."))
		{
			string[] fields = key.Split("."[0]);
			int tileId = int.Parse(fields[1]);

			foreach (TileDataModelAbstract tile in _lobbyData.LobbyTilesList)
			{
				if (tile.TileID == tileId)
				{
					return tile;
				}
			}
		}
		else
		{
			Debug.Log("ERROR " + key + " is not a valid tile key!");
		}
		return null;
	}

	#endregion


	void Start()
	{
	
	}



}
