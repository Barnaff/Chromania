using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupsController : MonoBehaviour {

    #region Private Properties

    private Dictionary<eChromieType, int> _collectedColorsCounter;

    [SerializeField]
    private bool _makeAllChromiezPowerups = false;

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
        Debug.Log("Activate powerup: " + chromieController.ChromieData.ActivePowerup);
        switch(chromieController.ChromieData.ActivePowerup)
        {
            case ePowerups.Active.AddLife:
                {
                    ActivePowerupExtraLife((int)chromieController.ChromieData.ActivePowerupValue);
                    break;
                }
            case ePowerups.Active.AddTime:
                {
                    ActivePowerupExtraTime(chromieController.ChromieData.ActivePowerupValue);
                    break;
                }
            case ePowerups.Active.DoubleScore:
                {
                    StartCoroutine(ActivePowerupScoreMultiplier((int)chromieController.ChromieData.ActivePowerupValue, chromieController.ChromieData.PowerupDuration));
                    break;
                }
            case ePowerups.Active.AllSameColor:
                {
                    StartCoroutine(ActivePowerupAllSameColor(chromieController.ChromieType, chromieController.ChromieData.PowerupDuration));
                    break;
                }
        }


       // GameplayEventsDispatcher.SendPowerupActivation(chromieController.ChromieData.ActivePowerup, 0, 0);
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

    #endregion


    #region Private - Active Powerups

    private void ActivePowerupExtraLife(int livesToAdd = 1)
    {
        LivesPanelController livesController = GameObject.FindObjectOfType<LivesPanelController>();
        if (livesController != null)
        {
            for (int i = 0; i < livesToAdd; i++)
            {
                livesController.AddLife();
            }
        }
    }

    private void ActivePowerupExtraTime(float timeToAdd)
    {
        TimerPanelController timerController = GameObject.FindObjectOfType<TimerPanelController>();
        if (timerController != null)
        {
            timerController.AddTimer(timeToAdd);
        }
    }

    private IEnumerator ActivePowerupAllSameColor(eChromieType chromieType, float duration)
    {
        SpwanerController spwanController = GameObject.FindObjectOfType<SpwanerController>();
        if (spwanController != null)
        {
            spwanController.ChangeAllActiveChromiezToColor(chromieType);

            spwanController.SetSpwanColorOverride(chromieType);

            yield return new WaitForSeconds(duration);

            spwanController.SetSpwanColorOverride(eChromieType.None);
        }
 
    }

    private IEnumerator ActivePowerupScoreMultiplier(int multiplier, float duration)
    {
        yield return null;
    }

    #endregion
}
