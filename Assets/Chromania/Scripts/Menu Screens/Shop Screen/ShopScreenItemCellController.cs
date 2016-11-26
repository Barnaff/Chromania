using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopScreenItemCellController : MonoBehaviour {

    #region Public

    public delegate void ShopitemSelectedDelegate(ShopItem shopItem);

    public event ShopitemSelectedDelegate OnShopItemSelected;

    #endregion

    #region Private Properties

    [SerializeField]
    private Image _itemImage;

    [SerializeField]
    private ShopItem _shopItem;

    #endregion


    #region Public

    public void SetShopitem(ShopItem shopItem)
    {
        _shopItem = shopItem;
        if (_shopItem.Item.ChromieType != eChromieType.None)
        {
            ChromieDefenition chromieDefenition = ChromezData.Instance.GetChromie(_shopItem.Item.ChromieType);
            if (chromieDefenition != null)
            {
                _itemImage.sprite = chromieDefenition.ChromieSprite;
            }
        }
    }

    public void ItemSelectedAction()
    {
        if (OnShopItemSelected != null)
        {
            OnShopItemSelected(_shopItem);
        }
    }

    #endregion
}
