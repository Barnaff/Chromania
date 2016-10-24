using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreenController : MenuScreenBaseController{

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    #endregion


    #region Public

    public void DisplayGameOverData(GameplayTrackingData gameplayTrackingData)
    {
        _scoreLabel.text = gameplayTrackingData.Score.ToString();
    }

    #endregion


    #region Public - User Interactions

    public void PlayAgainButtonAction()
    {
        FlowManager.Instance.StartGame();
    }

    public void MainMenuButtonAction()
    {
        FlowManager.Instance.DisplayMainMenu();
    }

    #endregion
}
