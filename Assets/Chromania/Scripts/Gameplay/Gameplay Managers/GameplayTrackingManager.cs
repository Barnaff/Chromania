using UnityEngine;
using System.Collections;

public class GameplayTrackingManager : MonoBehaviour {

    #region Private

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    #endregion


    #region Public

    public GameplayTrackingData StartTracking()
    {
        _gameplayTrackingData = new GameplayTrackingData();
        SetupTrackingData();
        AddEventListeners();
        return _gameplayTrackingData;
    }

    #endregion


    #region Private 

    private void SetupTrackingData()
    {
        _gameplayTrackingData.SelectedColors = GameSetupManager.Instance.SelectedChromiezColorsList;
        _gameplayTrackingData.GameplayMode = GameSetupManager.Instance.SelectedPlayMode;
    }

    private void AddEventListeners()
    {
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
        GameplayEventsDispatcher.Instance.OnScoreUpdate += OnScoreUpdateHandler;
        GameplayEventsDispatcher.Instance.OnLivesUpdate += OnLivesUpdateHandler;
    }



    #endregion


    #region Events

    private void OnLivesUpdateHandler(int maxLives, int currentLives, int change)
    {
        if (change > 0)
        {
            _gameplayTrackingData.LivesGained++;
        }
        if (maxLives > _gameplayTrackingData.MaxLives)
        {
            _gameplayTrackingData.MaxLives = maxLives;
        }
    }

    private void OnScoreUpdateHandler(int scoreAdded, int newScore, ChromieController chromieController)
    {
        _gameplayTrackingData.Score += scoreAdded;
    }

    private void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        _gameplayTrackingData.CollectedChromiez++;

        if (_gameplayTrackingData.ColletedColors.ContainsKey(chromieController.ChromieDefenition.ChromieColor))
        {
            _gameplayTrackingData.ColletedColors[chromieController.ChromieDefenition.ChromieColor] += 1;
        }
        else
        {
            _gameplayTrackingData.ColletedColors.Add(chromieController.ChromieDefenition.ChromieColor, 1);
        }

        if (chromieController.IsPowerup)
        {
            _gameplayTrackingData.CollectedPowerups++;

            if (_gameplayTrackingData.CollectedPoweupsColors.ContainsKey(chromieController.ChromieDefenition.ChromieColor))
            {
                _gameplayTrackingData.CollectedPoweupsColors[chromieController.ChromieDefenition.ChromieColor] += 1;
            }
            else
            {
                _gameplayTrackingData.CollectedPoweupsColors.Add(chromieController.ChromieDefenition.ChromieColor, 1);
            }
        }
    }

    #endregion
}
