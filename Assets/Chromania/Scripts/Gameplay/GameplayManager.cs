using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    [SerializeField]
    private eChromieType[] _selectedChromiez;

    private SpwanerController _spwanController;

    private ColorZonesManager _colorZonesManager;

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
        StartGmwplay();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void QuitGame()
    {  
        MenuScreensManager.Instance().DisplayMenuScreen(eMenuScreenType.MainMenu); 
    }

    private void StartGmwplay()
    {
        _spwanController.Init(_selectedChromiez, 1);
        _colorZonesManager.Init(_selectedChromiez);
    }

    
}
