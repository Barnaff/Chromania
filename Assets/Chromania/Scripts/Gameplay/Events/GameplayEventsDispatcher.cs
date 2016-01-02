using UnityEngine;
using System.Collections;

public class GameplayEventsDispatcher : MonoBehaviour {

    public delegate void ChromieSpawnedDelegate(ChromieController chromieController);
    public delegate void ChromieDroppedDelegate(ChromieController chromieController);
    public delegate void ChromieHitColorzoneDelegate(ChromieController chromieController, ColorZoneController colorZone);
    public delegate void ChromieCollectedDelegate(ChromieController chromieController);

    public event ChromieSpawnedDelegate OnChromieSpawned;
    public event ChromieDroppedDelegate OnChromieDropped;

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

    public static void SendChromieSpwaned(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance().ChromieSpwaned(chromieController);
    }

    public void ChromieSpwaned(ChromieController cheomieController)
    {
        if (OnChromieSpawned != null)
        {
            OnChromieSpawned(cheomieController);
        }
    }

    public static void SendChromieDroppedd(ChromieController chromieController)
    {
        GameplayEventsDispatcher.Instance().ChromieDropped(chromieController);
    }

    public void ChromieDropped(ChromieController chromieController)
    {
        if (OnChromieDropped != null)
        {
            OnChromieDropped(chromieController);
        }
    }

    #endregion



}
