using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupSlowMotion : PowerupBase
{
    public float TimeValue = 0.5f;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        UnityStandardAssets.ImageEffects.MotionBlur motionBlur = Camera.main.gameObject.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
        if (motionBlur != null)
        {
            motionBlur.enabled = true;
        }
        Time.timeScale = TimeValue;

        yield return Timing.WaitForSeconds(GetDuration());

        Time.timeScale = 1f;
       
        if (motionBlur != null)
        {
            motionBlur.enabled = false;
        }
    }
}


