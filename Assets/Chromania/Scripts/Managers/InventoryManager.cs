using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : Kobapps.Singleton<InventoryManager> {

    #region Private Properties

    [SerializeField]
    public List<Inventoryitem> _inventoryItemList;

    [SerializeField]
    private Inventoryitem _currencyInventoryItem;

    private const string STORED_INVENTORY_KEY = "storedInventory";
    private const string CURRENCTY_ITEM_ID = "currency";

    #endregion


    #region Initialization

    void Awake()
    {
        Load();
    }

    #endregion

    #region Public

    public void AddInventoryItem(Inventoryitem inventoryItem)
    {
        Inventoryitem oldInventoryitem = GetInventoryItem(inventoryItem.ID);
        if (oldInventoryitem != null)
        {
            oldInventoryitem.Amount += inventoryItem.Amount;
        }
        else
        {
            _inventoryItemList.Add(inventoryItem);
        }
        Save();
    }

    public int AmountForItem(string itemId)
    {
        Inventoryitem inventoryItem = GetInventoryItem(itemId);
        if (inventoryItem != null)
        {
            return inventoryItem.Amount;
        }
        return 0;
    }

    public Inventoryitem GetInventoryItem(string itemId)
    {
        foreach (Inventoryitem inventoryItem in _inventoryItemList)
        {
            if (inventoryItem.ID == itemId)
            {
                return inventoryItem;
            }
        }
        return null;
    }

    public int Currency
    {
        get
        {
            if (_currencyInventoryItem != null)
            {
                return _currencyInventoryItem.Amount;
            }
            return 0;
        }
        set
        {
            if (_currencyInventoryItem != null)
            {
                _currencyInventoryItem.Amount = value;
                Save();
            }
        }
    }

    #endregion


    #region Private

    private void Load()
    {
        if (PlayerPrefsUtil.HasKey(STORED_INVENTORY_KEY))
        {
            _inventoryItemList = (List<Inventoryitem>)PlayerPrefsUtil.GetObject(STORED_INVENTORY_KEY);
        }

        if (_inventoryItemList != null)
        {
            _currencyInventoryItem = GetInventoryItem(CURRENCTY_ITEM_ID);
        }
        if (_currencyInventoryItem == null)
        {
            AddInventoryitem(_currencyInventoryItem = new Inventoryitem(CURRENCTY_ITEM_ID, CURRENCTY_ITEM_ID, 0));
        }
    }

    private void Save()
    {
        if (_inventoryItemList != null)
        {
            Debug.Log("Saved Inventory");
            PlayerPrefsUtil.SetObject(STORED_INVENTORY_KEY, _inventoryItemList);
        }
    }

    private void AddInventoryitem(Inventoryitem inventoryItem)
    {
        if (_inventoryItemList == null)
        {
            _inventoryItemList = new List<Inventoryitem>();
        }
        _inventoryItemList.Add(inventoryItem);
    }

    #endregion
}

[System.Serializable]
public class Inventoryitem
{
    public string ID;

    public string Name;

    public int Amount;

    public eChromieType ChromieType;

    public Inventoryitem(string id, string name, int amount)
    {
        ID = id;
        Name = name;
        Amount = amount;
    }
}

