using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScreensManager : MonoBehaviour {

    #region Public Properties

    [SerializeField]
    public List<BaseMenuController> _menuScreens;

    [SerializeField]
    private eMenuScreenType _selectedScreenOnStart;

    #endregion

    #region Private Properties

    private BaseMenuController _currentScreen;

    #endregion

    #region Initialization

    // Use this for initialization
    void Awake ()
    {
       
        foreach (BaseMenuController screen in _menuScreens)
        {
            screen.gameObject.SetActive(false);
        }

        if (_selectedScreenOnStart != eMenuScreenType.None)
        {
            DisplayMenuScreen(_selectedScreenOnStart);
        }
    }

    #endregion


    #region Public

   
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
            if (animated)
            {
                yield return _currentScreen.DisplayExitAnimationCorutine();
            }
            _currentScreen.gameObject.SetActive(false);
        }

        _currentScreen = GetScreen(screenType);

        _currentScreen.gameObject.SetActive(true);
        if (animated)
        {
            yield return _currentScreen.DisplayEnterAnimationCorutine();
        }

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
