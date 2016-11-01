using UnityEngine;
using System.Collections;
using System;


public class PowerupAllSameColor : PowerupBase {

    public float Duration = 5.0f;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        // make all same color
        PlayPowerup(Duration, () =>
        {

        });
    }

    protected override void StopPowerupInternal()
    {
      // make all original colors
    }


   
  
}
