using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetupManager : FactoryComponent, IGameSetup
{
    #region Private Properties

    [SerializeField]
    private int _numberOfSlots = 4;

    [SerializeField]
    private eChromieType[] _selectedChromiez;

    [SerializeField]
    private eGameMode _selectedGameMode;

    #endregion


    #region FactoryComponent Implementation

    public override void InitComponentAtStart()
    {
        _selectedChromiez = new eChromieType[_numberOfSlots];
        for (int i = 0; i < _selectedChromiez.Length; i++)
        {
            _selectedChromiez[i] = eChromieType.None;
        }
    }

    public override void InitComponentAtAwake()
    {
       
    }

    #endregion


    #region IGameSetup Implementation

    public int AddChromie(eChromieType chromieType)
    {
        if (!IsSelected(chromieType))
        {
            for (int i = 0; i < _selectedChromiez.Length; i++)
            {
                if (_selectedChromiez[i] == eChromieType.None)
                {
                    _selectedChromiez[i] = chromieType;
                    return i;
                }
            }
        }
        return -1;
    }

    public int RemoveChromie(eChromieType chromieType)
    {
        for (int i = 0; i < _selectedChromiez.Length; i++)
        {
            if (_selectedChromiez[i] == chromieType)
            {
                _selectedChromiez[i] = eChromieType.None;
                return i;
            }
        }
        Debug.LogError("ERROR - trying to remove chromie that dosnt exist in selection");
        return -1;
    }

    public bool RemoveChromieAtIndex(int index)
    {
        if (_selectedChromiez[index] != eChromieType.None)
        {
            _selectedChromiez[index] = eChromieType.None;
            return true;
        }
        return false;
    }

    public bool IsSelected(eChromieType chromieType)
    {
        for (int i = 0; i < _selectedChromiez.Length; i++)
        {
            if (_selectedChromiez[i] == chromieType)
            {
                return true;
            }
        }
        return false;
    }

    public eChromieType[] SelectedChromiez
    {
        get
        {
            return _selectedChromiez;
        }
    }

    public bool CanAddChromie()
    {
        return IsSelected(eChromieType.None);
    }

    public eGameMode SelectedGameMode
    {
        get
        {
            return _selectedGameMode;
        }
        set
        {
            _selectedGameMode = value;
        }
    }

    #endregion


}
