using UnityEngine;
using System.Collections;
using System;

public class PowerupEnlargeColorZones : PowerupBase
{
    public float ScaleFactor;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        ColorZoneController[] colorZoneControlerList = GameObject.FindObjectsOfType<ColorZoneController>();
        foreach (ColorZoneController colorZone in colorZoneControlerList)
        {
            colorZone.AddScaleFactor(ScaleFactor);
        }
    }

    protected override void StopPowerupInternal()
    {
        ColorZoneController[] colorZoneControlerList = GameObject.FindObjectsOfType<ColorZoneController>();
        foreach (ColorZoneController colorZone in colorZoneControlerList)
        {
            colorZone.RemoveScaleFactor(ScaleFactor);
        }
    }
}
