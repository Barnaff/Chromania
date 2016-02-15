using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    #region Delegates

    public delegate void ChromieSpawnedDelegate(ChromieController chromieController);
    public delegate void ChromieDroppedDelegate(ChromieController chromieController);
    public delegate void ChromieHitColorzoneDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void ChromieCollectedDelegate(ChromieController chromieController);
    public delegate void GameoverDelegate();

    #endregion

    #region Events

    public event ChromieSpawnedDelegate OnChromieSpawned;
    public event ChromieDroppedDelegate OnChromieDropped;
    public event ChromieHitColorzoneDelegate OnChromieHitColorZone;
    public event ChromieCollectedDelegate OnChromieCollected;
    public event GameoverDelegate OnGameOver;

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

    #endregion



}
