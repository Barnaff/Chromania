using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrencyShopItemCellController : MonoBehaviour {

    #region Public Properties

    public delegate void CurrencyShopItemCellPurchaceDelegate(ShopItem shopItem);

    public event CurrencyShopItemCellPurchaceDelegate OnCurrencyShopItemCellPurchace;

    #endregion

    #region Private Properties

    [SerializeField]
    private Text _ItemTitleLabel;

    [SerializeField]
    private Text _purchaseButtonLabel;

    [SerializeField]
    private ShopItem _shopItem;

    #endregion


    #region Public

    public void SetShopitem(ShopItem shopItem)
    {
        _shopItem = shopItem;

        if (_ItemTitleLabel != null)
        {
            _ItemTitleLabel.text = shopItem.Name;
        }

        if (_purchaseButtonLabel != null)
        {
            _purchaseButtonLabel.text = shopItem.Price.Amount + "$";
        }
    }

    public void BuyButtonAction()
    {
        if (OnCurrencyShopItemCellPurchace != null)
        {
            OnCurrencyShopItemCellPurchace(_shopItem);
        }
    }

    #endregion
}
