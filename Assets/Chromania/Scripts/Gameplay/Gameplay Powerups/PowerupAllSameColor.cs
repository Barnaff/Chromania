using UnityEngine;
using System.Collections;
using System;


public class PowerupAllSameColor : PowerupBase {

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

            ColorZonesController colorZonesController = GameObject.FindObjectOfType<ColorZonesController>();
            if (colorZonesController != null)
            {
                colorZonesController.OverrideColorZonesColor(chromieController.ChromieDefenition);
            }
        }
    }

    protected override void StopPowerupInternal()
    {
        SpwanerController spwanerController = GameObject.FindObjectOfType<SpwanerController>();
        if (spwanerController != null)
        {
            spwanerController.SetSpwanColorOverride(eChromieType.None);
        }

        ColorZonesController colorZonesController = GameObject.FindObjectOfType<ColorZonesController>();
        if (colorZonesController != null)
        {
            colorZonesController.ResetColorZonesToOriginalColor();
        }
    }


   
  
}
