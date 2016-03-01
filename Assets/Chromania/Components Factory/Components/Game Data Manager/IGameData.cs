using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGameData  {

    GameData GameData { get; }

    List<ChromieDataObject> ChromiezList { get; }

    int[] LevelsForgameplayMode(eGameMode gameMode);

}
