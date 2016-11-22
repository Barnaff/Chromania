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
    private GameplayGameOverManager _gameOverManager;

    [SerializeField]
    private bool _isGameOver;

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
        _gameplayTrackingData.GameplayMode = GameSetupManager.Instance.SelectedPlayMode;
        GameSetupManager.Instance.Init();
        yield return 0f;
        PrepareGame();
    }

    private void PrepareGame()
    {
        // create color zones
        _colorZonesController.CreateColorZones(GameSetupManager.Instance.SelectedChromiez);
        // init the spwaner
        _spwanerController.Init(GameSetupManager.Instance.SelectedPlayMode, GameSetupManager.Instance.SelectedChromiez);
        // init the game over manager
        _gameOverManager.Init(_gameplayTrackingData);

        GameplayEventsDispatcher.Instance.OnChromieHitColorZone += OnChromieHitColorZoneHandler;
        

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

    #endregion


    #region Events

    private void OnChromieHitColorZoneHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.ChromieType == colorZone.Color)
        {
            ChromieCollected(chromieController, colorZone);
        }
    }

    #endregion

}
