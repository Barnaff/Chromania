using UnityEngine;
using System.Collections;

public class PauseManager : Kobapps.Singleton<PauseManager> {

    #region Initialization

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChangedHandler;
    }

    #endregion

    #region Public

    public void PauseGame()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == GeneratedConstants.Scenes.GameplayScene)
        {
            PopupsManager.Instance.DisplayPopup<PausePopupController>(()=>
            {
                ResumeGame();
            });
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    #endregion


    #region Events

    private void ActiveSceneChangedHandler(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
    {
        Time.timeScale = 1;
    }

    #endregion
}
