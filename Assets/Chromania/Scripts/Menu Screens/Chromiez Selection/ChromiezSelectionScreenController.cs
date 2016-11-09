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

    [SerializeField]
    private Text _chromieInfoLabel;

    #endregion


    #region Initialization

    void Start()
    {
        InitScreen();
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
        string infoString = "";
        infoString = chromieDefenition.ChromieName + "\n";
        if (chromieDefenition.PassivePowerup != null)
        {
            infoString = "Passivs: " + chromieDefenition.PassivePowerup.name + "\n";
        }
        if (chromieDefenition.ActivePowerup != null)
        {
            infoString = "active: " + chromieDefenition.ActivePowerup.name + "\n";
        }
        _chromieInfoLabel.text = infoString;
    }

    #endregion


    #region Events Hadnlers

    private void InventoryCellSelectedHandler(ChromieSelectionItemCellController cellController)
    {
        if (AddSelection(cellController.ChromieDefenition))
        {
            cellController.Selected = true;
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

    #endregion
}
