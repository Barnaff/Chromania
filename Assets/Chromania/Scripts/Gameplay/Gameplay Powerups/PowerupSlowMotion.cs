using UnityEngine;
using System.Collections;

public class PowerupSlowMotion : PowerupBase
{
    public float Duration;

    public float TimeValue = 0.5f;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        UnityStandardAssets.ImageEffects.MotionBlur motionBlur = Camera.main.gameObject.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
        if (motionBlur != null)
        {
            motionBlur.enabled = true;
        }
        Time.timeScale = TimeValue;
        PlayPowerup(Duration * TimeValue);
    }

    protected override void StopPowerupInternal()
    {
        Time.timeScale = 1f;
        UnityStandardAssets.ImageEffects.MotionBlur motionBlur = Camera.main.gameObject.GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();
        if (motionBlur != null)
        {
            motionBlur.enabled = false;
        }
    }
}


