using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupEnlargeColorZones : PowerupBase
{
    public float ScaleFactor;

    public bool IsActive = false;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayBuffEffect buffEffect = GameplayBuffsManager.Instance.CreateBuff(eBuffType.ColorZoneSizeMultiplier, ScaleFactor * GameplayBuffsManager.GetValue(eBuffType.PowerupEffectMultiplier));

        ColorZoneController[] colorZoneControlerList = GameObject.FindObjectsOfType<ColorZoneController>();
        foreach (ColorZoneController colorZone in colorZoneControlerList)
        {
            colorZone.UpdateScaleFactor();
        }

        if (IsActive)
        {
            GameplayBuffsManager.Instance.RemoveBuff(buffEffect);
            yield return Timing.WaitForSeconds(Duration);
            foreach (ColorZoneController colorZone in colorZoneControlerList)
            {
                colorZone.UpdateScaleFactor();
            }
        }
      
    }
}
