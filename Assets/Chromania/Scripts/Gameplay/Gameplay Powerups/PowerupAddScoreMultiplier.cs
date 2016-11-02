using UnityEngine;
using System.Collections;
using System;

public class PowerupAddScoreMultiplier : PowerupBase
{
    public int SocreMultiplier;

    public float Duration;

    public enum eScoreMultiplierMethod
    {
        Add,
        Multiply,
    }

    public eScoreMultiplierMethod Method;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScoreMultiplier(SocreMultiplier);
            PlayPowerup(Duration);
        }
    }

    protected override void StopPowerupInternal()
    {
        GameplayScoreManager scoreManager = GameObject.FindObjectOfType<GameplayScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.RemoveScoreMultiplier(SocreMultiplier);
        }
    }
}
