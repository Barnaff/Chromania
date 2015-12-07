using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BaseShopPopupController : PopupBaseController {

	#region Protected

	[SerializeField]
	protected StoreType _storeType;

	#endregion


	#region Private Properties

	[SerializeField]
	private GameObject _itemsContainer;

	[SerializeField]
	private BaseShopCellController[] _cellsPrefabs;

	[SerializeField]
	private ePriceAction[] _sortingOrder;

	private IShop _shopManager;

	private List<ShopItemDataModel> _shopItems = new List<ShopItemDataModel>();

	#endregion

	// Use this for initialization
	void Awake () {

		_shopManager = ComponentFactory.GetAComponent<IShop>() as IShop;

		_shopItems = _shopManager.GetShopItems(_storeType);
		SortShopItems();

		//TODO: Kobi  - this is a fix, need to find a way to refresh the list before the popup is shown!
		BuildShopList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void PopupDidShow ()
	{
		base.PopupDidShow ();

	}

	public override void PopupWillShow ()
	{
		base.PopupWillShow ();

	}

	private void BuildShopList()
	{
		ClearItems();

		for (int i=0; i< _shopItems.Count; i++)
		{
			BaseShopCellController cellController = CreateCellForShopItem(_shopItems[i]);
			if (cellController != null)
			{
				cellController.gameObject.SetActive(true);
				cellController.transform.SetParent(_itemsContainer.transform);
				cellController.transform.localScale = new Vector3(1,1,1);
			}
		}
	}

	private void ClearItems()
	{
		for (int i=0; i< _itemsContainer.transform.childCount; i++)
		{
			_itemsContainer.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	private BaseShopCellController CreateCellForShopItem(ShopItemDataModel shopItem)
	{
		BaseShopCellController newCell = null;
		foreach (BaseShopCellController cell in _cellsPrefabs)
		{
			if (cell.PriceAction == shopItem.Price.ActionType)
			{
				GameObject cellObject = Instantiate(cell.gameObject) as GameObject;
				newCell = cellObject.GetComponent<BaseShopCellController>();
				break;
			}
		}
		if (newCell == null)
		{
			Debug.LogError("ERROR - Missing cell for " + shopItem.Price.ActionType);
			return null;
		}

		newCell.SetShopItem(shopItem);
		newCell.OnShopItemPurchaseAction += HandleShopItemPurchaseAction;

		return newCell;
	}

	private void HandleShopItemPurchaseAction(ShopItemDataModel shopItem)
	{
		_shopManager.PurchaseItem(shopItem, (sucess)=>
		{
			if (sucess)
			{
				ClosePopup();
			}
			
		}, (error)=>
		{
			Debug.Log(error.ErrorDescription);
		});
	}

	private void SortShopItems()
	{
		List<ShopItemDataModel> shopItemTempList = new List<ShopItemDataModel>();

		for (int i = 0; i < _sortingOrder.Length ; i++)
		{
			foreach (ShopItemDataModel shopItem in _shopItems)
			{
				if (shopItem.Price.ActionType == _sortingOrder[i])
				{
					shopItemTempList.Add(shopItem);
				}	
			}
		}

		if (shopItemTempList.Count > 0)
		{
			_shopItems = shopItemTempList;
		}
	}
}
