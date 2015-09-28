using UnityEngine;
using System;
using System.Collections.Generic;

public class GameDataLoadermanager : MonoBehaviour, IGameDataLoader {

	#region Private Properties
	
	public  GameData _gameData;

	#endregion


	#region Initialize

	// Use this for initialization
	void Awake () {
		Debug.Log("load waves data");
		LoadGameData(null);
	}

	#endregion

	#region IGameDataLoader Implementation

	public void LoadGameData(Action dataLoadedAction)
	{
		//_gameData = GameData.Load(Path.Combine(Application.dataPath + "/Resources" , "GameData.xml"));

		_gameData = GameData.LoadResource("GameData");

		if (dataLoadedAction != null)
		{
			dataLoadedAction();
		}
	}

	public GameData GetGameData()
	{
		return _gameData;
	}

    public List<ChromieDataItem> GetAllChromiezData()
    {
        return _gameData.ChromiezData;
    }


    #endregion


    #region Private



    #endregion
}
