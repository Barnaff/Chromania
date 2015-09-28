using System.Collections.Generic;

public class SequanceDefenition {

	#region Public Properties

	public float EndInterval;
	public int MinLevel;
	public int ID;
	public bool IsEnabled;
	public float LevelModifier;
	public float StartInterval;
	public int MaxLevel;
	public List<SpwanedItemDefenition> SpwanItemList;

	#endregion


	#region Public

	public SequanceDefenition(JSONObject jsonObject)
	{
		EndInterval = float.Parse(jsonObject["endInterval"].ToString());
		MinLevel = int.Parse(jsonObject["minLevel"].ToString());
		ID = int.Parse(jsonObject["id"].ToString());
		IsEnabled = bool.Parse(jsonObject["enabled"].ToString());
		LevelModifier = float.Parse(jsonObject["levelModier"].ToString());
		StartInterval = float.Parse(jsonObject["startInterval"].ToString());
		MaxLevel = int.Parse(jsonObject["maxLevel"].ToString());
		JSONObject spwanedItems = jsonObject["spwanItemsArray"];
		SpwanItemList = new List<SpwanedItemDefenition>();
		for (int i=0; i < spwanedItems.Count; i++)
		{
			JSONObject spwanedItemData = spwanedItems[i];
			SpwanedItemDefenition spwanedItem = new SpwanedItemDefenition(spwanedItemData);
			SpwanItemList.Add(spwanedItem);
		}
	}

	#endregion
}
