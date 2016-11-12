using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameplayController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private ColorZonesController _colorZonesController;

    [SerializeField]
    private SpwanerController _spwanerController;

    [SerializeField]
    private bool _isGameOver;

    [SerializeField]
    private int _currentLevel;

    [SerializeField]
    private GameplayGUIController _gameplayGUIController;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    #endregion


    #region Initilization

    // Use this for initialization
    IEnumerator Start()
    {
        Physics2D.gravity = new Vector2(0, -9.81f * GameplaySettings.Instance.GameSpeedMultiplier);

        _gameplayTrackingData = new GameplayTrackingData();
        _gameplayTrackingData.SelectedColors = GameSetupManager.Instance.SelectedChromiezColorsList;
        GameSetupManager.Instance.Init();
        yield return 0f;
        PrepareGame();
    }

    private void PrepareGame()
    {
        _currentLevel = 0;
        // create color zones
        _colorZonesController.CreateColorZones(GameSetupManager.Instance.SelectedChromiez);
        // init the spwaner
        _spwanerController.Init(GameSetupManager.Instance.SelectedPlayMode, GameSetupManager.Instance.SelectedChromiez);

        GameplayEventsDispatcher.Instance.OnChromieHitColorZone += OnChromieHitColorZoneHandler;
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;
        GameplayEventsDispatcher.Instance.OnTimeUp += OnTimeUpHandler;

        if (_gameplayGUIController != null)
        {
            _gameplayGUIController.DisplayGameplayGUI(GameSetupManager.Instance.SelectedPlayMode);
        }

        this.gameObject.AddComponent<GameplayScoreManager>().Init(_gameplayTrackingData);

        StartCoroutine(StartPlaying());
    }

    public IEnumerator StartPlaying()
    {
        yield return new WaitForSeconds(0.5f);

        _spwanerController.StartSpwaning();

        switch (GameSetupManager.Instance.SelectedPlayMode)
        {
            case eGameplayMode.Classic:
                {
                    gameObject.AddComponent<GameplayLivesManager>().Init(GameplaySettings.Instance.BaseClassicNumberOfLives, _gameplayTrackingData);
                    break;
                }
            case eGameplayMode.Rush:
                {
                    GameplayTimerManager timerManager = gameObject.AddComponent<GameplayTimerManager>();
                    timerManager.Init(GameplaySettings.Instance.BaseRushGameTime);
                    timerManager.Run();
                    break;
                }
        }

        // activate passive powerups
        foreach (ChromieDefenition chromieDefenition in GameSetupManager.Instance.SelectedChromiez)
        {
            if (chromieDefenition.PassivePowerup != null)
            {
                chromieDefenition.PassivePowerup.StartPowerup(null);
            }
        }
    }

    #endregion


    #region Private

    private void ChromieCollected(ChromieController chromieController, ColorZoneController colorZone)
    {

        //chromieController.CollectChromie();

        //_spwanerController.ChromieCollected(chromieController);

        //if (colorZone != null)
        //{
        //    colorZone.CollectChromie(chromieController);
        //}

        GameplayEventsDispatcher.SendChromieCollected(chromieController, colorZone);

        //  int currentScore = this.gameObject.GetComponent<ScoreCounterManager>().Score;

        //  _gameplayTrackingData.Score = currentScore;

        //if (_currentLevel < _levelRequierments.Length)
        //{
        //    int currentLevelRequirment = _levelRequierments[_currentLevel];
        //    if (currentScore > currentLevelRequirment)
        //    {
        //        _currentLevel++;
        //        _spwanController.UpdateLevel(_currentLevel);
        //    }
        //}
    }

    #endregion


    #region Events

    private void OnChromieHitColorZoneHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.ChromieType == colorZone.Color)
        {
            ChromieCollected(chromieController, colorZone);
        }
    }

    private void OnGameOverHandler()
    {
        FlowManager.Instance.GameOver(_gameplayTrackingData);
    }

    private void OnTimeUpHandler()
    {
        FlowManager.Instance.GameOver(_gameplayTrackingData);
    }

    #endregion

}
