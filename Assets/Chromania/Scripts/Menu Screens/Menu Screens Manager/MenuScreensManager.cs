using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScreensManager : MonoBehaviour {

    #region Public Properties

    [SerializeField]
    public List<BaseMenuController> _menuScreens;

    #endregion

    #region Private Properties

    private static MenuScreensManager _instance;

    private const string MAIN_MENU_CONTROLLER_PREFAB_NAME = "MainMenuController";

    private BaseMenuController _currentScreen;

    #endregion

    #region Initialization

    // Use this for initialization
    void Awake ()
    {
        MenuScreensManager._instance = this;

        foreach (BaseMenuController screen in _menuScreens)
        {
            screen.gameObject.SetActive(false);
        }
    }

    #endregion


    #region Public

    public static MenuScreensManager Instance()
    {
        if (MenuScreensManager._instance == null)
        {
            GameObject mainMenuControllerPrefab = Resources.Load(MAIN_MENU_CONTROLLER_PREFAB_NAME) as GameObject;
            if (mainMenuControllerPrefab != null)
            {
                GameObject mainMenuController = Instantiate(mainMenuControllerPrefab) as GameObject;
                mainMenuController.transform.position = Vector3.zero;
            } 
        }
        return MenuScreensManager._instance;
    }

    public void DisplayMenuScreen(eMenuScreenType screenType, bool animated = true)
    {
        StartCoroutine(DisplayScreen(screenType, animated));
    }

    #endregion


    #region Private

    IEnumerator DisplayScreen(eMenuScreenType screenType, bool animated)
    {
        if (_currentScreen != null)
        {
            yield return null;
            _currentScreen.gameObject.SetActive(false);
        }


        _currentScreen = GetScreen(screenType);

        _currentScreen.gameObject.SetActive(true);

        yield return null;
    }

    private BaseMenuController GetScreen(eMenuScreenType screenType)
    {
        foreach (BaseMenuController screen in _menuScreens)
        {
            if (screen.ScreenType == screenType)
            {
                return screen;
            }
        }
        Debug.LogError("ERROR - Screen [" + screenType.ToString() + "] Could not be found!");
        return null;
    }

    #endregion
}
