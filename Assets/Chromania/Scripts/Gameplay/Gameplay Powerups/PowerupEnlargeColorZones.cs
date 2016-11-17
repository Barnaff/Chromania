using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupEnlargeColorZones : PowerupBase
{
    public GameplayBuffEffect BuffEffect;

    public bool IsActive = false;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        float scaleValue = BuffEffect.Value * GameplayBuffsManager.GetValue(eBuffType.PowerupEffectMultiplier);
        GameplayBuffEffect buffEffect = GameplayBuffsManager.Instance.CreateBuff(BuffEffect.Type, scaleValue, BuffEffect.Method);

        ColorZoneController[] colorZoneControlerList = GameObject.FindObjectsOfType<ColorZoneController>();
        foreach (ColorZoneController colorZone in colorZoneControlerList)
        {
            colorZone.UpdateScaleFactor();
        }

        if (IsActive)
        {
            GameplayBuffsManager.Instance.RemoveBuff(buffEffect);
            yield return Timing.WaitForSeconds(GetDuration());
            foreach (ColorZoneController colorZone in colorZoneControlerList)
            {
                colorZone.UpdateScaleFactor();
            }
        }
      
    }
}
