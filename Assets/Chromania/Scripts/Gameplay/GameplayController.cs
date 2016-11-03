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

    // Update is called once per frame
    void Update () {
	
	}


    #region Initilization

    // Use this for initialization
    IEnumerator Start()
    {
        _gameplayTrackingData = new GameplayTrackingData();
        _gameplayTrackingData.SelectedColors = GameSetupManager.Instance.SelectedChromiezColorsList;
        GameSetupManager.Instance.Init();
        yield return null;
        PrepareGame();
    }

    private void PrepareGame()
    {
        _currentLevel = 0;
        _colorZonesController.CreateColorZones(GameSetupManager.Instance.SelectedChromiez);
        _spwanerController.Init(GameSetupManager.Instance.SelectedPlayMode, GameSetupManager.Instance.SelectedChromiez, 1);

        GameplayEventsDispatcher.Instance.OnChromieHitColorZone += OnChromieHitColorZoneHandler;
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;

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
