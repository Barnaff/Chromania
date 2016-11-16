using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PowerupExtraLife : PowerupBase {

    public int NumberOfExtraLives = 1;

    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.AddLife(NumberOfExtraLives);
        }

        yield return 0f;
    }

  
}
