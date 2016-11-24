using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurrencyShopPopupController : PopupBaseController
{

    #region Private Properties

    [SerializeField]
    private CurrencyShopItemCellController _shopItemCellPrefab;

    [SerializeField]
    private Transform _shopItemsListContent;

    [SerializeField]
    private bool _closeOnPurchase = true;

    #endregion

    void Start()
    {
        if (_shopItemCellPrefab != null)
        {
            _shopItemCellPrefab.gameObject.SetActive(false);
        }
        PopulateShopItemsList();
    }


    private void PopulateShopItemsList()
    {
        List<ShopItem> shopItems = ShopManager.Instance.GetShopitems();

        for (int i = 0; i < _shopItemsListContent.childCount; i++)
        {
            if (!_shopItemsListContent.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(_shopItemsListContent.GetChild(i));
            }
        }

        foreach (ShopItem shopitm in shopItems)
        {
            CurrencyShopItemCellController shopItemCell = Instantiate(_shopItemCellPrefab) as CurrencyShopItemCellController;
            shopItemCell.gameObject.SetActive(true);
            shopItemCell.gameObject.transform.SetParent(_shopItemsListContent);
            shopItemCell.gameObject.transform.localScale = Vector3.one;
            shopItemCell.SetShopitem(shopitm);
            shopItemCell.OnCurrencyShopItemCellPurchace += OnCurrencyShopItemCellPurchaceHandler;
        }
    }


    #region Events

    private void OnCurrencyShopItemCellPurchaceHandler(ShopItem shopitem)
    {
        ShopManager.Instance.Purchase(shopitem, (sucsess) =>
        {
            if (sucsess && _closeOnPurchase)
            {
                ClosePopup();
            }
        });
    }

    #endregion


}
