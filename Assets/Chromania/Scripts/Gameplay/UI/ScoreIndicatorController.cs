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

    [SerializeField]
    private ParticleSystem[] _particleEffct;

    public void DisplayScore(int amount, Color colorEffct)
    {
        _scoreLabel.text = amount.ToString();
        if (_particleEffct != null)
        {
            for (int i = 0; i < _particleEffct.Length; i++)
            {
                Color c = colorEffct;
                c.r += (i * 0.2f);
                c.g += (i * 0.2f);
                c.b += (i * 0.2f);
                c.a = 1;
                _particleEffct[i].startColor = c;
            }
        }

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
