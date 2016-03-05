using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    [SerializeField]
    private eChromieType[] _selectedChromiez;

    [SerializeField]
    private eGameMode _selectedGameMode;

    private SpwanerController _spwanController;

    private ColorZonesManager _colorZonesManager;

    [SerializeField]
    private TimerPanelController _timerPanelController;

    [SerializeField]
    private LivesPanelController _livesPanelController;

    [SerializeField]
    private bool _isGameOver;

    [SerializeField]
    private EndGameMessage _timeUpMessage;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    private int[] _levelRequierments;

    private int _currentLevel;

    // Use this for initialization
    void Start () {

        _gameplayTrackingData = new GameplayTrackingData();

        _spwanController = this.gameObject.GetComponent<SpwanerController>();
        if (_spwanController == null)
        {
            throw new System.Exception("Missing spwan manager!");
        }
        _colorZonesManager = this.gameObject.GetComponent<ColorZonesManager>();
        if (_colorZonesManager == null)
        {
            throw new System.Exception("Missing color zones manager!");
        }

        GameplayEventsDispatcher.Instance.OnChromieHitColorZone += OnChromieHitColorZoneHandler;

        InitializeGameplay();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void QuitGame()
    {
        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.MainMenu();
        }
    }

    private void InitializeGameplay()
    {
        IGameSetup gameSetupManager = ComponentFactory.GetAComponent<IGameSetup>();
        if (gameSetupManager != null)
        {
            _selectedChromiez = gameSetupManager.SelectedChromiez;
            _selectedGameMode = gameSetupManager.SelectedGameMode;
        }

        IGameData gameDataManager = ComponentFactory.GetAComponent<IGameData>();
        if (gameDataManager != null)
        {
            _levelRequierments = gameDataManager.LevelsForgameplayMode(_selectedGameMode);
        }

        // do game mode initializations
        switch (_selectedGameMode)
        {
            case eGameMode.Classic:
                {
                    InitializeClassicMode();
                    break;
                }
            case eGameMode.Rush:
                {
                    InitializeRushMode();
                    break;
                }
        }

        _currentLevel = 0;
        _colorZonesManager.Init(_selectedChromiez);

        // TODO: display game intro, then start everything
        StartPlaying();
    }


    #region Private

    private void InitializeClassicMode()
    {
        _livesPanelController.Init();
        _timerPanelController.gameObject.SetActive(false);
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;
    }

    private void InitializeRushMode()
    {
        _timerPanelController.Init();
        _livesPanelController.gameObject.SetActive(false);
        GameplayEventsDispatcher.Instance.OnTimeUp += OnTimeUpHandler;
    }

    #endregion


    #region Private

    private void StartPlaying()
    {
        _isGameOver = false; 

        _spwanController.Init(_selectedGameMode, _selectedChromiez, 1);

        if (_selectedGameMode == eGameMode.Rush)
        { 
           _timerPanelController.StartTimer();
        }
    }

    private void FinishGame()
    {
        if (_isGameOver)
        {
            return;
        }

        _isGameOver = true;
        Debug.Log("Game over");

        IFlow flowManager = ComponentFactory.GetAComponent<IFlow>();
        if (flowManager != null)
        {
            flowManager.FinishGame(_gameplayTrackingData);
        }
    }

    private void ChromieCollected(ChromieController chromieController, ColorZoneController colorZone)
    {

        chromieController.CollectChromie();

        if (colorZone != null)
        {
            colorZone.CollectChromie(chromieController);
        }

        GameplayEventsDispatcher.SendChromieCollected(chromieController);

        int currentScore = this.gameObject.GetComponent<ScoreCounterManager>().Score;

        _gameplayTrackingData.Score = currentScore;

        if (_currentLevel < _levelRequierments.Length)
        {
            int currentLevelRequirment = _levelRequierments[_currentLevel];
            if (currentScore > currentLevelRequirment)
            {
                _currentLevel++;
                _spwanController.UpdateLevel(_currentLevel);
            }
        }
    }

    #endregion


    #region Events

    private void OnChromieHitColorZoneHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.ChromieType == colorZone.ColorZoneType)
        {
            ChromieCollected(chromieController, colorZone);
        }
    }

    private void OnGameOverHandler()
    {
        FinishGame();
    }

    private void OnTimeUpHandler()
    {
        _spwanController.StopSpwaning();
        if (_timeUpMessage != null)
        {
            _timeUpMessage.PlayMessageAnimation(() =>
            {
                FinishGame();
            });
        }
    }



    #endregion

}
