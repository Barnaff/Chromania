using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    #region Delegates

    public delegate void ChromieActionDelegate(ChromieController chromieController);
    public delegate void ChromieColorZoneActionDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void GameoverDelegate();
    public delegate void LivesUpdateDelegate(int maxLives, int currentLives, int change);
    public delegate void ScoreUpdateDelegate(int scoreAdded, int newScore, ChromieController chromieController);
    public delegate void TimeUpdateDelegate(float currentTime);
    public delegate void ScoreMultiplierUpdateDelegate(int newScoreMultiplier);
    public delegate void LevelUpdateDelegate(int newLevel);
    public delegate void PowerupUpdateDelegate(PowerupBase powerup);

    #endregion

    #region Events

    public event ChromieActionDelegate OnChromieSpawned;
    public event ChromieActionDelegate OnChromieDeSpawned;
    public event ChromieActionDelegate OnChromieDropped;
    public event ChromieColorZoneActionDelegate OnChromieHitColorZone;
    public event ChromieColorZoneActionDelegate OnChromieCollected;
    public event GameoverDelegate OnGameOver;
    public event GameoverDelegate OnTimeUp;
    public event LivesUpdateDelegate OnLivesUpdate;
    public event ScoreUpdateDelegate OnScoreUpdate;
    public event TimeUpdateDelegate OnTimeUpdate;
    public event ScoreMultiplierUpdateDelegate OnScoreMultiplierUpdate;
    public event LevelUpdateDelegate OnLevelUpdate;
    public event PowerupUpdateDelegate OnPowerupStarted;
    public event PowerupUpdateDelegate OnPowerupStopped;

    #endregion


    #region Singleton Instance

    private static GameplayEventsDispatcher _instance;

    public static GameplayEventsDispatcher Instance
    {
        get
        {
            if (GameplayEventsDispatcher._instance == null)
            {
                GameObject container = new GameObject();
                container.name = "Gameplay Events Dispatcher";
                container.AddComponent<GameplayEventsDispatcher>();
            }
            return GameplayEventsDispatcher._instance;
        }
    }

    #endregion


    #region Initialization

    void Awake ()
    {
        GameplayEventsDispatcher._instance = this;
    }

    #endregion


    #region Events

    public void ChromieHitColorZone(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (OnChromieHitColorZone != null)
        {
            OnChromieHitColorZone(chromieController, colorZone);
        }
    }

    public void ChromieSpwaned(ChromieController cheomieController)
    {
        if (OnChromieSpawned != null)
        {
            OnChromieSpawned(cheomieController);
        }
    }

    public void ChromieDeSpwaned(ChromieController cheomieController)
    {
        if (OnChromieDeSpawned != null)
        {
            OnChromieDeSpawned(cheomieController);
        }
    }

    public void ChromieDropped(ChromieController chromieController)
    {
        if (OnChromieDropped != null)
        {
            OnChromieDropped(chromieController);
        }
    }

    public void ChromieCollected(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (OnChromieCollected != null)
        {
            OnChromieCollected(chromieController, colorZone);
        }
    }

    public void GameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public void TimeUp()
    {
        if (OnTimeUp != null)
        {
            OnTimeUp();
        }
    }

    public void LivesUpdate(int maxLives, int currentLives, int change)
    {
        if (OnLivesUpdate != null)
        {
            OnLivesUpdate(maxLives, currentLives, change);
        }
    }

    public void ScoreUpdate(int scoreAdded, int newScore, ChromieController chromieController)
    {
        if (OnScoreUpdate != null)
        {
            OnScoreUpdate(scoreAdded, newScore, chromieController);
        }
    }

    public void TimerUpdate(float currentTime)
    {
        if (OnTimeUpdate != null)
        {
            OnTimeUpdate(currentTime);
        }
    }

    public void ScoreMultiplierUpdate(int scoreMultiplier)
    {
        if (OnScoreMultiplierUpdate != null)
        {
            OnScoreMultiplierUpdate(scoreMultiplier);
        }
    }

    public void LevelUpdate(int newLevel)
    {
        if (OnLevelUpdate != null)
        {
            OnLevelUpdate(newLevel);
        }
    }

    public void PowerupStarted(PowerupBase powerup)
    {
        if (OnPowerupStarted != null)
        {
            OnPowerupStarted(powerup);
        }
    }

    public void PowerupStopped(PowerupBase powerup)
    {
        if (OnPowerupStopped != null)
        {
            OnPowerupStopped(powerup);
        }
    }

    #endregion


    #region Static Helpers

    public static void SendChromieHitColorZone(ChromieController chromieController, ColorZoneController colorZone)
    {
        GameplayEventsDispatcher.Instance.ChromieHitColorZone(chromieController, colorZone);
    }

    public static void SendChromieSpwaned(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance.ChromieSpwaned(chromieController);
    }

    public static void SendChromieDeSpwaned(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance.ChromieDeSpwaned(chromieController);
    }

    public static void SendChromieDroppedd(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance.ChromieDropped(chromieController);
    }

    public static void SendGameOver()
    {
        GameplayEventsDispatcher.Instance.GameOver();
    }

    public static void SendTimeUp()
    {
        GameplayEventsDispatcher.Instance.TimeUp();
    }

    public static void SendChromieCollected(ChromieController chromieController, ColorZoneController colorZone)
    {
        GameplayEventsDispatcher.Instance.ChromieCollected(chromieController, colorZone);
    }

    public static void SendLiveUpdate(int maxLives, int currentLives, int change)
    {
        GameplayEventsDispatcher.Instance.LivesUpdate(maxLives, currentLives, change);
    }

    public static void SendScoreUpdate(int scoreAdded, int newScore, ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance.ScoreUpdate(scoreAdded, newScore, chromieController);
    }

    public static void SendTimerUpdate(float currentTime)
    {
        GameplayEventsDispatcher.Instance.TimerUpdate(currentTime);
    }

    public static void SendScoreMultiplierUpdate(int scoreMultiplier)
    {
        GameplayEventsDispatcher.Instance.ScoreMultiplierUpdate(scoreMultiplier);
    }

    public static void SendLevelUpdate(int newLevel)
    {
        GameplayEventsDispatcher.Instance.LevelUpdate(newLevel);
    }

    public static void SendPowerupStarted(PowerupBase powerup)
    {
        GameplayEventsDispatcher.Instance.PowerupStarted(powerup);
    }

    public static void SendPowerupStopped(PowerupBase powerup)
    {
        GameplayEventsDispatcher.Instance.PowerupStopped(powerup);
    }

    #endregion



}
