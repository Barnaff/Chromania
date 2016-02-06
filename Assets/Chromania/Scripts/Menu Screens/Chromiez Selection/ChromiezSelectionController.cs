using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChromiezSelectionController : BaseMenuController {

    #region Private Properties

    [SerializeField]
    private GameObject _chromieCellPrefab;

    [SerializeField]
    private Transform _gridContent;

    [SerializeField]
    private List<Image> _selecttedIconsImages;

    private IGameSetup _gameSetup;

    #endregion


    #region BaseMenuController Implementation

  

    #endregion


    #region User Interaction

    public void BackButtonAction()
    {
     
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.ModeSelection);
    }

    public void LetsGoButtonAction()
    {
        if (_gameSetup.IsSelected(eChromieType.None))
        {
            Debug.LogWarning("Warning: trying to start a game with no 4 seelcted chromiez");
            return;
        }


        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.StartGame();
        }
    }

    public void SelectionButtonAction(int index)
    {
        Debug.Log("remove selection: " + index);

        _gameSetup.RemoveChromieAtIndex(index);
    }

    #endregion


    #region Initialization

    void Start()
    {
        _gameSetup = ComponentFactory.GetAComponent<IGameSetup>();

        if (_chromieCellPrefab != null)
        {
            _chromieCellPrefab.SetActive(false);
        }

        PopulateChromiezInventory();
    }

    #endregion


    #region Private

    private void PopulateChromiezInventory()
    {
        IGameData gameDataManager = ComponentFactory.GetAComponent<IGameData>();
        if (gameDataManager != null)
        {
            foreach (ChromieDataObject chromieData in gameDataManager.ChromiezList)
            {
                GameObject cell = Instantiate(_chromieCellPrefab) as GameObject;
                cell.SetActive(true);
                cell.transform.SetParent(_gridContent);
                cell.transform.localScale = new Vector3(1, 1, 1);

                ChromiezSelectionCellController cellController = cell.GetComponent<ChromiezSelectionCellController>();
                if (cellController != null)
                {
                    cellController.SetChromie(chromieData);
                    cellController.OnCellSelected += OnCellSelectedHandler;
                }
            }
        }
       

    }


    #endregion


    #region Events

    private void OnCellSelectedHandler(ChromiezSelectionCellController cellController)
    {
        Debug.Log("add to selection: " + cellController.ChromieData.ChromieColor);

        if (_gameSetup.CanAddChromie())
        {
            int result = _gameSetup.AddChromie(cellController.ChromieData.ChromieColor);
            if (result > 0)
            {
                cellController.SetSelected(true);
            }
        }
       
    }

    #endregion
}
