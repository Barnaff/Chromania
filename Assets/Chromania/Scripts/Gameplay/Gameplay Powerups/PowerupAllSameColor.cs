using UnityEngine;
using System.Collections;
using System;


public class PowerupAllSameColor : PowerupBase {

    public float Duration = 5.0f;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        if (chromieController != null)
        {
            ChromieController[] chromiesControllers = GameObject.FindObjectsOfType<ChromieController>();
            for (int i = 0; i < chromiesControllers.Length; i++)
            {
                if (chromiesControllers[i].gameObject.activeSelf)
                {
                    chromiesControllers[i].SwitchChromie(chromieController.ChromieDefenition);
                }
            }

            SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();
            if (spwanerController != null)
            {
                spwanerController.SetSpwanColorOverride(chromieController.ChromieType);
            }

            PlayPowerup(Duration);
        }
      
    }

    protected override void StopPowerupInternal()
    {
        SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();
        if (spwanerController != null)
        {
            spwanerController.SetSpwanColorOverride(eChromieType.None);
        }
    }


   
  
}
