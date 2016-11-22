using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreenController : MenuScreenBaseController{

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _bestScoreLabel;

    [SerializeField]
    private Text _currencyGainLabel;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    #endregion


    #region Public

    public void DisplayGameOverData(GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;
        _scoreLabel.text = _gameplayTrackingData.Score.ToString();
        GameplayTrackingData highScore = HighScoreManager.Instance.GetHighScore(_gameplayTrackingData.GameplayMode);
        if (highScore != null)
        {
            _bestScoreLabel.text = "Best: " + highScore.Score;
        }
        else
        {
            _bestScoreLabel.gameObject.SetActive(false);
        }
        _currencyGainLabel.text = _gameplayTrackingData.CollectedCurrency.ToString() + "Coins";

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
