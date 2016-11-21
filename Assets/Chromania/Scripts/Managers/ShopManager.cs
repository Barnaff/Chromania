using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : Kobapps.Singleton<ShopManager> {

    #region Private Properties

    [SerializeField]
    private List<ShopItem> _shopitems;

    #endregion

    #region Public

    public bool CanPurchase(ShopItem shopitem)
    {
        return true;
    }

    public void Purchase(ShopItem shopItem, System.Action<bool> completionAction)
    {

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

        }
    }

    #endregion


    #region private

    private void LoadSHopitems()
    {

    }

    #endregion
}

[System.Serializable]
public class ShopItem
{
    public string Identifier;

    public string Name;

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
