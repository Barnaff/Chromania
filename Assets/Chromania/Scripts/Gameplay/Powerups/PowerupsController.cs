using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupsController : MonoBehaviour {

    #region Private Properties

    private Dictionary<eChromieType, int> _collectedColorsCounter;

    [SerializeField]
    private bool _makeAllChromiezPowerups = false;

    private Dictionary<eChromieType, PowerupActivatorController> _activePowerupups = new Dictionary<eChromieType, PowerupActivatorController>();

    #endregion

    // Use this for initialization
    void Start ()
    {
        _collectedColorsCounter = new Dictionary<eChromieType, int>();

        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
        GameplayEventsDispatcher.Instance.OnChromieSpawned += OnChromieSpwanedHandler;
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    private bool ShouldSpwanAsPowerup(ChromieController chromieController)
    {
        if (_makeAllChromiezPowerups)
        {
            return true;
        }

        if (_collectedColorsCounter.ContainsKey(chromieController.ChromieType) &&
            _collectedColorsCounter[chromieController.ChromieType] > chromieController.ChromieData.CountForPowerup)
        {
            return true;
        }

        return false;
    }


    private void ActivatePowerupFromChromie(ChromieController chromieController)
    {
        if (!_activePowerupups.ContainsKey(chromieController.ChromieType))
        {
            PowerupActivatorController activePowerupController = PowerupActivatorController.ActivatePowerup(chromieController.ChromieData);
            activePowerupController.OnFinishActivatedPowerup += OnFinishedPowerupHandler;
            _activePowerupups.Add(chromieController.ChromieType, activePowerupController);
        }
    }

    #region Events

    private void OnChromieSpwanedHandler(ChromieController chromieController)
    {
        if (ShouldSpwanAsPowerup(chromieController))
        {
            chromieController.SetAsPowerup();
        }
    }

    private void OnChromieCollectedHandler(ChromieController chromieController)
    {
        if (chromieController.IsPowerup)
        {
            if (_collectedColorsCounter.ContainsKey(chromieController.ChromieType))
            {
                _collectedColorsCounter[chromieController.ChromieType] = 0;
            }
            else
            {
                _collectedColorsCounter.Add(chromieController.ChromieType, 0);
            }

            ActivatePowerupFromChromie(chromieController);
        }
        else
        {
            if (_collectedColorsCounter.ContainsKey(chromieController.ChromieType))
            {
                _collectedColorsCounter[chromieController.ChromieType] += 1;
            }
            else
            {
                _collectedColorsCounter.Add(chromieController.ChromieType, 1);
            }
        }
    }

    private void OnFinishedPowerupHandler(PowerupActivatorController powerupActivatorController)
    {
        Debug.Log("finished powerup");
        if (_activePowerupups.ContainsKey(powerupActivatorController.PowerupChromieType))
        {
            _activePowerupups.Remove(powerupActivatorController.PowerupChromieType);
        }
    }

    #endregion



}
