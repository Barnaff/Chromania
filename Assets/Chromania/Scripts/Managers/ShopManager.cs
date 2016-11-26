using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : Kobapps.Singleton<ShopManager> {

    #region Private Properties

    [SerializeField]
    private List<ShopItem> _shopitems;

    #endregion


    #region Initialization

    void Awake()
    {
        LoadShopitems();
    }

    #endregion


    #region Public

    public bool CanPurchase(ShopItem shopitem)
    {
        return true;
    }

    public void Purchase(ShopItem shopItem, System.Action<bool> completionAction)
    {
        switch (shopItem.Price.Action)
        {
            case Price.ePriceAction.Coins:
                {
                    Pay(shopItem.Price.Amount, (sucsess) =>
                    {
                        if (sucsess)
                        {
                            AddShopitemToInventory(shopItem);
                        }
                        if (completionAction != null)
                        {
                            completionAction(sucsess);
                        }
                    });
                    break;
                }
            case Price.ePriceAction.IAP:
                {
                    AddShopitemToInventory(shopItem);
                    if (completionAction != null)
                    {
                        completionAction(true);
                    }
                    break;
                }
        }
    }

    public void Pay(int currencyAmount, System.Action<bool> completionAction)
    {
        if (InventoryManager.Instance.Currency >= currencyAmount)
        {
            InventoryManager.Instance.Currency -= currencyAmount;
            if (completionAction != null)
            {
                completionAction(true);
            }
        }
        else
        {
            // not enough currency
            Debug.Log("Not enough currency: " + currencyAmount);
            PopupsManager.Instance.DisplayPopup<NoEnoughCurrencyPopupController>();

            if (completionAction != null)
            {
                completionAction(false);
            }

        }
    }

    public List<ShopItem> GetShopitems(ShopItem.eShopItemCategoty category= ShopItem.eShopItemCategoty.All)
    {
        if (category == ShopItem.eShopItemCategoty.All)
        {
            return _shopitems;
        }

        List<ShopItem> shopitems = new List<ShopItem>();
        foreach (ShopItem shopItem in _shopitems)
        {
            if (shopItem.Category == category)
            {
                shopitems.Add(shopItem);
            }
        }
        return shopitems;
    }

    public ShopItem GetShopItemForChromie(eChromieType chromieType)
    {
        foreach (ShopItem shopItem in _shopitems)
        {
            if (shopItem.Item.ChromieType == chromieType)
            {
                return shopItem;
            }
        }
        return null;
    }

    #endregion


    #region private

    private void AddShopitemToInventory(ShopItem shopitem)
    {
        InventoryManager.Instance.AddInventoryItem(shopitem.Item);
    }

    private void LoadShopitems()
    {
        _shopitems = AppSettings.Instance.ShopItems;
    }

    #endregion
}

[System.Serializable]
public class ShopItem
{
    public string Identifier;

    public string Name;

    public string Description;

    public enum eShopItemCategoty
    {
        All,
        IAP,
        Chromiez,
    }

    public eShopItemCategoty Category;

    public bool IsUnique;

    public Price Price;

    public Inventoryitem Item;

}

[System.Serializable]
public class Price
{
    public enum ePriceAction
    {
        Coins,
        IAP,
    }

    public ePriceAction Action;

    public int Amount;
}
