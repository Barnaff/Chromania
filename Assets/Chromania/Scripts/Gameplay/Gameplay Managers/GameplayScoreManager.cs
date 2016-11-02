using UnityEngine;
using System.Collections;

public class GameplayScoreManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

#if UNITY_EDITOR
    [SerializeField]
    private int _currentScore;
#endif

    [SerializeField]
    private int _currentScoreMultiplier = 1;

    #endregion


    #region Public

    public void Init(GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;
        _currentScoreMultiplier = 1;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    public void AddScoreMultiplier(int scoreMultiplierToAdd)
    {
        _currentScoreMultiplier += scoreMultiplierToAdd;
    }

    public void RemoveScoreMultiplier(int scoreMultiplierToRemove)
    {
        _currentScoreMultiplier -= scoreMultiplierToRemove;
    }

    #endregion

    #region Private 


    #endregion

    #region Events

    private void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        int scoreToAdd = 1;

        _gameplayTrackingData.Score += scoreToAdd;

        GameplayEventsDispatcher.Instance.ScoreUpdate(scoreToAdd, _gameplayTrackingData.Score);

#if UNITY_EDITOR
        _currentScore = _gameplayTrackingData.Score;
#endif

    }

    #endregion
}
