using UnityEngine;
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

    public ePowerupActivationType ActivationType;

    public float Duration;

    public GameObject ActivationEffectPrefab;

    public GameObject DurationEffectPrefab;

    public GameObject FinishedEffectPrefab;

    private GameObject _durationEffectInstance;

    #endregion

    #region Public

    public void StartPowerup(ChromieController chromieController)
    {
        StartPowerupInternal(chromieController);

        switch (ActivationType)
        {
            case ePowerupActivationType.ContinuesSingle:
                {
                    break;
                }
            case ePowerupActivationType.SingleUse:
                {
                    break;
                }
            case ePowerupActivationType.OverTime:
                {
                    StartPowerupDurationCount(Duration);
                    break;
                }
        }

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
    }

    public void StopPowerup()
    {
        StopPowerupInternal();

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
    }

    #endregion


    #region Protected

    protected abstract void StartPowerupInternal(ChromieController chromieController);

    protected abstract void StopPowerupInternal();




    #endregion


    #region Private

    private void StartPowerupDurationCount(float duration)
    {
        Timing.RunCoroutine(PlayPowerupCorutine(duration), this.GetHashCode().ToString());
    }

    private IEnumerator<float> PlayPowerupCorutine(float duration)
    {
        yield return Timing.WaitForSeconds(duration);

        StopPowerup();
    }

    #endregion

}
