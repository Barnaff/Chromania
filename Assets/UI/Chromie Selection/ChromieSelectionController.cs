﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChromieSelectionController : BaseMenuScreen {

    #region Private Properties

    [SerializeField]
    private GameObject _chromieSelectionCellPrefab;

    [SerializeField]
    private List<GameObject> _selectionSlots;

    [SerializeField]
    private Button _letsPlayButton;

    [SerializeField]
    private GridLayoutGroup _gridLayout;

    [SerializeField]
    private RectTransform _gridContainer;

    [SerializeField]
    private Canvas _canvas;

    IGameSetup _gameSetupmanager;

	GameData _gameData;

	#endregion


	#region Initialize

	void Start()
	{
		this.gameObject.transform.localPosition = Vector3.zero;
        _gameSetupmanager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;

		IGameDataLoader gameDataLoader = ComponentsFactory.GetAComponent<IGameDataLoader>() as IGameDataLoader;
		if (gameDataLoader != null)
		{
			_gameData = gameDataLoader.GetGameData();

			SetInventory();

			UpdateSlots();
		}
	}

    void Update()
    {

    }

	#endregion


	#region Buttons Actions

	public void LetsPlayButtonAction()
	{
		IScreensFlow screenFlowManager = ComponentsFactory.GetAComponent<IScreensFlow>() as IScreensFlow;
		if (screenFlowManager != null)
		{
			screenFlowManager.StartGameScene();
		}

		_gameSetupmanager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;
	}

	public void BackButtonAction()
	{
		GameObject menuScreenManager = GameObject.Find("Menu Screens Manager") as GameObject;
		if (menuScreenManager != null)
		{
			menuScreenManager.GetComponent<MenuScreensManager>().DisplayMenuScreen(MenuScreenType.ModeSelection);
		}
	}

	public void ItemSlotButtonAction(GameObject sender)
	{
		int index = _selectionSlots.IndexOf(sender);
		RemoveColor(index);
	}

    #endregion


    #region Public



    #endregion


    #region Private



	private void UpdateSlots()
	{
		for (int i=0; i< _gameSetupmanager.SelectedColors.Length ; i++)
		{
			ColorType selectedColorForSlot = _gameSetupmanager.SelectedColors[i];

			GameObject slot = _selectionSlots[i];

			slot.GetComponent<Image>().sprite = ChromiezSpritesFactory.GetChromieSprite(selectedColorForSlot);

			if (selectedColorForSlot == ColorType.None)
			{
				slot.SetActive(false);
			}
			else
			{
				slot.SetActive(true);
			}
		}


		bool isReady = true;
		foreach (ColorType selectedColor in _gameSetupmanager.SelectedColors)
		{
			if (selectedColor == ColorType.None)
			{
				isReady = false;
			}
		}

		_letsPlayButton.enabled = isReady;
	}

	private void AddColor(ColorType colorType, ChromieSelectionCellController cell)
	{
		for (int i=0; i <  _gameSetupmanager.SelectedColors.Length ; i++)
		{
			ColorType selectedColorForSlot = _gameSetupmanager.SelectedColors[i];
			if (selectedColorForSlot == colorType)
			{
				return;
			}
		}


        for (int i=0; i <  _gameSetupmanager.SelectedColors.Length ; i++)
		{
			ColorType selectedColorForSlot = _gameSetupmanager.SelectedColors[i];
			if (selectedColorForSlot == ColorType.None)
			{
				_gameSetupmanager.SelectedColors[i] = colorType;
				UpdateSlots();
                cell.SetSelectState(true, true);
                GameObject slot = _selectionSlots[i];
                StartCoroutine(AnimateAddColor(cell, slot, () =>
                             {

                                
                             }));
				return;
			}
		}
	}

	private void RemoveColor(int index)
	{
        StartCoroutine(AnimateRemoveColor(_selectionSlots[index].gameObject, () =>
        {
            ChromieSelectionCellController cellController = CellForColor(_gameSetupmanager.SelectedColors[index]);
            if (cellController != null)
            {
                cellController.SetSelectState(false, true);
            }

            _gameSetupmanager.SelectedColors[index] = ColorType.None;
            UpdateSlots();
        }));

      
	}

    private void SetInventory()
    {
        for (int i = 0; i < _gridLayout.gameObject.transform.childCount; i++)
        {
            if (_gridLayout.gameObject.transform.GetChild(i) != null)
            {
                Destroy(_gridLayout.gameObject.transform.GetChild(i).gameObject);
            }
        }

        IGameDataLoader gameDataLoader = ComponentsFactory.GetAComponent<IGameDataLoader>() as IGameDataLoader;
        if (gameDataLoader != null)
        {
            List<ChromieDataItem> allChromiezList = gameDataLoader.GetAllChromiezData();

            foreach (ChromieDataItem chromie in allChromiezList)
            {
                GameObject chromieCell = Instantiate(_chromieSelectionCellPrefab) as GameObject;
                chromieCell.transform.SetParent(_gridLayout.gameObject.transform);
				chromieCell.transform.localScale = new Vector3(1,1,1);
                ChromieSelectionCellController cellController = chromieCell.GetComponent<ChromieSelectionCellController>() as ChromieSelectionCellController;
                if (cellController != null)
                {
                    cellController.SetChromie(chromie);
                    cellController.OnSelectChromieCell += SelectChromieCellhandler;
                }
            }
        }
    }

    private void SelectChromieCellhandler(ChromieSelectionCellController cell)
    {
        ColorType colorType = cell.CellColorType;
        AddColor(colorType, cell);
    }


    IEnumerator AnimateAddColor(ChromieSelectionCellController fromCell, GameObject toSlot, System.Action completionAction)
	{
        Vector3 targetPosition = toSlot.transform.position;
        toSlot.transform.position = fromCell.transform.position;

        iTween.Stop(toSlot);
        yield return null;

        float animationDuration = 0.01f;
        iTween.ScaleTo(toSlot, iTween.Hash("time", animationDuration, "x", 1f, "y", 1f));
        yield return new WaitForSeconds(animationDuration);

        animationDuration = 0.3f;
        iTween.MoveTo(toSlot, iTween.Hash("time", animationDuration, "position", targetPosition, "easetype", iTween.EaseType.easeInCubic));
        yield return new WaitForSeconds(animationDuration);

        animationDuration = 0.6f;
        iTween.PunchScale(toSlot, iTween.Hash("time", animationDuration, "x", 0.3f, "y", 0.3f));
        yield return new WaitForSeconds(animationDuration);
       

        if (completionAction != null)
        {
            completionAction();
        }
    }

    IEnumerator AnimateRemoveColor(GameObject colorSlot, System.Action completionAction)
    {
        iTween.Stop(colorSlot);
        yield return null;

        float animationDuration = 0.1f;
        iTween.ScaleTo(colorSlot, iTween.Hash("time", animationDuration, "x", 1.2f, "y", 1.2f, "easetype", iTween.EaseType.easeOutSine));
        yield return new WaitForSeconds(animationDuration);

        animationDuration = 0.3f;
        iTween.ScaleTo(colorSlot, iTween.Hash("time", animationDuration, "x", 0.0f, "y", 0.0f, "easetype", iTween.EaseType.easeInSine));
        yield return new WaitForSeconds(animationDuration);

        colorSlot.SetActive(false);

        yield return null;
       
        if (completionAction != null)
        {
            completionAction();
        }
    }

    private ChromieSelectionCellController CellForColor(ColorType colorType)
    {
        for (int i=0; i< _gridLayout.transform.childCount; i++)
        {
            ChromieSelectionCellController cellController = _gridLayout.transform.GetChild(i).GetComponent<ChromieSelectionCellController>();
            if (cellController != null && cellController.CellColorType == colorType)
            {
                return cellController;
            }
        }
        return null;
    }

	#endregion
}
