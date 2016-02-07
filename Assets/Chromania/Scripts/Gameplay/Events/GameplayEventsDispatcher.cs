using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    public delegate void ChromieSpawnedDelegate(ChromieController chromieController);
    public delegate void ChromieDroppedDelegate(ChromieController chromieController);
    public delegate void ChromieHitColorzoneDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void ChromieCollectedDelegate(ChromieController chromieController);

    public event ChromieSpawnedDelegate OnChromieSpawned;
    public event ChromieDroppedDelegate OnChromieDropped;
    public event ChromieHitColorzoneDelegate OnChromieHitColorZone;

    private static GameplayEventsDispatcher _instance;

    public static GameplayEventsDispatcher Instance()
    {
        if (GameplayEventsDispatcher._instance == null)
        {
            GameObject container = new GameObject();
            container.name = "Gameplay Events Dispatcher";
            container.AddComponent<GameplayEventsDispatcher>();
        }
        return GameplayEventsDispatcher._instance;
    }


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

    #endregion


    #region Static Helpers

    public static void SendChromieHitColorZone(ChromieController chromieController, ColorZoneController colorZone)
    {
        GameplayEventsDispatcher.Instance().ChromieHitColorZone(chromieController, colorZone);
    }

    public static void SendChromieSpwaned(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance().ChromieSpwaned(chromieController);
    }

    public static void SendChromieDroppedd(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance().ChromieDropped(chromieController);
    }

    #endregion



}
