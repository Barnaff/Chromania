using UnityEngine;
using System.Collections;
using MovementEffects;
using System.Collections.Generic;

public abstract class PowerupBase : ScriptableObject {

    #region Public

    public void StartPowerup(ChromieController chromieController)
    {
        StartPowerupInternal(chromieController);

    }

    public void StopPowerup()
    {
        StopPowerupInternal();

        Timing.KillCoroutines(this.GetHashCode().ToString());
    }

    #endregion


    #region Protected

    protected abstract void StartPowerupInternal(ChromieController chromieController);

    protected abstract void StopPowerupInternal();


    protected void PlayPowerup(float duration, System.Action completionAction)
    {
        Timing.RunCoroutine(PlayPowerupCorutine(duration, completionAction), this.GetHashCode().ToString());
    }

    #endregion


    #region Private

    private IEnumerator<float> PlayPowerupCorutine(float duration, System.Action completionAction)
    {
        yield return Timing.WaitForSeconds(duration);

        if (completionAction != null)
        {
            completionAction();
        }

        StopPowerup();
    }

    #endregion

}
