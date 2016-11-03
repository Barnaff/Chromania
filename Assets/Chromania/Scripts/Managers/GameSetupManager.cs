using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetupManager : Kobapps.Singleton<GameSetupManager> {

    #region Private Properties

    [SerializeField]
    private eGameplayMode _selectedGameplayMode;

    [SerializeField]
    private List<ChromieDefenition> _selectedChromiez;

    [SerializeField]
    private bool _displayTutorial;

    private const string SELECTED_CHROMIEZ_PREFS = "selectedChromiez";
    private const string SELECTED_GAMEPLAY_MODE_PREFS = "selectedGameplayMode";

    private bool _isInitialized = false;

    #endregion


    #region Initialization

    void Awake()
    {
        if (!_isInitialized)
        {
            Init();
        }
    }

    #endregion


    #region Public

    public void Init()
    {
        if (_isInitialized)
        {
            return;
        }
        if (PlayerPrefsUtil.HasKey(SELECTED_CHROMIEZ_PREFS))
        {
            List<eChromieType> selectedChromiezColors = (List<eChromieType>)PlayerPrefsUtil.GetObject(SELECTED_CHROMIEZ_PREFS);
            _selectedChromiez = new List<ChromieDefenition>();
            for (int i = 0; i < selectedChromiezColors.Count; i++)
            {
                _selectedChromiez.Add(ChromezData.Instance.GetChromie(selectedChromiezColors[i]));
            }
        }
        else
        {
            _selectedChromiez = new List<ChromieDefenition>();
            for (int i=0; i < GameplaySettings.Instance.NumberOfChromiezSlots; i++)
            {
                _selectedChromiez.Add(null);
            }
        }

        if (PlayerPrefsUtil.HasKey(SELECTED_GAMEPLAY_MODE_PREFS))
        {
            _selectedGameplayMode = (eGameplayMode)PlayerPrefsUtil.GetInt(SELECTED_GAMEPLAY_MODE_PREFS);
        }
        _isInitialized = true;
    }

    public eGameplayMode SelectedPlayMode
    {
        set
        {
            if (!_isInitialized)
            {
                Init();
            }
            _selectedGameplayMode = value;
        }
        get
        {
            if (!_isInitialized)
            {
                Init();
            }
            return _selectedGameplayMode;
        }
    }

    public List<ChromieDefenition> SelectedChromiez
    {
        set
        {
            _selectedChromiez = value;
        }
        get
        {
            if (!_isInitialized)
            {
                Init();
            }
            return _selectedChromiez;
        }
    }

    public List<eChromieType> SelectedChromiezColorsList
    {
        get
        {
            List<eChromieType> chromiezColors = new List<eChromieType>();
            for (int i=0; i< _selectedChromiez.Count; i++)
            {
                chromiezColors.Add(_selectedChromiez[i].ChromieColor);
            }
            return chromiezColors;
        }
    }

    public void SaveSettings()
    {
        if (_selectedChromiez != null && _selectedChromiez.Count > 0)
        {
            List<eChromieType> selectedChromiezColors = new List<eChromieType>();
            for (int i=0; i< _selectedChromiez.Count; i++)
            {
                if (_selectedChromiez[i] == null)
                {
                    selectedChromiezColors.Add(eChromieType.None);
                }
                else
                {
                    selectedChromiezColors.Add(_selectedChromiez[i].ChromieColor);
                }
            }
            PlayerPrefsUtil.SetObject(SELECTED_CHROMIEZ_PREFS, selectedChromiezColors);
        }

        PlayerPrefsUtil.SetInt(SELECTED_GAMEPLAY_MODE_PREFS, (int)_selectedGameplayMode);
    }

    public bool TryAddSelection(ChromieDefenition chromieDefenition)
    {
        for (int i = 0; i < _selectedChromiez.Count; i++)
        {
            if (_selectedChromiez[i] != null && _selectedChromiez[i].ChromieColor == chromieDefenition.ChromieColor)
            {
                return false;
            }
        }

            for (int i = 0; i < _selectedChromiez.Count; i++)
        {
            if (_selectedChromiez[i] == null || (_selectedChromiez[i] != null && _selectedChromiez[i].ChromieColor == eChromieType.None))
            {
                _selectedChromiez[i] = chromieDefenition;
                SaveSettings();
                return true;
            }
        }
        return false;
    }

    public bool TryRemoveSelection(ChromieDefenition chromieDefenition)
    {
        for (int i = 0; i < _selectedChromiez.Count; i++)
        {
            if (_selectedChromiez[i] != null && _selectedChromiez[i].ChromieColor == chromieDefenition.ChromieColor)
            {
                _selectedChromiez[i] = null;
                SaveSettings();
                return true;
            }

        }
        return false;
    }

    #endregion
}
