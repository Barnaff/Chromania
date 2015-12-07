using UnityEngine;
using System.Collections;

public class GameplayBase : MonoBehaviour {

	#region Protected Properties

	protected GameDefenitionDataModel _gameDefenition = null;

	[SerializeField]
	protected int _score;

	[SerializeField]
	protected GameplayHUDController _gameHUD;

	protected bool _isGameOver = false;

	protected bool _isPaused = false;

	#endregion


	#region Private

	private int _keepPlayingCount;

	private bool _isIniitialized;

	private const string HUD_RESOURCE_PATH = "Prefabs/Gameplay HUD";

	[SerializeField]
	private bool _enableCoinsInGame;

	[SerializeField]
	private bool _editMode;

	[SerializeField]
	protected AudioClip[] _gameSounds;

	[SerializeField]
	protected AudioClip _backgroundMusic;

	[SerializeField]
	private string _gameVaribales;

	private enum BackButtonGameplayActionType
	{
		Quit,
		Pause,
	}

	[SerializeField]
	private BackButtonGameplayActionType _backButtonGameplayAction;



	#endregion

	IEnumerator Start()
	{
		yield return null;
		PlayBackgrounMusic();

		yield return new WaitForSeconds(3.0f);
		if (_gameDefenition == null)
		{
			if (!_isIniitialized && _editMode)
			{
				Init();
			}
			StartGame(null);
		}

		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, 
		                           AnalyticsEvents.ANALYTICS_CATEGORY_GAME, 
		                           AnalyticsEvents.ANALYTICS_EVENT_GAME_STARTED,
		                           Application.loadedLevelName);

		IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
		if (backButtonManager != null)
		{
			backButtonManager.RegisterToBackButton(BackButtonAction);
		}

	}

	void OnDisable()
	{
		GameplayWrapper.CommitCoinsChange();

		AnalyticsUtil.SendScreenHit(AnalyticsServiceType.GoogleAnalytics, 
		                           AnalyticsEvents.ANALYTICS_SCREEN_GAME + Application.loadedLevelName);

		IBackButton backButtonManager = ComponentFactory.GetAComponent<IBackButton>();
		if (backButtonManager != null)
		{
			backButtonManager.RemoveResponderFromBackButton(BackButtonAction);
		}
	}

	private void BackButtonAction()
	{
		switch (_backButtonGameplayAction)
		{
		case BackButtonGameplayActionType.Pause:
		{
			if (!_isPaused)
			{
				OnHUDPauseHandler();
			}
			break;
		}
		case BackButtonGameplayActionType.Quit:
		default:
		{
			OnHUDQuitGameHandler();
			break;
		}
		}

	}

	#region Initialization

	public void Initialize()
	{
		_isIniitialized = true;
		_editMode = false;
		Init();
	}

	public void StartGame(GameDefenitionDataModel gameDefenition)
	{
		_gameDefenition = gameDefenition;
		InitilizeGameHUD();
		if (_gameDefenition != null && !string.IsNullOrEmpty(_gameDefenition.GameVariables) && _gameDefenition.GameVariables.Length > 0)
		{
			_gameVaribales = _gameDefenition.GameVariables;
		}

		_keepPlayingCount = 0;
		GameStarted(_gameVaribales);

		AnalyticsUtil.SendEventHit(AnalyticsServiceType.GoogleAnalytics, 
		                           AnalyticsEvents.ANALYTICS_CATEGORY_GAME, 
		                           AnalyticsEvents.ANALYTICS_EVENT_GAME_STARTED,
		                           gameDefenition.GameName, gameDefenition.GameId);
	}

	#endregion


	#region Subclassing

	protected virtual void Init()
	{
		
	}

	protected virtual void GameStarted(string gameVariables) 
	{ 
		_isGameOver = false;
		Time.timeScale = 1.0f;
	}

	protected virtual void GameOver() 
	{
		if (!_isGameOver)
		{
			if (_keepPlayingCount >= 1)
			{
				GameplayWrapper.GameOver(_score);
			}
			else
			{
				_gameHUD.DisplayKeepPlayingPanel();
			}
		}
		_isPaused = true;
		_isGameOver = true;	
	}

	protected virtual void KeepPlaying() 
	{ 
		_isGameOver = false;	
	}

	protected virtual void GamePaused() { }

	protected virtual void GameResumed() { }

	protected virtual void QuitGame() { }

	protected virtual void AddScore(int score)
	{
		_score += score;
		if (_gameHUD != null)
		{
			_gameHUD.UpdateScore(_score);
		}
	}
	

	#endregion


	#region Private

	private void InitilizeGameHUD()
	{
		GameObject gameHUDContainer = Instantiate(Resources.Load(HUD_RESOURCE_PATH)) as GameObject;
		_gameHUD = gameHUDContainer.GetComponent<GameplayHUDController>() as GameplayHUDController;
		if (_gameHUD != null)
		{
			_gameHUD.OnHUDPause += OnHUDPauseHandler;
			_gameHUD.OnHUDKeepPlaying += OnHUDKeepPlayingHandler;
			_gameHUD.OnHUDFinishPlaying += OnHUDFinishPlayingHandler;
			_gameHUD.OnHUDResume += OnHUDResumeHandler;
			_gameHUD.OnHUDQuitGame += OnHUDQuitGameHandler;
			_gameHUD.OnHUDRestartGame += OnHUDRestartGameHandler;

			if (_enableCoinsInGame)
			{
				_gameHUD.EnableCoinsPanel();
			}
			else
			{

			}
		}
		else
		{
			Debug.LogError("ERROR - Game HUD Could not be found!");
		}
	}

	#endregion


	#region HUD Events

	private void OnHUDPauseHandler()
	{
		_isPaused = true;
		Time.timeScale = 0;
		GamePaused();
		_gameHUD.DisplayPausePanel();
	}

	private void OnHUDResumeHandler()
	{
		_isPaused = false;
		Time.timeScale = 1.0f;
		GameResumed();
		_gameHUD.HidePausePanel();
	}

	private void OnHUDKeepPlayingHandler()
	{
		// keep palying price, back to game;
		_keepPlayingCount++;
		_gameHUD.HideKeepPlayingPanel();
		KeepPlaying();
	}

	private void OnHUDFinishPlayingHandler()
	{
		Time.timeScale = 1;
		GameplayWrapper.GameOver(_score);
	}

	private void OnHUDQuitGameHandler()
	{
		_isGameOver = true;
		Time.timeScale = 1;
		Application.LoadLevel("Lobby Scene");
	}

	private void OnHUDRestartGameHandler()
	{
		_isPaused = true;
		_isGameOver = true;
		GameLoaderUtil.LoadGame(_gameDefenition, false);
	}

	#endregion


	#region Unit Events

	void OnApplicationPause(bool pauseStatus) 
	{
		if (!_isPaused && pauseStatus)
		{
			GamePaused();
		}
	}

	#endregion


	#region Wallet control

	protected bool PayCoins(int coinsAmount)
	{
		if (_editMode)
		{
			return true;
		}

		return GameplayWrapper.PayCoins(coinsAmount);
	}

	protected void AddCoins(int coinsAmount)
	{
		GameplayWrapper.AddCoins(coinsAmount);
	}

	protected bool CanPayAmount(int coinsAmount)
	{
		if (_editMode)
		{
			return true;
		}
		return GameplayWrapper.CanPayCoins(coinsAmount);
	}

	#endregion


	#region Sounds

	protected void PlaySoundFX(string soundName, GameObject source = null, bool loop = false, float delay = 0, float volume = float.MaxValue, float pitch = float.MaxValue)
	{
		AudioClip audioClip = GetSound(soundName);

		PlaySoundFX(audioClip, source, loop, delay, volume, pitch);
	}

	protected void PlaySoundFX(AudioClip audioClip, GameObject source = null, bool loop = false, float delay = 0, float volume = float.MaxValue, float pitch = float.MaxValue)
	{
		if (audioClip != null)
		{
			if (source == null)
			{
				source = this.gameObject;
			}
			SoundManager.PlaySFX(source, audioClip, loop, delay, volume, pitch);
		}
	}

	private AudioClip GetSound(string soundName)
	{
		if (_gameSounds != null)
		{
			for (int i=0; i< _gameSounds.Length ; i++)
			{
				AudioClip audioClip = _gameSounds[i];
				if (audioClip.name == soundName)
				{
					return audioClip;
				}
			}
		}
		Debug.LogError("ERROR - Sound " + soundName + " could'nt be found!");
		return null;
	}

	private void PlayBackgrounMusic()
	{
		Debug.Log("play background music");

		if (_backgroundMusic != null)
		{
			SoundManager.Play(_backgroundMusic, true);

		}
	}

	protected void StopSoundFX(GameObject source = null)
	{
		if (source == null)
		{
			source = this.gameObject;
		}
		SoundManager.StopSFXObject(source);
	}


	#endregion


}
