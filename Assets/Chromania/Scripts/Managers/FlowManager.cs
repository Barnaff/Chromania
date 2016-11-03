using UnityEngine;
using System.Collections;

public class FlowManager : Kobapps.Singleton<FlowManager> {


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

        });
    }

    public void GameOver(GameplayTrackingData gameplayTrackingData)
    {
        ServerRequestsManager.Instance.PostLeaderboardEntry(gameplayTrackingData, () =>
        {
            // posted leaderboard entry to server
        });

        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MenuScene, () =>
        {
            MenuScreensController.Instance.DisplayScreen(MenuScreensController.eMenuScreenType.GameOver);

            GameOverScreenController gameOverScreenController = GameObject.FindObjectOfType<GameOverScreenController>();
            if (gameOverScreenController != null)
            {
                gameOverScreenController.DisplayGameOverData(gameplayTrackingData);
            }
        });
    }

    public void QuitGame()
    {
        DisplayMainMenu();
    }
}
