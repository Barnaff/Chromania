using UnityEngine;
using System.Collections;
using System;

public class PowerupShield : PowerupBase
{
    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.SetImmune(true);
        }
    }

    protected override void StopPowerupInternal()
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.SetImmune(false);
        }
    }
}
