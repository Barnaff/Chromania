using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

	#region Public Properties

	public LivesPanelController LivesController;

    public TimerPanelController TimerController;

	public GameOverPanelController GameOverController;

	public GameModeType GameMode;

	public CameraController MainCamera;

	#endregion


	#region Private Properties

	private SpwanManager _spwanManager;

	private ScoreManager _scoremanager;

    private PowerupsManager _powerupsManager;

	private bool _isGameOver;

	#endregion


	#region Initialize

	// Use this for initialization
	void Start () {
	
		_spwanManager = this.gameObject.GetComponent<SpwanManager>() as SpwanManager;
		_spwanManager.OnChromieCollected += OnchromieCollectedHandler;
        _spwanManager.OnChromieSpwaned += OnChromieSpwanedHanler;
        _scoremanager = this.gameObject.GetComponent<ScoreManager>() as ScoreManager;
        _powerupsManager = this.gameObject.GetComponent<PowerupsManager>() as PowerupsManager;
        
        IGameSetup gameSetupmanager = ComponentsFactory.GetAComponent<IGameSetup>() as IGameSetup;
		if (gameSetupmanager != null)
		{
			GameMode = gameSetupmanager.SelectedGameMode;
		}

		switch(GameMode)
		{
		case GameModeType.Classic:
		{
			EnableClassicMode();
			break;
		}
		case GameModeType.Rush:
		{
			EnableRushMode();
			break;
		}
		}

        if (gameSetupmanager != null)
        {
            ColorType[] selectedColors = gameSetupmanager.SelectedColors;
            GameInitiator.StartGameInit(selectedColors, GameMode, () =>
            {
                _spwanManager.Paused = false;
                if (GameMode == GameModeType.Rush)
                {
                    if (TimerController != null)
                    {
                        TimerController.StartTimer();
                    }
                }
            });
        }
       
	}

	#endregion


	#region Update
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion


	#region User Actions

	public void PauseButtonAction()
	{
		GameOver();
	}

	#endregion


	#region Game Setup

	private void EnableClassicMode()
	{
		if (_spwanManager != null)
		{
			_spwanManager.OnChromieDropped += OnChromieDropped;
		}

		if (LivesController != null)
		{
            TimerController.gameObject.SetActive(false);
            LivesController.gameObject.SetActive(true);
            LivesController.EnableLives();
			LivesController.OnGameOver += GameOver;
		}
	}

	private void EnableRushMode()
	{
        if (_spwanManager != null)
        {
            _spwanManager.OnChromieDropped += OnChromieDropped;
        }
       
        if (TimerController != null)
        {
            TimerController.gameObject.SetActive(true);
            LivesController.gameObject.SetActive(false);
            TimerController.EnableTimer();
            TimerController.OnGameOver += GameOver;
        }
    }

	#endregion


	#region Game Phases

	private void GameOver()
	{
		_isGameOver = true;
        _spwanManager.Paused = true;

		if (GameOverController != null)
		{
			GameOverController.GameOver(()=>
			                            {
				// end game
				_scoremanager.EndGame();
				IScreensFlow screenFlowManager = ComponentsFactory.GetAComponent<IScreensFlow>() as IScreensFlow;
				if (screenFlowManager != null)
				{
					screenFlowManager.DisplayEndGameScreen();
				}
			}, ()=>
			{
				// keep playing
			});
		}

	}

	#endregion


	#region Events

	private void OnchromieCollectedHandler(ChromieController chromieController)
	{
		_scoremanager.CollectedChromie(chromieController);
        ChromiePowerupController powerupController = chromieController.gameObject.GetComponent<ChromiePowerupController>() as ChromiePowerupController;
        if (powerupController != null)
        {
            if (powerupController.PowerupEnabled)
            {
                _powerupsManager.ChromieHitColorZone(chromieController);
                //powerupController.ActivatePowerup();
            }
        }

       
    }

	private void OnChromieDropped(ChromieController chromieController)
	{
		if (!_isGameOver)
		{
			LivesController.ReduceLife();
			LivesController.DisplayHit(chromieController.gameObject.transform.position);
			if (MainCamera != null)
			{
				MainCamera.Shake();
			}
		}
	}

    private void OnChromieSpwanedHanler(ChromieController chromie)
    {
        if (_powerupsManager != null)
        {
            _powerupsManager.ChromiesSpwaned(chromie);

        }
    }

    #endregion
}
