using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    #endregion


    #region Initialization

    void Start()
    {
        GameplayEventsDispatcher.Instance.OnScoreUpdate += OnScoreUpdateHandler;
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

    #endregion
}
