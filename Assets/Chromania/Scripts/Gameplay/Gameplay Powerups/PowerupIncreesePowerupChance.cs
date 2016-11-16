using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupIncreesePowerupChance : PowerupBase
{
    public float SpwanChanceMultiplier;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayPowerupsManager gameplayPowerupsManager = GameObject.FindObjectOfType<GameplayPowerupsManager>();
        if (gameplayPowerupsManager != null)
        {
            gameplayPowerupsManager.AddSpwanIntervalMultiplier(SpwanChanceMultiplier);

            yield return Timing.WaitForSeconds(Duration);

            gameplayPowerupsManager.AddSpwanIntervalMultiplier(-SpwanChanceMultiplier);
        }
    }
}
