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
    private ScorePanelController _scorePanelController;

    // Use this for initialization
    void Start () {

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
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;
        InitializeGameplay();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void QuitGame()
    {  
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu); 
    }

    private void InitializeGameplay()
    {
        IGameSetup gameSetupManager = ComponentFactory.GetAComponent<IGameSetup>();
        if (gameSetupManager != null)
        {
            _selectedChromiez = gameSetupManager.SelectedChromiez;
            _selectedGameMode = gameSetupManager.SelectedGameMode;
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

        _colorZonesManager.Init(_selectedChromiez);

        // TODO: display game intro, then start everything
        StartPlaying();
    }


    #region Private

    private void InitializeClassicMode()
    {
        _livesPanelController.Init();
        _timerPanelController.gameObject.SetActive(false);
    }

    private void InitializeRushMode()
    {
        _timerPanelController.Init();
        _livesPanelController.gameObject.SetActive(false);
    }

    #endregion


    #region Private

    private void StartPlaying()
    {
        _spwanController.Init(_selectedChromiez, 1);
        _timerPanelController.StartTimer();
    }

    #endregion


    #region Events

    private void OnChromieHitColorZoneHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.ChromieType == colorZone.ColorZoneType)
        {
            chromieController.CollectChromie();

            if (colorZone != null)
            {
                colorZone.CollectChromie(chromieController);
            }
        }
    }

    private void OnGameOverHandler()
    {
        Debug.Log("Game over");
    }

    #endregion

}
