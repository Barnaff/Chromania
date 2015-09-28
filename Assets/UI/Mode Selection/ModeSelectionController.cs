using UnityEngine;
using System.Collections;

public class ModeSelectionController : BaseMenuScreen {

	#region Private Properties

	private MenuScreensManager _menuScreenManager;
	private IGameSetup _gameSetupManager;

	#endregion

	#region Initialize

	void Start()
	{
		this.gameObject.transform.localPosition = Vector3.zero;

		GameObject menuScreenManager = GameObject.Find("Menu Screens Manager") as GameObject;
		if (menuScreenManager != null)
		{
			_menuScreenManager = menuScreenManager.GetComponent<MenuScreensManager>() as MenuScreensManager;
		}

		_gameSetupManager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;
	}

	#endregion

	#region Buttons Actions

	public void BackButtonAction()
	{
		_menuScreenManager.DisplayMenuScreen(MenuScreenType.MainMenu);
	}

	public void PlayClassicButtonAction()
	{
		_gameSetupManager.SelectedGameMode = GameModeType.Classic;
		_menuScreenManager.DisplayMenuScreen(MenuScreenType.ChromieSelection);
	}

	public void PlayRushButtonAction()
	{
		_gameSetupManager.SelectedGameMode = GameModeType.Rush;
		_menuScreenManager.DisplayMenuScreen(MenuScreenType.ChromieSelection);
	}

	#endregion
}
