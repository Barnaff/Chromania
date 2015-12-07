using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameDefenitionDataModel  {
	
	[SerializeField]
	public string GameName;

	[SerializeField]
	public int GameId;

	[SerializeField]
	public string GameBundleName;

	[SerializeField]
	public string GameSceneName;

	[SerializeField]
	public string GameVariables;

	[SerializeField]
	public string GameIconName;


	public GameDefenitionDataModel(Hashtable data)
	{
		GameName = data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_NAME].ToString();
		GameId = int.Parse(data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_ID].ToString());
		GameBundleName = data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_BUNDLE_NAME].ToString();
		GameSceneName = data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_SCENE_NAME].ToString();
		GameVariables = data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_GAME_VARIABLES].ToString();
		GameIconName = data[ServerRequestKeys.SERVER_RESPONSE_KEY_GAME_DEFENITION_ICON_NAME].ToString();
	}
}
