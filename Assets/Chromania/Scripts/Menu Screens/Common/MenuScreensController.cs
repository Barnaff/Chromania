using UnityEngine;
using System.Collections;

public class MenuScreensController : MonoBehaviour {

    #region Public Properties

    public enum eMenuScreenType
    {
        MainMenu,
        ModeSelection,
        ChromiezSelection,
        Shop,
        Settings,
        GameOver,
    }

    #endregion

    #region Private Properties

    [SerializeField]
    private GameObject _mainMenuScreen;

    [SerializeField]
    private GameObject _modeSelectionScreen;

    [SerializeField]
    private GameObject _chromiezSelectionScreen;

    [SerializeField]
    private GameObject _gameOverScreen;

    private GameObject _currentDisplayingScreen = null;

    #endregion


    #region Singleton

    private static MenuScreensController _instance;

    void Awake()
    {
        _instance = this;
    }

    void OnDestory()
    {
        _instance = null;
    }

    public static MenuScreensController Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            Debug.LogError("ERROR - Menu Screen Controller is not in this scene!");
            return null;
        }
    }

    #endregion

   

    #region Public

    public void DisplayScreen(eMenuScreenType screenType)
    {
        if (_currentDisplayingScreen != null)
        {
            _currentDisplayingScreen.SetActive(false);
        }

        switch (screenType)
        {
            case eMenuScreenType.MainMenu:
                {
                    _currentDisplayingScreen = _mainMenuScreen;
                    break;
                }
            case eMenuScreenType.ModeSelection:
                {
                    _currentDisplayingScreen = _modeSelectionScreen;
                    break;
                }
            case eMenuScreenType.ChromiezSelection:
                {
                    _currentDisplayingScreen = _chromiezSelectionScreen;
                    break;
                }
            case eMenuScreenType.Settings:
                {
                    break;
                }
            case eMenuScreenType.Shop:
                {
                    break;
                }
            case eMenuScreenType.GameOver:
                {
                    _currentDisplayingScreen = _gameOverScreen;
                    break;
                }
        }

        HideAllScreens();
        _currentDisplayingScreen.SetActive(true);

    }

    #endregion


    #region Private

    private void HideAllScreens()
    {
        _mainMenuScreen.SetActive(false);
        _modeSelectionScreen.SetActive(false);
        _chromiezSelectionScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
    }

    #endregion
}
