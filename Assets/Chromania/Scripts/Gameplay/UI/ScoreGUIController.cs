using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _scoreMultiplierLabel;

    #endregion


    #region Initialization

    void Start()
    {
        _scoreMultiplierLabel.gameObject.SetActive(false);
        GameplayEventsDispatcher.Instance.OnScoreUpdate += OnScoreUpdateHandler;
        GameplayEventsDispatcher.Instance.OnScoreMultiplierUpdate += OnScoreMultiplierUpdateHandler;
        UpdateScoreDisplay(0);
    }

    #endregion


    #region Private

    private void UpdateScoreDisplay(int newScoreValue)
    {
        _scoreLabel.text = newScoreValue.ToString();
    }

    #endregion

    #region Events

    private void OnScoreUpdateHandler(int scoreAdded, int newScore)
    {
        UpdateScoreDisplay(newScore);
    }

    private void OnScoreMultiplierUpdateHandler(int scoreMultiplier)
    {
        if (scoreMultiplier > 1)
        {
            _scoreMultiplierLabel.gameObject.SetActive(true);
            _scoreMultiplierLabel.text = "X" + scoreMultiplier.ToString();
        }
        else
        {
            _scoreMultiplierLabel.gameObject.SetActive(false);
        }
    }

    #endregion
}
