using UnityEngine;
using System.Collections;
using System;

public class PowerupComboMultiplier : PowerupBase
{
    public int ComboMultiplier;

    public enum eComboMultiplierMethod
    {
        Add,
        Multiply,
    }

    public eComboMultiplierMethod Method;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddomboMultiplier(ComboMultiplier);
        }
    }

    protected override void StopPowerupInternal()
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.RemoveComboMultiplier(ComboMultiplier);
        }
    }
}
