using UnityEngine;
using System.Collections;

public class GameplaySoundManager : MonoBehaviour {

    [SerializeField]
    private SoundDefenition _chromieSpwaned;

    [SerializeField]
    private SoundDefenition _chromieCollected;

    [SerializeField]
    private SoundDefenition _chromieDropped;

    // Use this for initialization
    void Start () {

        GameplayEventsDispatcher.Instance.OnChromieSpawned += (chromieController) =>
        {
            PlaySound(_chromieSpwaned);
        };

        GameplayEventsDispatcher.Instance.OnChromieCollected += (chromieController, colorZone) =>
        {
            PlaySound(_chromieCollected);
        };

        GameplayEventsDispatcher.Instance.OnLivesUpdate += (maxLives, currentLives, change) =>
        {
            if (change < 0)
            {
                PlaySound(_chromieDropped);
            }
        };
    }


    private void PlaySound(SoundDefenition soundDefenition)
    {
        soundDefenition.Play();
    }

}
