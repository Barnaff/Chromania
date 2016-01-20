using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(GameData)), CanEditMultipleObjects]
public class GameDataEditor : Editor {

	private GameData _gameData;

    private bool _loaded = false;

	private List<ChromieDataObject> _expendedItems = new List<ChromieDataObject>();

	[MenuItem("Assets/Create/Game Data")]
	public static void CreateAsset ()
	{
		CustomAssetUtility.CreateAsset<GameData>();

	}

	void Awake()
	{
		_gameData = (GameData)target;
	}
	
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();

		_gameData.Version = EditorGUILayout.TextField("Version", _gameData.Version);
        
        if (_loaded)
        {

            if (GUILayout.Button("Save Game Data"))
            {
                ExportData();
            }
        }
        else
        {
            if (GUILayout.Button("Load From XML"))
            {
                LoadData();
            }
        }
		


		EditorGUILayout.Space();


		if (GUILayout.Button("Add New Chromie"))
		{
			if (_gameData.ChromiezData == null)
			{
				_gameData.ChromiezData = new List<ChromieDataObject>();
			}

            //ChromieDataItem newChromie = ScriptableObject.CreateInstance<ChromieDataItem>() as ChromieDataItem;
            ChromieDataObject newChromie = new ChromieDataObject();
			newChromie.ChromieName = "Unamed Chromie";
			_gameData.ChromiezData.Add(newChromie);
		}

		if (_gameData.ChromiezData != null)
		{
			foreach (ChromieDataObject chromieDataItem in _gameData.ChromiezData)
			{
			
				if (chromieDataItem != null)
				{
					EditorGUILayout.BeginVertical("box");

                  

                    EditorGUILayout.BeginHorizontal("box");


                    Color c = chromieDataItem.ColorValue;
                    c.a = 1;
                    GUI.color = c;
                    chromieDataItem.ChromieName = EditorGUILayout.TextField(chromieDataItem.ChromieName);

                    GUI.color = Color.white;

                    if (GUILayout.Button("Expand"))
					{
						ExpandChromieField(chromieDataItem);
					}
                  
                    if (GUILayout.Button("Delete"))
					{
						_gameData.ChromiezData.Remove(chromieDataItem);
						return;
					}
                   
                    EditorGUILayout.EndHorizontal();
					
					if (_expendedItems.Contains(chromieDataItem))
					{
						
						EditorGUILayout.BeginVertical();
						
						chromieDataItem.ChromieID = EditorGUILayout.IntField("Chromie ID", chromieDataItem.ChromieID);
						chromieDataItem.ChromieColor = (eChromieType)EditorGUILayout.EnumPopup("Color Type", chromieDataItem.ChromieColor);
						chromieDataItem.ColorValue = EditorGUILayout.ColorField("Color Value", chromieDataItem.ColorValue);
						chromieDataItem.ColorName = EditorGUILayout.TextField("Color Name", chromieDataItem.ColorName);
						chromieDataItem.PassiveDescription = EditorGUILayout.TextField("Passive Description", chromieDataItem.PassiveDescription);
						chromieDataItem.ActiveDescription = EditorGUILayout.TextField("Active Description", chromieDataItem.ActiveDescription);
                       // chromieDataItem.ActivePowerup = (ActivePowerupType)EditorGUILayout.EnumPopup("Active Powerup", chromieDataItem.ActivePowerup);
                        //chromieDataItem.PassivePowerup = (PassivePowerupType)EditorGUILayout.EnumPopup("Passive Powerup", chromieDataItem.PassivePowerup);
                        chromieDataItem.UnlockedAtStart = EditorGUILayout.Toggle("Unlocked At Start", chromieDataItem.UnlockedAtStart);
						chromieDataItem.UnlockPrice = EditorGUILayout.IntField("Unlock Price", chromieDataItem.UnlockPrice);
						chromieDataItem.UnlockWithAchievment = EditorGUILayout.IntField("Unlock With Achievment", chromieDataItem.UnlockWithAchievment);
						chromieDataItem.UnlockShopItem = EditorGUILayout.TextField("Unlock Shop Item", chromieDataItem.UnlockShopItem);
						chromieDataItem.UnlockAtLevel = EditorGUILayout.IntField("Unlock At Level", chromieDataItem.UnlockAtLevel);
						chromieDataItem.InventorySlotId = EditorGUILayout.IntField("Inventory Slot", chromieDataItem.InventorySlotId);
						chromieDataItem.CountForPowerup = EditorGUILayout.IntField("Count For Powerup", chromieDataItem.CountForPowerup);
						chromieDataItem.PowerupDuration = EditorGUILayout.FloatField("Powerup Duration", chromieDataItem.PowerupDuration);
						chromieDataItem.PassivePowerupValue = EditorGUILayout.FloatField("Passive Value", chromieDataItem.PassivePowerupValue);
						chromieDataItem.ActivePowerupValue = EditorGUILayout.FloatField("Active Value", chromieDataItem.ActivePowerupValue);
						chromieDataItem.CritValue = EditorGUILayout.FloatField("Crit Value", chromieDataItem.CritValue);
						chromieDataItem.CritMultipier = EditorGUILayout.FloatField("Crit Multiplier", chromieDataItem.CritMultipier);
						
						EditorGUILayout.EndVertical();

                        
                    }
					
					EditorGUILayout.EndVertical();

				}

			}
			EditorUtility.SetDirty(_gameData);
		}


		EditorGUILayout.EndVertical();
	}


	private void ExpandChromieField(ChromieDataObject chromieDataItem)
	{
		if (_expendedItems.Contains(chromieDataItem))
		{
			_expendedItems.Remove(chromieDataItem);
		}
		else
		{
			_expendedItems.Add(chromieDataItem);
		}
	}

	private void ExportData()
	{
		AssetDatabase.SaveAssets();

		_gameData.Save(Path.Combine(Application.dataPath + "/Chromania/Resources" , "GameData.xml"));
	}

	private void LoadData()
	{
		_gameData = GameData.Load(Path.Combine(Application.dataPath + "/Chromania/Resources", "GameData.xml"));
        _loaded = true;
	}
	
}
