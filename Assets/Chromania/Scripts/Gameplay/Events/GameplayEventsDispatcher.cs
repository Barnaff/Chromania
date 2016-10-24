using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    #region Delegates

    public delegate void ChromieActionDelegate(ChromieController chromieController);
    public delegate void ChromieColorZoneActionDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void PowerupActivationDelegate(ePowerups.Active powerupType, float duration, float value);
    public delegate void GameoverDelegate();

    public delegate void LivesUpdateDelegate(int maxLives, int currentLives);
    public delegate void ScoreUpdateDelegate(int scoreAdded, int newScore);
    public delegate void TimeUpdateDelegate(float currentTime);

    #endregion

    #region Events

    public event ChromieActionDelegate OnChromieSpawned;
    public event ChromieActionDelegate OnChromieDropped;
    public event ChromieColorZoneActionDelegate OnChromieHitColorZone;
    public event ChromieColorZoneActionDelegate OnChromieCollected;
    public event GameoverDelegate OnGameOver;
    public event GameoverDelegate OnTimeUp;
    public event PowerupActivationDelegate OnPowerupActivation;

    public event LivesUpdateDelegate OnLivesUpdate;
    public event ScoreUpdateDelegate OnScoreUpdate;
    public event TimeUpdateDelegate OnTimeUpdate;
   
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

    public void PowerupActivation(ePowerups.Active powerupType, float duration, float value)
    {
        if (OnPowerupActivation != null)
        {
            OnPowerupActivation(powerupType, duration, value);
        }
    }

    public void LivesUpdate(int maxLives, int currentLives)
    {
        if (OnLivesUpdate != null)
        {
            OnLivesUpdate(maxLives, currentLives);
        }
    }

    public void ScoreUpdate(int scoreAdded, int newScore)
    {
        if (OnScoreUpdate != null)
        {
            OnScoreUpdate(scoreAdded, newScore);
        }
    }

    public void TimerUpdate(float currentTime)
    {
        if (OnTimeUpdate != null)
        {
            OnTimeUpdate(currentTime);
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

    public static void SendPowerupActivation(ePowerups.Active powerupType, float duration, float value)
    {
        GameplayEventsDispatcher.Instance.PowerupActivation(powerupType, duration, value);
    }

    public static void SendLiveUpdate(int maxLives, int currentLives)
    {
        GameplayEventsDispatcher.Instance.LivesUpdate(maxLives, currentLives);
    }

    public static void SendScoreUpdate(int scoreAdded, int newScore)
    {
        GameplayEventsDispatcher.Instance.ScoreUpdate(scoreAdded, newScore);
    }

    public static void SendTimerUpdate(float currentTime)
    {
        GameplayEventsDispatcher.Instance.TimerUpdate(currentTime);
    }

    #endregion



}
