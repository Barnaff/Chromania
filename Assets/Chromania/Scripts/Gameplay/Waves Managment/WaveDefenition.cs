using System.Collections.Generic;
using UnityEngine;

public class WaveDefenition  {

	#region Public

	public float EndDelay;
	public int MinLevel;
	public int ID;
	public bool IsEnabled;
	public float LevelModier;
	public eGameMode GameMode;
	public float StartDelay;
	public string Name;
	public int MaxLevel;
	public List<SequanceDefenition> SequanceList;

	#endregion


	#region Public

	public WaveDefenition(JSONObject jsonObject)
	{
		EndDelay = float.Parse(jsonObject["endDelay"].ToString());
		MinLevel = int.Parse(jsonObject["minLevel"].ToString());
		ID = int.Parse(jsonObject["id"].ToString());
		IsEnabled = bool.Parse(jsonObject["enabled"].ToString());
		LevelModier = float.Parse(jsonObject["levelModier"].ToString());
		GameMode = (eGameMode)int.Parse(jsonObject["gameMode"].ToString());
		StartDelay = float.Parse(jsonObject["startDelay"].ToString());
		Name = jsonObject["name"].ToString();
		MaxLevel = int.Parse(jsonObject["maxLevel"].ToString());
		JSONObject sequances = jsonObject["sequanceArray"];
		SequanceList = new List<SequanceDefenition>();
		for (int i=0; i< sequances.Count; i++)
		{
			JSONObject sequanceData = sequances[i];
			SequanceDefenition sequance = new SequanceDefenition(sequanceData);
			SequanceList.Add(sequance);
		}
	}

	#endregion
}
