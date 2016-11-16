using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupExtraTime : PowerupBase
{
    public float TimeToAdd = 5f;

    public bool AddToMaxTime = true;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayTimerManager timerManager = GameObject.FindObjectOfType<GameplayTimerManager>();
        if (timerManager != null)
        {
            timerManager.AddTime(TimeToAdd);
        }

        yield return 0f;
    }
}
