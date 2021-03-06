﻿using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public abstract class PowerupBase : ScriptableObject {

    public enum ePowerupActivationType
    {
        ContinuesSingle,
        SingleUse,
        OverTime,
    }

    #region Public Properties

    public bool AllowMultiple;

    public float Duration;

    public GameObject ActivationEffectPrefab;

    public GameObject DurationEffectPrefab;

    public GameObject FinishedEffectPrefab;

    private GameObject _durationEffectInstance;

    [SerializeField]
    public Object PowerupEffect;

    [SerializeField]
    public string Description;


    #endregion


    #region Public

    public PowerupBase StartPowerup(ChromieController chromieController)
    {
        Debug.Log("start powerup: " + this.name);
        PowerupBase powerupEffect = ScriptableObject.Instantiate(this);
        if (powerupEffect != null)
        {
            Timing.RunCoroutine(powerupEffect.PlayPowerupEffect(chromieController));
        }

        return powerupEffect;
    }

    public IEnumerator<float> PlayPowerupEffect(ChromieController chromieController)
    {
        if (ActivationEffectPrefab != null)
        {
            Instantiate(ActivationEffectPrefab);
        }

        if (DurationEffectPrefab != null)
        {
            if (_durationEffectInstance != null)
            {
                Destroy(_durationEffectInstance);
            }

            _durationEffectInstance = Instantiate(DurationEffectPrefab) as GameObject;
        }

        GameplayEventsDispatcher.SendPowerupStarted(this);

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(PowerupEffectCorutine(chromieController)));

        Timing.KillCoroutines(this.GetHashCode().ToString());

        if (_durationEffectInstance != null)
        {
            Destroy(_durationEffectInstance);
            _durationEffectInstance = null;
        }

        if (FinishedEffectPrefab != null)
        {
            Instantiate(FinishedEffectPrefab);
        }

        GameplayEventsDispatcher.SendPowerupStopped(this);

    }
    
    #endregion


    #region Protected

    protected abstract IEnumerator<float> PowerupEffectCorutine(ChromieController cheomieControllerActivator);

    protected float GetDuration()
    {
        return Duration * GameplayBuffsManager.GetValue(eBuffType.PowerupEffectMultiplier);
    }

    #endregion


   
}
