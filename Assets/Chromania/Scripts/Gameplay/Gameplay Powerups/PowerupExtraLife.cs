using UnityEngine;
using System.Collections;
using System;

public class PowerupExtraLife : PowerupBase {


    public int NumberOfExtraLives = 1;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.AddLife(NumberOfExtraLives);
        }
    }

    protected override void StopPowerupInternal()
    {
        
    }
}
