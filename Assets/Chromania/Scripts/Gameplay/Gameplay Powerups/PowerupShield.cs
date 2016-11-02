using UnityEngine;
using System.Collections;
using System;

public class PowerupShield : PowerupBase
{
    public float Duration;

    public GameObject ShieldEffect;

    private GameObject _shieldEffectInstance;

    protected override void StartPowerupInternal(ChromieController chromieController)
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.SetImmune(true);
        }

        if (ShieldEffect != null && _shieldEffectInstance == null)
        {
            _shieldEffectInstance = Instantiate(ShieldEffect) as GameObject;
        }
        PlayPowerup(Duration);
    }

    protected override void StopPowerupInternal()
    {
        GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
        if (livesManager != null)
        {
            livesManager.SetImmune(false);
        }

        if (_shieldEffectInstance != null)
        {
            Destroy(_shieldEffectInstance);
        }
    }
}
