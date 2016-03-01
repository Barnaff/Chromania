using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    #endregion


    #region Initialization

    void Start ()
    {
        UpdateScoreLabel(false);
    }

    public GameplayTrackingData GameplayTrackingData
    {
        set
        {
            _gameplayTrackingData = value;
        }
    }

    #endregion


    #region Public

    public void AddScore(int scoreToAdd)
    {
        _gameplayTrackingData.Score += scoreToAdd;
        UpdateScoreLabel(true);
    }

    public int Score
    {
        get
        {
            return _gameplayTrackingData.Score;
        }
    }

    #endregion


    #region Private

    private void UpdateScoreLabel(bool animated)
    {
        if (_gameplayTrackingData != null)
        {
            _scoreLabel.text = _gameplayTrackingData.Score.ToString();
            if (animated)
            {
                iTween.PunchScale(_scoreLabel.gameObject, iTween.Hash("time", 0.5f, "amount", new Vector3(1.1f, 1.1f, 1.1f)));
            }
        }
        else
        {
            _scoreLabel.text = "0";
        }
    }

    #endregion
}
