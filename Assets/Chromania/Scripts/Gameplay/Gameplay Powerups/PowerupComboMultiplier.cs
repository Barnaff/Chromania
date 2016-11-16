using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupComboMultiplier : PowerupBase
{
    public int ComboMultiplier;

    public enum eComboMultiplierMethod
    {
        Add,
        Multiply,
    }

    public eComboMultiplierMethod Method;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddomboMultiplier(ComboMultiplier);

            yield return Timing.WaitForSeconds(Duration);

            scoreManager.RemoveComboMultiplier(ComboMultiplier);
        }
    }
}
