using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class ScoreIndicatorController : MonoBehaviour {

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private float _lifetime = 3.0f;

    public void DisplayScore(int amount)
    {
        _scoreLabel.text = amount.ToString();
        Timing.RunCoroutine(DelayRemove(_lifetime));
    }

    IEnumerator <float> DelayRemove(float duration)
    {
        yield return Timing.WaitForSeconds(duration);

        if (this.gameObject != null)
        {
            Lean.LeanPool.Despawn(this.gameObject);
        } 
    }
}
