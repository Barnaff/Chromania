using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupComboMultiplier : PowerupBase
{
    public GameplayBuffEffect BuffEffect;

    public bool IsActive;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            GameplayBuffEffect buffEffect = GameplayBuffsManager.Instance.CreateBuff(BuffEffect);
            scoreManager.UpdateComboMultiplier();
            if (IsActive)
            {
                yield return Timing.WaitForSeconds(GetDuration());
                GameplayBuffsManager.Instance.RemoveBuff(buffEffect);
                scoreManager.UpdateComboMultiplier();
            }
          
        }
    }
}
