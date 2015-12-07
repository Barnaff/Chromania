using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyListController : MonoBehaviour {

	#region Serialized Properties

	[SerializeField]
	private GridLayoutGroup _gridController;

	[SerializeField]
	private GameObject _rowPanelPrefab;

	[SerializeField]
	private GameObject _gameTilePrefab;

	[SerializeField]
	private int _cellInRow = 3;

	[SerializeField]
	private float _tileAspectRatio = 1.84f;

	[SerializeField]
	private GameObject _termsOfUseCell;

	private IInventory _inventoryManager;
	#endregion


	#region Private Properties

	private Vector2 _baseTileSize = Vector2.zero;
	private float _margins = 0.0f;

	#endregion

	// Use this for initialization
	void Start () 
	{
		_inventoryManager = ComponentFactory.GetAComponent<IInventory>() as IInventory;
		if (_inventoryManager != null)
		{
			_inventoryManager.OnInventoryUpdate += HandleOnInventoryUpdate;

			// update the server for any coins we kept from the game
			_inventoryManager.PublishCoinsDeltaChange();
		}
		_margins = Screen.width * 0.06f;
		_baseTileSize = new Vector2((Screen.width - _margins * 2.0f) / _cellInRow, (Screen.width - _margins * 2.0f) / _cellInRow);
		_gridController.cellSize = new Vector2(Screen.width - (_margins * 2.0f), _baseTileSize.y * _tileAspectRatio);
		_gridController.padding.bottom = (int)(Screen.height * 0.1f);
		_gridController.padding.left = (int)_margins;
		_gridController.padding.right = (int)_margins;
		_gridController.padding.top = (int)(Screen.height * 0.1f);
		PopuplateGameList();


		AnalyticsUtil.SendScreenHit(AnalyticsServiceType.GoogleAnalytics,AnalyticsEvents.ANALYTICS_SCREEN_LOBBY);

		IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
		if (backButtonManager != null)
		{
			backButtonManager.RegisterToBackButton(BackButtonAction);
		}

		IAds adsManager = ComponentFactory.GetAComponent<IAds>();
		if (adsManager != null)
		{
			adsManager.GetAdsListFromServer((adsList)=>
			                                {
				PopuplateGameList();
			});
		}
	}

	void OnDisable()
	{
		if (_inventoryManager != null)
		{
			_inventoryManager.OnInventoryUpdate -= HandleOnInventoryUpdate;
		}

		IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
		if (backButtonManager != null)
		{
			backButtonManager.RemoveResponderFromBackButton(BackButtonAction);
		}
	}

	private void BackButtonAction()
	{
		Debug.Log("Quit Application");
		Application.Quit();
	}

	
	#region Private

	private void PopuplateGameList()
	{
		IPortal portalManager = ComponentFactory.GetAComponent<IPortal>() as IPortal;

		List<TileDataModelAbstract> tilesList = portalManager.LobbyTilesList;

		List<EmptyTileHolder> emptyTiles = new List<EmptyTileHolder>();

		for (int i=0 ; i < _gridController.transform.childCount; i++)
		{
			Destroy(_gridController.transform.GetChild(i).gameObject);
		}

		if (tilesList != null)
		{
			IAds adsManager = ComponentFactory.GetAComponent<IAds>();

			foreach (TileDataModelAbstract tileData in tilesList)
			{
				bool displayTile = true;
				if (tileData.GetType() == typeof(AdTileDataModel))
				{
					AdTileDataModel adTileData = (AdTileDataModel)tileData;
					if (adsManager != null)
					{
						adTileData.Ad = adsManager.GetAdForPresentation(AdPresentationType.Tile);
					}
					if (adTileData.Ad == null)
					{
						displayTile = false;
					}
				}

				if (displayTile)
				{
					EmptyTileHolder emptyTileHolder = null;
					foreach (EmptyTileHolder emptyTile in emptyTiles)
					{
						if (emptyTile.Size >= (int)tileData.TileSize)
						{
							emptyTileHolder = emptyTile;
						}
					}
					
					if (emptyTileHolder == null)
					{
						emptyTileHolder = new EmptyTileHolder();
						emptyTileHolder.Size = _cellInRow;
						emptyTileHolder.Row = Instantiate(_rowPanelPrefab) as GameObject;
						emptyTileHolder.Row.transform.SetParent(_gridController.transform);
						emptyTiles.Add(emptyTileHolder);
					}
					
					
					GameObject cell = Instantiate(_gameTilePrefab) as GameObject;
					
					cell.transform.SetParent(emptyTileHolder.Row.transform);
					
					cell.GetComponent<LayoutElement>().preferredWidth = _baseTileSize.x * (float)tileData.TileSize;
					
					emptyTileHolder.Size -= (int)tileData.TileSize;
					if (emptyTileHolder.Size <= 0)
					{
						emptyTiles.Remove(emptyTileHolder);
					}
					
					LobbyTileController tileController = cell.GetComponent<LobbyTileController>() as LobbyTileController;
					if (tileController != null)
					{
						
						bool isUnlocked = IsTileUnlocked(tileData);
						
						tileController.SetTile(tileData, isUnlocked, HandleTileSelect);
					}
				}


			}
		}
		else
		{
			Debug.LogError("ERROR - Tile list is empty!");
		}

		if (_termsOfUseCell != null)
		{
			GameObject termsOfUseCell = Instantiate(_termsOfUseCell) as GameObject;
			if (termsOfUseCell != null)
			{
				termsOfUseCell.transform.SetParent(_gridController.transform);
			}
		}

	}


	IEnumerator LoadGame (GameDefenitionDataModel game) 
	{
		while (!AssetManager.instance.isReady)
		{
			Debug.LogError("assets manager is not ready!");
			yield return null;
		}
		
		AssetManager.instance.LoadBundle (game.GameBundleName);
		
		while (!AssetManager.instance.IsBundleLoaded(game.GameBundleName))
		{
			yield return null;
		}
		
		Application.LoadLevel (game.GameSceneName);
	}

	private bool IsTileUnlocked(TileDataModelAbstract tileData)
	{
		if (DebugManager.LockAllTiles)
		{
			return false;
		}
		if (DebugManager.UnLockAllTiles)
		{
			return true;
		}

		bool isUnlocked = true;

		if (tileData != null && tileData.ShopItems != null && tileData.ShopItems.Count > 0)
		{
			isUnlocked = _inventoryManager.IsTileUnlocked(tileData.TileID);
		}

		return isUnlocked;
	}

	#endregion


	#region Handle Events

	
	private void HandleOnInventoryUpdate (IInventory inventoryManager)
	{
		PopuplateGameList();
	}

	private void HandleTileSelect(LobbyTileController tileController)
	{
		
		TileDataModelAbstract tileData = tileController.TileData;
		
		bool isUnlocked = IsTileUnlocked(tileData);

		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics ,
		                           AnalyticsEvents.ANALYTICS_CATEGORY_LOBBY, 
		                           AnalyticsEvents.ANALYTICS_EVENT_TILE_CLICKED,
		                           tileData.TileType.ToString(),
		                           tileData.TileID);
		
		if (isUnlocked)
		{
			if (tileData is GameTileDataModel)
			{
				GameTileDataModel gameData = (GameTileDataModel)tileData;
				GameLoaderUtil.LoadGame(gameData.GameDefenition);
				
			}
			else if (tileData is AdTileDataModel)
			{
				IAds adsManager = ComponentFactory.GetAComponent<IAds>() as IAds;
				if (adsManager != null)
				{
					AdTileDataModel adTile = (AdTileDataModel)tileData;
					adsManager.ClickAdFromTile(tileController.Ad, adTile);
				}
				else
				{
					Debug.LogError("ERROR - Ads manager is not avalable!");
				}
			}
		}
		else
		{
			IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>() as IPopups;
			if (popupsManager != null)
			{
				TileUnlockPopupController tileUnlockPopupController = popupsManager.DisplayPopup<TileUnlockPopupController>();
				
				tileUnlockPopupController.SetTile(tileData);
			}
			Debug.Log("item is locked");	
		}



	}

	#endregion

}


// data object to temporary hold the empty 
// tiles slots when building the grid layout
public class EmptyTileHolder
{
	public GameObject Row;
	public int Size;
}
