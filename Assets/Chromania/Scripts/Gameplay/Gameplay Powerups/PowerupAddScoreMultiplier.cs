using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class PowerupAddScoreMultiplier : PowerupBase
{
    public int SocreMultiplier;

    public enum eScoreMultiplierMethod
    {
        Add,
        Multiply,
    }

    public eScoreMultiplierMethod Method;


    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScoreMultiplier(SocreMultiplier);

            yield return Timing.WaitForSeconds(Duration);

            scoreManager.RemoveScoreMultiplier(SocreMultiplier);
        }
    }
}
