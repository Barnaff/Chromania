using UnityEngine;
using System;
using System.Collections.Generic;

public interface IGameDataLoader  {

	void LoadGameData(Action dataLoadedAction);

	GameData GetGameData();

    List<ChromieDataItem> GetAllChromiezData();

}
