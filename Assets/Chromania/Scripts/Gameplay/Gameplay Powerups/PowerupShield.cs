using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupShield : PowerupBase
{
    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.SetImmune(true);

            yield return Timing.WaitForSeconds(Duration);

            livesManager.SetImmune(false);
        }
    }

}
