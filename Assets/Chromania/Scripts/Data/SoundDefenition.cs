using UnityEngine;
using System.Collections;

public class SoundDefenition : ScriptableObject {

    public AudioClip AudioClip;

    public float MinVolume = 1.0f;
    public float MaxVolume = 1.0f;

    public float MinPitch = 1.0f;
    public float MaxPitch = 1.0f;

    public bool Looping = false;

    public float Delay = 0f;

    public void Play()
    {
        SoundManager.PlaySFX(AudioClip, Looping, Delay, Random.Range(MinVolume, MaxVolume), Random.Range(MinPitch, MaxPitch));
    }
}
