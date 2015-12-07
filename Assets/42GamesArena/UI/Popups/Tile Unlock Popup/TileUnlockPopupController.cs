using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TileUnlockPopupController : PopupBaseController {

	[SerializeField]
	private Transform _listContainer;

	[SerializeField]
	private Text _headerTitle;

	private TileDataModelAbstract _tile; 

	private List<TileUnlockCellController> _cellsPrefabs;

	#region Initialization

	public override void PopupWillShow ()
	{
		base.PopupWillShow ();
		_cellsPrefabs = new List<TileUnlockCellController>();
		for (int i=0; i< _listContainer.childCount; i++)
		{
			Transform child = _listContainer.GetChild(i);
			TileUnlockCellController tileUnlockCellController = child.gameObject.GetComponent<TileUnlockCellController>();
			if (tileUnlockCellController != null)
			{
				_cellsPrefabs.Add(tileUnlockCellController);
				tileUnlockCellController.gameObject.SetActive(false);
			}
		}
	}


	#endregion


	#region Public

	public void SetTile(TileDataModelAbstract tile)
	{
		_tile = tile;

		if (_tile is GameTileDataModel)
		{
			string gameName = ((GameTileDataModel)_tile).GameDefenition.GameName;

			_headerTitle.text = "Unlock " + gameName;
		}

		RefreshActionsCells();
	}

	#endregion


	#region Private

	private void RefreshActionsCells()
	{
		foreach (ShopItemDataModel shopItem in _tile.ShopItems)
		{
			TileUnlockCellController cellPrefab = GetCellPrefab(shopItem.Price.ActionType);
			if (cellPrefab != null)
			{
				TileUnlockCellController itemCell = Instantiate(cellPrefab) as TileUnlockCellController;
				itemCell.gameObject.SetActive(true);
				itemCell.transform.SetParent(_listContainer);
				itemCell.transform.localScale = new Vector3(1,1,1);
				
				itemCell.SetShopItem(shopItem);
				itemCell.OnUnlockAction += OnUnlockActionHandler;
			}


		}
	}

	private TileUnlockCellController GetCellPrefab(ePriceAction ActionType)
	{
		foreach (TileUnlockCellController cell in _cellsPrefabs)
		{
			if (cell.CellActionType == ActionType)
			{
				return cell;
			}
		}
		Debug.LogError("ERROR - Missing Cell for " + ActionType);
		return null;
	}

	#endregion


	#region Handle Events

	private void OnUnlockActionHandler(ShopItemDataModel shopItem)
	{
		IShop shopManager = ComponentFactory.GetAComponent<IShop>();
		if (shopManager != null)
		{
			shopManager.PurchaseItem(shopItem, (success)=>
			                         {

				if (success)
				{
					ClosePopup();
				}

			}, (error) =>
			{
				Debug.LogError(error.ErrorDescription);
			});
		}

	}

	#endregion
}
