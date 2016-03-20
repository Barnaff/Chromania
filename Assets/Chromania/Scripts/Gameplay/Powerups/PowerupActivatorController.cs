using UnityEngine;
using System.Collections;

public class PowerupActivatorController : MonoBehaviour {

    #region Public

    public delegate void FinishedActivatedPowerupDelegate(PowerupActivatorController powerupActivatorController);

    public event FinishedActivatedPowerupDelegate OnFinishActivatedPowerup;

    #endregion

    #region Private properties

    private ChromieDataObject _chromieData;

    #endregion


    #region Initialize

    public static PowerupActivatorController ActivatePowerup(ChromieDataObject chromieData)
    {
        GameObject powerupActivatorContainer = new GameObject();
        PowerupActivatorController powerupActivatorController = powerupActivatorContainer.AddComponent<PowerupActivatorController>();

        powerupActivatorContainer.name = "Powerup " + chromieData.ActivePowerup.ToString();

        powerupActivatorController.StartPowerup(chromieData);

        return powerupActivatorController;
    }

    public void StartPowerup(ChromieDataObject chromieData)
    {
        _chromieData = chromieData;
        Debug.Log("Activate powerup: " + chromieData.ActivePowerup);
        switch (chromieData.ActivePowerup)
        {
            case ePowerups.Active.AddLife:
                {
                    StartCoroutine(ActivePowerupExtraLife((int)chromieData.ActivePowerupValue));
                    break;
                }
            case ePowerups.Active.AddTime:
                {
                    StartCoroutine(ActivePowerupExtraTime(chromieData.ActivePowerupValue));
                    break;
                }
            case ePowerups.Active.DoubleScore:
                {
                    StartCoroutine(ActivePowerupScoreMultiplier((int)chromieData.ActivePowerupValue, chromieData.PowerupDuration));
                    break;
                }
            case ePowerups.Active.AllSameColor:
                {
                    StartCoroutine(ActivePowerupAllSameColor(chromieData.ChromieColor, chromieData.PowerupDuration));
                    break;
                }
        }
    }

    public void FinishPowerup()
    {
        if (OnFinishActivatedPowerup != null)
        {
            OnFinishActivatedPowerup(this);
        }
        Destroy(this.gameObject);
    }

    public eChromieType PowerupChromieType
    {
        get
        {
            return _chromieData.ChromieColor;
        }
    }

    #endregion


    #region Private - Active Powerups

    private IEnumerator ActivePowerupExtraLife(int livesToAdd = 1)
    {
        LivesPanelController livesController = GameObject.FindObjectOfType<LivesPanelController>();
        if (livesController != null)
        {
            for (int i = 0; i < livesToAdd; i++)
            {
                livesController.AddLife();
            }
        }
        yield return null;
        FinishPowerup();
    }

    private IEnumerator ActivePowerupExtraTime(float timeToAdd)
    {
        TimerPanelController timerController = GameObject.FindObjectOfType<TimerPanelController>();
        if (timerController != null)
        {
            timerController.AddTime(timeToAdd);
        }
        yield return null;
        FinishPowerup();
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
        yield return null;
        FinishPowerup();
    }

    private IEnumerator ActivePowerupScoreMultiplier(int multiplier, float duration)
    {
        ScoreCounterManager scoreCounterManager = GameObject.FindObjectOfType<ScoreCounterManager>();
        if (scoreCounterManager != null)
        {
            scoreCounterManager.AddScoreMultiplier(multiplier);
            yield return new WaitForSeconds(duration);

            scoreCounterManager.RemoveScoreMultiplier(multiplier);
        }
        yield return null;
        FinishPowerup();
    }

    #endregion

}
