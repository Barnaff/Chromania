using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _scoreMultiplierLabel;

    [SerializeField]
    private bool _displayScoreIndicationOnCollection;

    [SerializeField]
    private ScoreIndicatorController _scoreIndicatorPrefab;

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

    private void OnScoreUpdateHandler(int scoreAdded, int newScore, ChromieController chromieController)
    {
        UpdateScoreDisplay(newScore);

        if (_displayScoreIndicationOnCollection && chromieController != null)
        {
            Vector3 position = chromieController.transform.localPosition;
            ScoreIndicatorController scoreIndicator = Lean.LeanPool.Spawn(_scoreIndicatorPrefab, position, Quaternion.identity, this.transform);
            scoreIndicator.transform.position = position;
            scoreIndicator.DisplayScore(scoreAdded);
        }
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
