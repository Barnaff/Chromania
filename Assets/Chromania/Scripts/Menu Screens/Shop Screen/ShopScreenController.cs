using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopScreenController : MonoBehaviour {

    #region Private proeprties

    [SerializeField]
    private ShopScreenItemCellController _shopitemCellPrefab;

    [SerializeField]
    private Transform _shopitemListContent;

    [SerializeField]
    private ShopScreenItemDisplayController _itemDisplayController;

    private ShopItem _selectedShopitem;

    

    #endregion

    #region Initialization

    void Start () {
	
        if (_shopitemCellPrefab != null)
        {
            _shopitemCellPrefab.gameObject.SetActive(false);
        }

        PopulateShopitems();
    }

    #endregion


    #region Public

    public void BackButtonAction()
    {
        FlowManager.Instance.DisplayMainMenu();
    }

    #endregion


    #region Private

    private void PopulateShopitems()
    {
        for (int i=0; i < _shopitemListContent.childCount; i++)
        {
            if (_shopitemListContent.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(_shopitemListContent.GetChild(i).gameObject);
            }
        }

        List<ShopItem> shopitems = ShopManager.Instance.GetShopitems(ShopItem.eShopItemCategoty.Chromiez);
        foreach (ShopItem shopItem in shopitems)
        {
            ShopScreenItemCellController shopItemCell = Instantiate(_shopitemCellPrefab) as ShopScreenItemCellController;
            shopItemCell.gameObject.SetActive(true);
            shopItemCell.gameObject.transform.SetParent(_shopitemListContent);
            shopItemCell.gameObject.transform.localScale = Vector3.one;
            shopItemCell.SetShopitem(shopItem);
            shopItemCell.OnShopItemSelected += OnShopItemSelectedHandler;
        }

        if (_selectedShopitem == null)
        {
            SelectShopItem(shopitems[0]);
        }
    }

    private void SelectShopItem(ShopItem shopItem)
    {
        _selectedShopitem = shopItem;

        if (_itemDisplayController != null)
        {
            _itemDisplayController.SetShopitem(_selectedShopitem);
        }
    }

    #endregion


    #region Events

    private void OnShopItemSelectedHandler(ShopItem shopItem)
    {
        SelectShopItem(shopItem);
    }

    #endregion
}
