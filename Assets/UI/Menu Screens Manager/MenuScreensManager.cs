using UnityEngine;
using System.Collections;

public enum MenuScreenType
{
	MainMenu,
	ModeSelection,
	ChromieSelection,
	EndGame,
	Shop,
	About,
	Settings,
}

public class MenuScreensManager : MonoBehaviour {

	#region Public Proprties

	public GameObject MainMenuPanel;

	public GameObject ModeSelectionPanel;

	public GameObject ChromieSelectionPanel;
	
	public GameObject EndGamePanel;

	public GameObject ShopPanel;

	public GameObject AboutPanel;

	public GameObject SettingsPanel;

    [SerializeField]
    private MenuScreenType _selectedScreen;

    #endregion


    #region Private Properties


    private BaseMenuScreen _currentScreen = null;

	#endregion


	#region Initialize

	IEnumerator Start()
	{
		DisableAllPanels();
        yield return null;
        yield return null;
        Debug.Log("menu start");
        DisplayMenuScreen(_selectedScreen);
    }

	#endregion


	#region Public

    public MenuScreenType SelectedScreen
    {
        set
        {
            _selectedScreen = value;
        }
    }

	/// <summary>
	/// Displaies the menu screen.
	/// </summary>
	/// <param name="screenType">Screen type.</param>
	/// <param name="animated">If set to <c>true</c> animated.</param>
	public void DisplayMenuScreen(MenuScreenType screenType)
	{
		GameObject newScreenPanel = GetPanel(screenType);
		if (newScreenPanel != null)
		{
			BaseMenuScreen newScreen = newScreenPanel.GetComponent<BaseMenuScreen>() as BaseMenuScreen;
			if (_currentScreen != null)
			{
                _currentScreen.DisplayExitAnimationWithCompletion(()=>
				                                                  {
                                                                   
					_currentScreen.ScreenWillBeRemoved();
					_currentScreen.gameObject.SetActive(false);
					//DisableAllPanels();
					newScreen.gameObject.SetActive(true);
					newScreen.ScreenWillBeDisplayed();
                    _currentScreen = newScreen;
					newScreen.DisplayEnterAnimationWithCompletion(()=>
					                                              {
						
					});
				});
			}
			else
			{
				newScreen.gameObject.SetActive(true);
				newScreen.ScreenWillBeDisplayed();
                _currentScreen = newScreen;
                newScreen.DisplayEnterAnimationWithCompletion(()=>
				{
					
				});
			}
		}
		else
		{
			Debug.LogError("ERROR - cant find panel for: " + screenType);
		}
	}


    #endregion


    #region Private

    private GameObject GetPanel(MenuScreenType screenType)
	{
		GameObject panel = null;
		switch (screenType) 
		{
		case MenuScreenType.About:
		{
			panel = AboutPanel;
			break;
		}
		case MenuScreenType.ChromieSelection:
		{
			panel = ChromieSelectionPanel;
			break;
		}
		case MenuScreenType.EndGame:
		{
			panel = EndGamePanel;
			break;
		}
		case MenuScreenType.MainMenu:
		{
			panel = MainMenuPanel;
			break;
		}
		case MenuScreenType.ModeSelection:
		{
			panel = ModeSelectionPanel;
			break;
		}
		case MenuScreenType.Settings:
		{
			panel = SettingsPanel;
			break;
		}
		case MenuScreenType.Shop:
		{
			panel = ShopPanel;
			break;
		}
		}

		return panel;
	}

	private void DisableAllPanels()
	{
		if (AboutPanel != null)
		{
			AboutPanel.SetActive(false);
		}
		if (ChromieSelectionPanel != null)
		{
			ChromieSelectionPanel.SetActive(false);
		}
		if (EndGamePanel != null)
		{
			EndGamePanel.SetActive(false);
		}
		if (MainMenuPanel != null)
		{
			MainMenuPanel.SetActive(false);
		}
		if (ModeSelectionPanel != null)
		{
			ModeSelectionPanel.SetActive(false);
		}
		if (SettingsPanel != null)
		{
			SettingsPanel.SetActive(false);
		}
		if (ShopPanel != null)
		{
			ShopPanel.SetActive(false);
		}
	}

	#endregion


}
