using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopScreenItemDisplayController : MonoBehaviour {

    #region Private proeprties

    [SerializeField]
    private Image _itemImage;

    [SerializeField]
    private Text _titleLabel;

    [SerializeField]
    private Text _infoLabel;

    [SerializeField]
    private Text _priceLabel;

    [SerializeField]
    private GameObject _purchaseButton;

    [SerializeField]
    private ShopItem _shopItem;

    #endregion


    #region Initialization

    void Start()
    {
        InventoryManager.Instance.OnInventoryUpdated += OnInventoryUpdatedHandler;
    }

    void OnDisable()
    {
        InventoryManager.Instance.OnInventoryUpdated -= OnInventoryUpdatedHandler;
    }

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
                _titleLabel.text = chromieDefenition.ChromieName;

                _priceLabel.text = shopItem.Price.Amount.ToString();

                if (shopItem.IsUnique && InventoryManager.Instance.HasItem(shopItem.Item.ID))
                {
                    _purchaseButton.SetActive(false);
                }
                else
                {
                    _purchaseButton.SetActive(true);
                }
                return;
            }
        }
    }


    public void PurchaseButtonAction()
    {
        ShopManager.Instance.Purchase(_shopItem, (sucsess) =>
        {

        });
    }

    #endregion


    #region Events

    private void OnInventoryUpdatedHandler()
    {
        SetShopitem(_shopItem);
    }

    #endregion
}
