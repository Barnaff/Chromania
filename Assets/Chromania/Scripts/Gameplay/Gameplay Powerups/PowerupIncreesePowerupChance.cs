using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupIncreesePowerupChance : PowerupBase
{
    public GameplayBuffEffect BuffEffect;

    public bool IsActive;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayPowerupsManager gameplayPowerupsManager = GameObject.FindObjectOfType<GameplayPowerupsManager>();
        if (gameplayPowerupsManager != null)
        {
            GameplayBuffEffect buffEffect = GameplayBuffsManager.Instance.CreateBuff(BuffEffect);
            gameplayPowerupsManager.UpdatePowerupSpwanInterval();
            if (IsActive)
            {
                yield return Timing.WaitForSeconds(GetDuration());
                GameplayBuffsManager.Instance.RemoveBuff(buffEffect);
                gameplayPowerupsManager.UpdatePowerupSpwanInterval();
            }
        }
    }
}
