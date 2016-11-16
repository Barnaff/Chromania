using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupExtraLifeSlot : PowerupBase {

    public int NumberOfExtraLivesSlots = 1;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.AddLifeSlot(NumberOfExtraLivesSlots);
        }

        yield return 0f;
    }

}
