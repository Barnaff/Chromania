using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : FactoryComponent, IGameData {

    #region FactoryComponent Implementation

    public override void InitComponentAtStart()
    {

    }

    public override void InitComponentAtAwake()
    {

    }

    #endregion

    #region Private Properties

    public GameData _gameData;

    #endregion


    #region Initialize

    // Use this for initialization
    void Awake()
    {
        Debug.Log("load waves data");
        LoadGameData(null);
    }

    #endregion

    #region IGameDataLoader Implementation

    public void LoadGameData(System.Action dataLoadedAction)
    {
        //_gameData = GameData.Load(Path.Combine(Application.dataPath + "/Resources" , "GameData.xml"));

        _gameData = GameData.LoadResource("GameData");

        if (dataLoadedAction != null)
        {
            dataLoadedAction();
        }
    }

    public GameData GameData
    {
        get
        {
            return _gameData;
        }
    }

    public List<ChromieDataObject> ChromiezList
    {
        get
        { 
            return _gameData.ChromiezData;
       }
    }


    #endregion


    #region Private



    #endregion

}
