using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    #region Delegates

    public delegate void ChromieActionDelegate(ChromieController chromieController);
    public delegate void ChromieColorZoneActionDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void PowerupActivationDelegate(ePowerups.Active powerupType, float duration, float value);
    public delegate void GameoverDelegate();

    #endregion

    #region Events

    public event ChromieActionDelegate OnChromieSpawned;
    public event ChromieActionDelegate OnChromieDropped;
    public event ChromieColorZoneActionDelegate OnChromieHitColorZone;
    public event ChromieActionDelegate OnChromieCollected;
    public event GameoverDelegate OnGameOver;
    public event GameoverDelegate OnTimeUp;
    public event PowerupActivationDelegate OnPowerupActivation;

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

    public void ChromieCollected(ChromieController chromieController)
    {
        if (OnChromieCollected != null)
        {
            OnChromieCollected(chromieController);
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

    public static void SendChromieCollected(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance.ChromieCollected(chromieController);
    }

    public static void SendPowerupActivation(ePowerups.Active powerupType, float duration, float value)
    {
        GameplayEventsDispatcher.Instance.PowerupActivation(powerupType, duration, value);
    }

    #endregion



}
