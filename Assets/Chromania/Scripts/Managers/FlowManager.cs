using UnityEngine;
using System.Collections;

public class FlowManager : Kobapps.Singleton<FlowManager> {

    public void DisplayFirstScreenAfterLaunch()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.MainMenu);
        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void DisplayMainMenu()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.MainMenu);
        });
    }

    public void MainMenuPlay()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.ModeSelection);
        });
    }

    public void SelectPlayMode(eGameplayMode gameplayMode)
    {
        GameSetupManager.Instance.SelectedPlayMode = gameplayMode;

        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.ChromiezSelection);
        });
    }

    public void StartGame()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.GameplayScene, () =>
        {

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void RestartGame()
    {
        Kobapps.SceneLoaderutil.ReloadCurrentScene(() =>
        {

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void GameOver(GameplayTrackingData gameplayTrackingData)
    {

        HighScoreManager.Instance.SendGametrackingData(gameplayTrackingData);

        InventoryManager.Instance.Currency += gameplayTrackingData.CollectedCurrency;

        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.GameOver);

            GameOverScreenController gameOverScreenController = GameObject.FindObjectOfType<GameOverScreenController>();
            if (gameOverScreenController != null)
            {
                gameOverScreenController.DisplayGameOverData(gameplayTrackingData);
            }
        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void QuitGame()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            PauseManager.Instance.ResumeGame();
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.MainMenu);
        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void Shop()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.ShopScene, () =>
        {

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }
}
