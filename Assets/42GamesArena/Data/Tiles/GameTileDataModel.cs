using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameTileDataModel : TileDataModelAbstract  {

	[SerializeField]
	public GameDefenitionDataModel GameDefenition;

	#region TileDataModelAbstract Implementation

	public override TileDataModelAbstract Decode (Hashtable data)
	{
		Hashtable gameDefnitionData = data[ServerRequestKeys.SERVER_RESPONSE_KEY_TILE_GAME_DEFENITION] as Hashtable;
		GameDefenition = new GameDefenitionDataModel(gameDefnitionData);
		return base.Decode (data);
	}

	#endregion

}
