using UnityEngine;
using System.Collections;

public class ScreensFlowManager : MonoBehaviour, IScreensFlow {

	private bool _isEndgame = false;

	#region IScreensFlow Implementation

	public void DisplayMenuScene()
	{
		Application.LoadLevel("MenuScene");
	}

	public void StartGameScene()
	{
		Application.LoadLevel("GameScene");
	}

	public void DisplayEndGameScreen()
	{
		_isEndgame = true;
		Application.LoadLevel("MenuScene");
	}

	#endregion
	
	#region Scene Loaded Events

	void OnLevelWasLoaded(int level)
	{
		if (level == 1)
		{
			Debug.Log("loaded main menu");
			GameObject menuScreenManager = GameObject.Find("Menu Screens Manager") as GameObject;
			if (menuScreenManager != null)
			{
				if (_isEndgame)
				{
					menuScreenManager.GetComponent<MenuScreensManager>().DisplayMenuScreen(MenuScreenType.EndGame);
					_isEndgame = false;
				}
				else
				{
					menuScreenManager.GetComponent<MenuScreensManager>().DisplayMenuScreen(MenuScreenType.MainMenu);
				}
			}
			else
			{
				Debug.Log("cant find menu screen manager");
			}

		}
	}

	#endregion
}
