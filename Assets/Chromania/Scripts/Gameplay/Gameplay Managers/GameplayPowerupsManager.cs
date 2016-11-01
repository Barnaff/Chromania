using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayPowerupsManager : MonoBehaviour {

    #region Private Properties

#if UNITY_EDITOR
    [SerializeField]
    private bool _makeAllPowerups = false;
#endif

    [SerializeField]
    private Dictionary<eChromieType, int> _collectedColorsCount;

    #endregion


    #region Initialization

    void Start()
    {
        GameplayEventsDispatcher.Instance.OnChromieSpawned += OnChromieSpwanedhandler;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    #endregion


    #region Events

    private void OnChromieSpwanedhandler(ChromieController chromieController)
    {
        if (ShouldSpwanAsPwerup(chromieController))
        {
            chromieController.IsPowerup = true;
        }
    }

    private void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.IsPowerup)
        {
            // activate powerup
            if (chromieController.ChromieDefenition.ActivePowerup != null)
            {
                chromieController.ChromieDefenition.ActivePowerup.StartPowerup(chromieController);
            }
        }
    }

    #endregion


    #region Private 

    private bool ShouldSpwanAsPwerup(ChromieController chromieController)
    {
#if UNITY_EDITOR
        if (_makeAllPowerups)
        {
            return true;
        }
#endif
        return false;
    }

    #endregion
}
