using UnityEngine;
using System.Collections;
using System;

public class PowerupIncreesePowerupChance : PowerupBase
{
    public float SpwanChanceMultiplier;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayPowerupsManager gameplayPowerupsManager = GameObject.FindObjectOfType<GameplayPowerupsManager>();
        if (gameplayPowerupsManager != null)
        {
            gameplayPowerupsManager.AddSpwanIntervalMultiplier(SpwanChanceMultiplier);
        }
    }

    protected override void StopPowerupInternal()
    {
        GameplayPowerupsManager gameplayPowerupsManager = GameObject.FindObjectOfType<GameplayPowerupsManager>();
        if (gameplayPowerupsManager != null)
        {
            gameplayPowerupsManager.AddSpwanIntervalMultiplier(-SpwanChanceMultiplier);
        }
    }
}
