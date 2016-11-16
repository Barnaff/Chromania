using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public class PowerupAllSameColor : PowerupBase
{
    protected override IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator)
    {
        if (cheomieControllerActivator != null)
        {
            SpwanerController spwanerController;
            ColorZonesController colorZonesController;

            ChromieController[] chromiesControllers = GameObject.FindObjectsOfType<ChromieController>();
            for (int i = 0; i < chromiesControllers.Length; i++)
            {
                if (chromiesControllers[i].gameObject.activeSelf)
                {
                    chromiesControllers[i].SwitchChromie(cheomieControllerActivator.ChromieDefenition);
                }
            }

            spwanerController = GameObject.FindObjectOfType<SpwanerController>();
            spwanerController.SetSpwanColorOverride(cheomieControllerActivator.ChromieType);

            colorZonesController = GameObject.FindObjectOfType<ColorZonesController>();
            colorZonesController.OverrideColorZonesColor(cheomieControllerActivator.ChromieDefenition);

            yield return Timing.WaitForSeconds(Duration);

            spwanerController.SetSpwanColorOverride(eChromieType.None);
            colorZonesController.ResetColorZonesToOriginalColor();
        }
    }

}
