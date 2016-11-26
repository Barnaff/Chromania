using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChromiezSelectionScreenController : MenuScreenBaseController {

    #region Private Properties

    [SerializeField]
    private Transform _inventoryListContent;

    [SerializeField]
    private GameObject _inventoryItemCellPrefab;

    [SerializeField]
    private List<ChromieSelectionSelectedCellController> _selctionsCellsList;

    [Header("Info Panel")]
    [SerializeField]
    private Text _chromieInfoLabel;

    [SerializeField]
    private GameObject _unlockButton;

    [SerializeField]
    private Text _unlockPriceLabel;

    private ChromieDefenition _currenctSelection;

    #endregion


    #region Initialization

    void Start()
    {
        InitScreen();
    }

    void OnEnable()
    {
        InventoryManager.Instance.OnInventoryUpdated += OnInventoryUpdateHandler;
    }

    void OnDisable()
    {
        InventoryManager.Instance.OnInventoryUpdated -= OnInventoryUpdateHandler;
    }

    #endregion


    #region User Interactions

    public void PlayButtonAction()
    {
        if (GameSetupManager.Instance.SelectedChromiez.Contains(null))
        {
            return;
        }
        FlowManager.Instance.StartGame();
    }

    public void BackButtonAction()
    {
        MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.ModeSelection);
    }

    #endregion


    #region Private

    private void InitScreen()
    {
        if (_unlockButton != null)
        {
            _unlockButton.SetActive(false);
        }
        _inventoryItemCellPrefab.SetActive(false);
        PopulateInventoryList();
        UpdateSelectionList();

        foreach (ChromieSelectionSelectedCellController cellController in _selctionsCellsList)
        {
            cellController.OnSelectionCellSelected += SelectionCellSelectedHandler;
        }
    }

    private void UpdateSelectionList()
    {
        for (int i=0; i< _selctionsCellsList.Count; i++)
        {
            ChromieSelectionSelectedCellController selectionCell = _selctionsCellsList[i];
            selectionCell.SetSelection(GameSetupManager.Instance.SelectedChromiez[i]);
        }
    }

    private void PopulateInventoryList()
    {
        for (int i=0; i< _inventoryListContent.childCount; i++)
        {
            if (_inventoryListContent.GetChild(i).gameObject.activeInHierarchy)
            {
                Destroy(_inventoryListContent.GetChild(i).gameObject);
            }
        }

        List<ChromieDefenition> chromiez = ChromezData.Instance.Chromiez;
        foreach (ChromieDefenition chromieDefenition in chromiez)
        {
            ChromieSelectionItemCellController cellController = CreateInventoryItemCell(chromieDefenition);
            if (cellController != null)
            {
                cellController.SetCell(chromieDefenition, false);
                cellController.OnItemCellSelected += InventoryCellSelectedHandler;
            }
        }
    }

    private ChromieSelectionItemCellController CreateInventoryItemCell(ChromieDefenition chromieDefenition)
    {
        GameObject newCell = Instantiate(_inventoryItemCellPrefab);
        newCell.transform.SetParent(_inventoryListContent);
        newCell.transform.localScale = Vector3.one;
        newCell.SetActive(true);
        return newCell.GetComponent<ChromieSelectionItemCellController>(); 
    }

    private bool AddSelection(ChromieDefenition chromieDefenition)
    {
        bool result = GameSetupManager.Instance.TryAddSelection(chromieDefenition);
        if (result)
        {
            UpdateSelectionList();
        }
        return result;
    }

    private bool RemoveSelection(ChromieDefenition chromieDefenition)
    {
        bool result = GameSetupManager.Instance.TryRemoveSelection(chromieDefenition);
        if (result)
        {
            UpdateSelectionList();
        }
        return result;
    }

    private void DisplayInfo(ChromieDefenition chromieDefenition)
    {
        _currenctSelection = chromieDefenition;

        string infoString = "";
        infoString = chromieDefenition.ChromieName + "\n\n";
        if (chromieDefenition.PassivePowerup != null)
        {
            infoString += chromieDefenition.PassivePowerup.Description + "\n\n";
        }
        if (chromieDefenition.ActivePowerup != null)
        {
            infoString += chromieDefenition.ActivePowerup.Description + "\n\n";
        }
        if (chromieDefenition.DroppedPowerup != null)
        {
            infoString += chromieDefenition.DroppedPowerup.Description + "\n\n";
        }
        _chromieInfoLabel.text = infoString;


        if (InventoryManager.Instance.HasItem(chromieDefenition.ChromieColor.ToString()))
        {
            if (_unlockButton != null)
            {
                _unlockButton.SetActive(false);
            }
        }
        else
        {
            ShopItem shopitem = ShopManager.Instance.GetShopItemForChromie(chromieDefenition.ChromieColor);
            if (shopitem != null)
            {
                if (_unlockButton != null)
                {
                    _unlockButton.SetActive(true);
                }
                if (_unlockPriceLabel != null)
                {
                    _unlockPriceLabel.text = shopitem.Price.Amount.ToString();
                }
            }
        }
    }

    public void UnlockButtonAction()
    {
        if (_currenctSelection != null)
        {
            ShopItem shopitem = ShopManager.Instance.GetShopItemForChromie(_currenctSelection.ChromieColor);
            if (shopitem != null)
            {
                ShopManager.Instance.Purchase(shopitem, (sucsess) =>
                {

                });
            }
        }
    }

    #endregion


    #region Events Hadnlers

    private void InventoryCellSelectedHandler(ChromieSelectionItemCellController cellController)
    {
        if (InventoryManager.Instance.HasItem(cellController.ChromieDefenition.ChromieColor.ToString()))
        {
            if (AddSelection(cellController.ChromieDefenition))
            {
                cellController.Selected = true;
            }
        }
        DisplayInfo(cellController.ChromieDefenition);
    }

    private void SelectionCellSelectedHandler(ChromieSelectionSelectedCellController cellController)
    {
        if (RemoveSelection(cellController.ChromieDefenition))
        {
            cellController.SetSelection(null);
        }
    }

    private void OnInventoryUpdateHandler()
    {
        PopulateInventoryList();
        if (_currenctSelection != null)
        {
            DisplayInfo(_currenctSelection);
        }
    }

    #endregion
}
