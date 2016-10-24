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

    #endregion


    #region Public

    public void Init(GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;

        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
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
