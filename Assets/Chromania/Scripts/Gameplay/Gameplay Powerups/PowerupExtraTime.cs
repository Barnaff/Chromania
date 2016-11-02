using UnityEngine;
using System.Collections;
using System;

public class PowerupExtraTime : PowerupBase
{
    public float TimeToAdd = 5f;

    public bool AddToMaxTime = true;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayTimerManager timerManager = GameObject.FindObjectOfType<GameplayTimerManager>();
        if (timerManager != null)
        {
            timerManager.AddTime(TimeToAdd);
        }
    }

    protected override void StopPowerupInternal()
    {
        
    }
}
