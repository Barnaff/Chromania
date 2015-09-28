using UnityEngine;
using System.Collections;

public delegate void StartActivePowerupDelegate(ActivePowerupType powerupType);

public class PowerupsManager : MonoBehaviour {

    public bool EnablePowerups;

    public StartActivePowerupDelegate OnStartActivePowerup;

    private Hashtable _colorsCounts;

	// Use this for initialization
	void Start () {
        _colorsCounts = new Hashtable();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Public

    public void ChromiesSpwaned(ChromieController chromie)
    {
        if (!EnablePowerups)
        {
            return;
        }

        if (_colorsCounts.ContainsKey(chromie.ChromieType))
        {
            _colorsCounts[chromie.ChromieType] = (int)_colorsCounts[chromie.ChromieType] + 1;
        }
        else
        {
            _colorsCounts.Add(chromie.ChromieType, 1);
        }

        if ((int)_colorsCounts[chromie.ChromieType] == chromie.ChromieData.CountForPowerup && chromie.ChromieData.CountForPowerup > 0)
        {
            ChromiePowerupController powerupController = chromie.GetComponent<ChromiePowerupController>() as ChromiePowerupController;
            if (powerupController != null)
            {
                powerupController.EnablePowerup();
                _colorsCounts = new Hashtable();
            }
        }
    }

    public void ChromieHitColorZone(ChromieController chromie)
    {
        if (!EnablePowerups)
        {
            return;
        }

        ActivatePowerupForChromie(chromie);
    }

    public void ActivatePssive(ChromieDataItem chromieData)
    {
        Debug.Log("activate passive: " + chromieData.PassivePowerup);
        switch (chromieData.PassivePowerup)
        {
            case PassivePowerupType.ExtraLifeSlot:
                {
                    ActivatePassiveExtraLifeSlot(chromieData.PassivePowerupValue);
                    break;
                }
            case PassivePowerupType.ExtraTime:
                {
                    ActivatePassiveExtraTime(chromieData.PassivePowerupValue);
                    break;
                }
            case PassivePowerupType.EnlargeColorZones:
                {
                    ActivateEnlargeColorsZones(chromieData.PassivePowerupValue);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    #endregion


    #region Private


    private void ActivatePowerupForChromie(ChromieController chromie)
    {
        ActivePowerupType powerupType = chromie.ChromieData.ActivePowerup;
        if (OnStartActivePowerup != null)
        {
            OnStartActivePowerup(powerupType);
        }

        switch (powerupType)
        {
            case ActivePowerupType.ExtraLife:
                {
                    AddExtraLife();
                    break;
                }
            case ActivePowerupType.ExtraTime:
                {
                    AddExtraTime();
                    break;
                }
            case ActivePowerupType.SameColor:
                {
                    StartCoroutine(ActivateSameColorPowerup(chromie.ChromieData.ChromieColor, chromie.ChromieData.PowerupDuration));
                    break;
                }
            case ActivePowerupType.ScoreMultiplier:
                {
                    ScoreMultipliyer(chromie.ChromieData.ActivePowerupValue, chromie.ChromieData.PowerupDuration);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    #endregion


    #region Passive Powerups

    private void ActivatePassiveExtraLifeSlot(float value)
    {
        LivesPanelController livesPanel = GameObject.FindObjectOfType<LivesPanelController>() as LivesPanelController;
        if (livesPanel != null)
        {
            livesPanel.AddExtraLifeSlot();
        }
    }

    private void ActivatePassiveExtraTime(float value)
    {
        TimerPanelController timerPanel = GameObject.FindObjectOfType<TimerPanelController>() as TimerPanelController;
        if (timerPanel != null)
        {
            timerPanel.AddTime(value);
        }
    }

    private void ActivateEnlargeColorsZones(float value)
    {
        ColorZonesManager colorsZonesManager = GameObject.FindObjectOfType<ColorZonesManager>() as ColorZonesManager;
        if (colorsZonesManager != null)
        {
            colorsZonesManager.EnlargeColorZones(value);
        }
    }

    #endregion


    #region Activated Powerups Effects

    private void AddExtraLife()
    {
        LivesPanelController livesPanel = GameObject.FindObjectOfType<LivesPanelController>() as LivesPanelController;
        if (livesPanel != null)
        {
            livesPanel.AddExtraLife();
        }
    }

    private void AddExtraTime()
    {


    }

    private void ScoreMultipliyer(float value, float duration)
    {
        ScoreManager scoreManager = GameObject.FindObjectOfType<ScoreManager>() as ScoreManager;
        if (scoreManager != null)
        {
            scoreManager.SetScoreMultiplier((int)value, duration);
        }
    }

    IEnumerator ActivateSameColorPowerup(ColorType color, float duration)
    {
        SpwanManager spwanManager = GameObject.FindObjectOfType<SpwanManager>() as SpwanManager;
        ColorZonesManager colorZonesManager = GameObject.FindObjectOfType<ColorZonesManager>() as ColorZonesManager;

        if (spwanManager != null && colorZonesManager != null)
        {
            spwanManager.MakeAllSpwansSameColor(color);
        }


        yield return new WaitForSeconds(duration);

        spwanManager.ReturnToOriginalSelectedColors();

        yield return null;
    }

    #endregion


   
}
