using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

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
        GameplayEventsDispatcher.SendChromieCollected(chromieController, colorZone);
    }

    private IEnumerator<float> GameOverSequance()
    {
        _isGameOver = true;
        _spwanerController.StopSpwaning();
        switch (GameSetupManager.Instance.SelectedPlayMode)
        {
            case eGameplayMode.Classic:
                {
                    GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
                    if (livesManager != null)
                    {
                        livesManager.Active = false;
                    }
                    PopupsManager.Instance.DisplayPopup<GameOverMessagePopup>();
                    break;
                }
            case eGameplayMode.Rush:
                {
                    GameplayTimerManager timerManager = GameObject.FindObjectOfType<GameplayTimerManager>();
                    if (timerManager != null)
                    {
                        timerManager.Stop();
                    }
                    PopupsManager.Instance.DisplayPopup<TimesUpMessagePopupController>();
                    break;
                }
            case eGameplayMode.Default:
            default:
                {
                    break;
                }
        }

        yield return Timing.WaitForSeconds(5.0f);

        KeepPlayingPopupController keepPlayingPopup = PopupsManager.Instance.DisplayPopup<KeepPlayingPopupController>();
        keepPlayingPopup.OnKeepPlaying += () =>
        {
            OnKeepPlayingHandler();
        };
        keepPlayingPopup.OnCloseAction += () =>
        {
            OnKeepPlayingTImeUpOrCancelhandler();
        };

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
        Timing.RunCoroutine(GameOverSequance());
    }

    private void OnTimeUpHandler()
    {
        Timing.RunCoroutine(GameOverSequance());
    }

    private void OnKeepPlayingHandler()
    {
        _isGameOver = false;
        _spwanerController.StartSpwaning();
        switch (GameSetupManager.Instance.SelectedPlayMode)
        {
            case eGameplayMode.Classic:
                {
                    GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
                    if (livesManager != null)
                    {
                        livesManager.AddLife(1);
                        livesManager.Active = true;
                    }
                    PopupsManager.Instance.ClosePopup<GameOverMessagePopup>();
                    break;
                }
            case eGameplayMode.Rush:
                {
                    GameplayTimerManager timerManager = GameObject.FindObjectOfType<GameplayTimerManager>();
                    if (timerManager != null)
                    {
                        timerManager.AddTime(10f);
                        timerManager.Run();
                    }
                    PopupsManager.Instance.ClosePopup<TimesUpMessagePopupController>();
                    break;
                }
            case eGameplayMode.Default:
            default:
                {
                    break;
                }
        }
    }

    private void OnKeepPlayingTImeUpOrCancelhandler()
    {
        FlowManager.Instance.GameOver(_gameplayTrackingData);
    }

    #endregion

}
