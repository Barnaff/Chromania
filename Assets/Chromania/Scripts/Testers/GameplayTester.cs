using UnityEngine;
using System.Collections;

public class GameplayTester : MonoBehaviour {

    [SerializeField]
    private bool _godMode;

    [SerializeField]
    private PowerupBase[] _powerupsAssets;


    [SerializeField]
    private ChromieController _selectedChromieController;

#if UNITY_EDITOR

    void Start()
    {
        _powerupsAssets = Resources.LoadAll<PowerupBase>("Powerups/");
        GameplayEventsDispatcher.Instance.OnChromieSpawned += OnChromieSpwanedHandler;
    }


    void OnGUI()
    {
        for (int i = 0; i < _powerupsAssets.Length; i++)
        {
            string powerupName = _powerupsAssets[i].name;
            if (GUILayout.Button(powerupName))
            {
                _powerupsAssets[i].StartPowerup(_selectedChromieController);
            }
        }
    }



    void Update()
    {
        if (_godMode)
        {
            GameplayLivesManager livesManager = GameObject.FindObjectOfType<GameplayLivesManager>();
            if (livesManager != null)
            {
                Destroy(livesManager);
            }

            GameplayTimerManager timerManager = GameObject.FindObjectOfType<GameplayTimerManager>();
            if (timerManager != null)
            {
                Destroy(timerManager);
            }
        }
    }


    private void OnChromieSpwanedHandler(ChromieController chromieController)
    {
        _selectedChromieController = chromieController;
    }

#endif

}
