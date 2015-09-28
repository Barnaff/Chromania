using UnityEngine;
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
        SetupGridSizes();
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
#if UNITY_EDITOR
        SetupGridSizes();
#endif
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
		Debug.Log("clock on : " + index);
		RemoveColor(index);
	}

    #endregion


    #region Public



    #endregion


    #region Private

    private void SetupGridSizes()
    {
        Rect gridContainerRect = RectTransformUtility.PixelAdjustRect(_gridContainer, _canvas);
        float marginSize = gridContainerRect.width * 0.02f;
        float cellWidth = (gridContainerRect.width - (marginSize * 2.0f)) * 0.3f;
        float cellHeight = cellWidth * 0.6f;
        Vector2 cellSize = new Vector2(cellWidth, cellHeight);
        _gridLayout.padding = new RectOffset((int)marginSize * 2, (int)marginSize, (int)marginSize * 2, (int)marginSize);
        _gridLayout.spacing = new Vector2(marginSize, marginSize);
        _gridLayout.cellSize = cellSize;
    }

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

	private void AddColor(ColorType colorType)
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
				GameObject slot = _selectionSlots[i];
				AnimateColor(Input.mousePosition, slot.transform.position, ()=>
				             {

				});
				return;
			}
		}
	}

	private void RemoveColor(int index)
	{
		_gameSetupmanager.SelectedColors[index] = ColorType.None;
		UpdateSlots();
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
        Debug.Log("selected: " + colorType);
        AddColor(colorType);
    }


    private void AnimateColor(Vector3 startPosition, Vector3 endPosition, System.Action completionAction)
	{
		GameObject slotSprite = Instantiate(_selectionSlots[0], startPosition, Quaternion.identity) as GameObject;
		slotSprite.SetActive(true);


	}

	#endregion
}
