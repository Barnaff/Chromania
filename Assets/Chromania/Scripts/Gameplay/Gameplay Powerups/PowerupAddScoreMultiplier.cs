using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class PowerupAddScoreMultiplier : PowerupBase
{
    public GameplayBuffEffect BuffEffect;

    public bool IsActive;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            GameplayBuffEffect buffEffectInstance = GameplayBuffsManager.Instance.CreateBuff(BuffEffect);
            scoreManager.UpdateScoreMultiplier();
            if (IsActive)
            {
                yield return Timing.WaitForSeconds(GetDuration());
                GameplayBuffsManager.Instance.RemoveBuff(buffEffectInstance);
                scoreManager.UpdateScoreMultiplier();
            }
        }
    }
}
