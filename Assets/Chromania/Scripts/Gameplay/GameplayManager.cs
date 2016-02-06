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

        _spwanController.Init(_selectedChromiez, 1);
        _colorZonesManager.Init(_selectedChromiez);

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
    }


    #region Private

    private void InitializeClassicMode()
    {
        _timerPanelController.gameObject.SetActive(false);
        _livesPanelController.gameObject.SetActive(true);
    }

    private void InitializeRushMode()
    {
        _timerPanelController.gameObject.SetActive(true);
        _livesPanelController.gameObject.SetActive(false);
    }

    #endregion

}
