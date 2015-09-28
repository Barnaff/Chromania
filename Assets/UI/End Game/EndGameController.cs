using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameController : BaseMenuScreen {

	#region Public Properties

	public Text ScoreLabel;

	#endregion


	#region Private Properties
	
	private MenuScreensManager _menuScreenManager;

	private int _score;

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


	}

	void OnEnable()
	{
		if (PlayerPrefs.HasKey("score"))
		{
			_score = PlayerPrefs.GetInt("score");
			ScoreLabel.text = _score.ToString();
		}
	}
	
	#endregion


	#region Buttons Actions

	public void PlayAgianButtonAction()
	{
		_menuScreenManager.DisplayMenuScreen(MenuScreenType.ChromieSelection);
	}

	public void MenuButtonAction()
	{
		_menuScreenManager.DisplayMenuScreen(MenuScreenType.MainMenu);
	}

	#endregion
}
