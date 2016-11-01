using UnityEngine;
using System.Collections;
using System;

public class PowerupAddScoreMultiplier : PowerupBase
{
    public int SocreMultiplier;

    public float Duration;

    public enum eScoreMultiplierMethod
    {
        Add,
        Multiply,
    }

    public eScoreMultiplierMethod Method;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
      
    }

    protected override void StopPowerupInternal()
    {
      
    }
}
