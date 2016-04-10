using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FlowManager : FactoryComponent, IFlow
{

    #region Private Properties

    [SerializeField]
    private MenuScreensManager _menuScreenManager;

    #endregion

    #region FactoryComponent Implementation

    public override void InitComponentAtStart()
    {

    }

    public override void InitComponentAtAwake()
    {

    }

    #endregion


    #region IFlow Implementation

    public void Autologin(System.Action completionAction)
    {
        IBackend serverBackend = ComponentFactory.GetAComponent<IBackend>();
        if (serverBackend != null)
        {
            serverBackend.Login(() =>
            {
                if (completionAction != null)
                { 
                    completionAction();
                }
            });
        }
    }

    public void FacebookConnect(System.Action completionAction)
    {
        IFacebookManager facebookManager = ComponentFactory.GetAComponent<IFacebookManager>();
        if (facebookManager != null)
        {
            facebookManager.FacebookLogin(() =>
            {
                if (facebookManager.IsLoggedIn)
                {
                    IBackend serverBackend = ComponentFactory.GetAComponent<IBackend>();
                    if (serverBackend != null)
                    {
                        serverBackend.FacebookConnect(facebookManager.AcsessToken, () =>
                        {
                            if (completionAction != null)
                            {
                                completionAction();
                            }
                        });
                    }
                }

            });
        }
    }

    public void StartGame()
    {

        SceneManager.LoadScene("Gameplay Scene");
    }

    public void FinishGame(GameplayTrackingData gameplayTrackingData)
    {
        IPopups popupsManager = ComponentFactory.GetAComponent<IPopups>();
        if (popupsManager != null)
        {
            GameEndedPopupController gameEndedPopupController = popupsManager.DisplayPopup<GameEndedPopupController>();
            if (gameEndedPopupController != null)
            {
                gameEndedPopupController.SetGameplayData(gameplayTrackingData);
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu Scene");

    }

    public void DisplayMenuScreen(eMenuScreenType menuScreenType, bool animated = true)
    {
        MenuManager.DisplayMenuScreen(menuScreenType, animated);
    }

    #endregion


    #region Private 

    private MenuScreensManager MenuManager
    {
        get
        {
            if (_menuScreenManager == null)
            {
                _menuScreenManager = GameObject.FindObjectOfType<MenuScreensManager>();
            }
            return _menuScreenManager;
        }
    }

    #endregion
}
