using UnityEngine;
using System.Collections;

public class MainMenuController : BaseMenuScreen {
	
	#region Buttons Actions

	public void ButtonActionPlay()
	{
		GameObject menuScreenManager = GameObject.Find("Menu Screens Manager") as GameObject;
		if (menuScreenManager != null)
		{
			menuScreenManager.GetComponent<MenuScreensManager>().DisplayMenuScreen(MenuScreenType.ModeSelection);
		}
	}

	public void ButtonActionSettings()
	{

	}

	public void ButtonActionAbout()
	{

	}

	#endregion

	
	void Start()
	{
		DisplayEnterAnimationWithCompletion(()=>
		                                    {

		});
	}

}
