using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayPowerupsManager : MonoBehaviour {

    #region Private Properties

#if UNITY_EDITOR
    [SerializeField]
    private bool _makeAllPowerups = false;
#endif

    [SerializeField]
    private Dictionary<eChromieType, int> _collectedColorsCount;

    [SerializeField]
    private int _spwanCount;

    [SerializeField]
    private int _powerupSpwanInterval;

    [SerializeField]
    private float _powerupSpwanMultiplier = 1;

    [SerializeField]
    private List<PowerupBase> _activePowerups;

    [SerializeField]
    private GameObject _powerupActivationEffectprefab;

    #endregion


    #region Initialization

    void Start()
    {
        _spwanCount = 0;
        GameplayEventsDispatcher.Instance.OnChromieSpawned += OnChromieSpwanedhandler;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
        GameplayEventsDispatcher.Instance.OnPowerupStarted += OnPowerupStartedHandler;
        GameplayEventsDispatcher.Instance.OnPowerupStopped += OnPowerupStoppedHandler;
    }

    #endregion


    #region Public

    public void UpdatePowerupSpwanInterval()
    {
        _powerupSpwanMultiplier = GameplayBuffsManager.GetValue(eBuffType.PowerupSpwanChanceMultiplier);
    }

    #endregion


    #region Events

    private void OnChromieSpwanedhandler(ChromieController chromieController)
    {
        if (ShouldSpwanAsPwerup(chromieController))
        {
            chromieController.IsPowerup = true;
        }
    }

    private void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController.IsPowerup)
        {
            if (chromieController.ChromieDefenition.ActivePowerup != null)
            {
                ActivatePowerup(chromieController.ChromieDefenition.ActivePowerup, chromieController);

            }
        }
    }

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        if (chromieController.ChromieDefenition.DroppedPowerup != null)
        {
            ActivatePowerup(chromieController.ChromieDefenition.DroppedPowerup, chromieController);
        }
    }

    private void OnPowerupStartedHandler(PowerupBase powerup)
    {

    }

    private void OnPowerupStoppedHandler(PowerupBase powerup)
    {
        if (_activePowerups.Contains(powerup))
        {
            _activePowerups.Remove(powerup);
        }
    }

    #endregion


    #region Private 

    private void ActivatePowerup(PowerupBase powerup, ChromieController chromieController)
    {
        if (!powerup.AllowMultiple)
        {
            foreach (PowerupBase activePowerup in _activePowerups)
            {
                if (activePowerup.GetType() == powerup.GetType())
                {
                    // stop the powerup from activate, there is alrady one active!
                    Debug.Log("stopped powerup: " + powerup.name + " from activate!");
                    return;
                }
            }
        }
        PowerupBase powerupInstance = powerup.StartPowerup(chromieController);

        _activePowerups.Add(powerupInstance);

        if (_powerupActivationEffectprefab != null)
        {
            GameObject powerupEffect = Lean.LeanPool.Spawn(_powerupActivationEffectprefab, chromieController.transform.position, Quaternion.identity);
            powerupEffect.GetComponent<ParticleSystem>().startColor = chromieController.ChromieDefenition.ColorValue;
            Lean.LeanPool.Despawn(powerupEffect, 2.0f);
        }
    }

    private bool ShouldSpwanAsPwerup(ChromieController chromieController)
    {
#if UNITY_EDITOR
        if (_makeAllPowerups)
        {
            return true;
        }
#endif

        _spwanCount++;

        if (chromieController.ChromieDefenition.ActivePowerup == null)
        {
            return false;
        }

        if (_spwanCount > _powerupSpwanInterval * _powerupSpwanMultiplier)
        {
            if (Random.Range(0, _powerupSpwanInterval * _powerupSpwanMultiplier * 2f) < _spwanCount)
            {
                _spwanCount = 0;
                return true;
            }
        }
        return false;
    }

    #endregion
}
