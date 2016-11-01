using UnityEngine;
using System.Collections;

public class PowerupExtraLifeSlot : PowerupBase {

    public int NumberOfExtraLivesSlots = 1;


    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.AddLifeSlot(NumberOfExtraLivesSlots);
        }
    }

    protected override void StopPowerupInternal()
    {

    }
}
