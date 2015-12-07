using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

[CustomEditor(typeof(SlotsGameController))]
public class SlotsGameControllerInspector : Editor {

	private bool _loadFromString;
	private string _exportString = "";

	private string _loadingString = "";

	// simulations
	private int _simulationNumberOfSpins = 0;

	[ExecuteInEditMode]
	public override void OnInspectorGUI()
	{
		SlotsGameController gameController = (SlotsGameController)target;

		GUILayout.BeginVertical("Box");

		GUILayout.Label("Slots Edit Mode");

		if (GUILayout.Button("Export"))
		{
			_loadFromString = false;
			ExportSlotsData();
		}

		if (!string.IsNullOrEmpty(_exportString))
		{
			EditorGUILayout.TextArea(_exportString, new GUILayoutOption[]{GUILayout.Height(200), GUILayout.Width(200)});
		}

		if (_loadFromString)
		{
			GUILayout.BeginVertical("Box");

			_loadingString = EditorGUILayout.TextArea(_loadingString, new GUILayoutOption[]{GUILayout.Height(200), GUILayout.Width(200)});

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Load"))
			{
				LoadSlotsData();
			}

			if (GUILayout.Button("Cacnel"))
			{
				_loadFromString = false;
			}

			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}
		else
		{
			if (GUILayout.Button("Load From String"))
			{
				_exportString = "";
				_loadFromString = true;
			}
		}

		if (GUILayout.Button("Set Icon Sprites"))
		{
			foreach (SlotsIconDefenition icon in gameController.GameDefenition.IconsDefenitions)
			{
				if (!string.IsNullOrEmpty(icon.IconSpriteName))
				{
					Sprite sprite = gameController.GetSpritePrfab(icon.IconSpriteName);
					icon.IconSprite = sprite;
				}
			}
		}

		GUILayout.Space(20.0f);


		GUILayout.BeginVertical("Box");

		GUILayout.Label("Simulation");

		_simulationNumberOfSpins = EditorGUILayout.IntField("Number Of Spins", _simulationNumberOfSpins);

		if (gameController.IsSimulating)
		{
			if (GUILayout.Button("Stop Simulation"))
			{
				gameController.StopSimulation();
			}

			GUILayout.BeginVertical("Box");
			 
			GUILayout.Label("Progress: " + gameController.SimulationProgress * 100 + "%");

			EditorUtility.SetDirty(target);

			GUILayout.EndVertical();
		}
		else
		{
			if (GUILayout.Button("Start Simulation"))
			{
				gameController.StartSimulation(_simulationNumberOfSpins);
			}
		}

		if (gameController.SimulationResults != null)
		{
			GUILayout.BeginVertical("Box");

			GUILayout.Label("Spins Counted " + gameController.SimulationResults.SpinsCounted);

			GUILayout.Label("Coins Balance " + gameController.SimulationResults.CoinsBalance);

			GUILayout.Label("Pay Amount " + gameController.SimulationResults.PayAmount);

			GUILayout.Label("Win Amount " + gameController.SimulationResults.WinAmount);

			GUILayout.Label("ROI " + ((float)gameController.SimulationResults.WinAmount / (float)gameController.SimulationResults.PayAmount) * 100.0f + "%");

			GUILayout.Label("Win Percentage " + ((float) gameController.SimulationResults.WinCount / (float)gameController.SimulationResults.SpinsCounted) * 100.0f + "%");

			GUILayout.Label("3 In on a line " + gameController.SimulationResults.Tripples);
			GUILayout.Label("4 In on a line " + gameController.SimulationResults.Forths);
			GUILayout.Label("5 In on a line " + gameController.SimulationResults.Fifths);

			GUILayout.Label("Scatter Count " + gameController.SimulationResults.NumberOfScatters);
			GUILayout.Label("Saved by Scatter " + gameController.SimulationResults.SavedByScatter);

			GUILayout.Label("Joker Count " + gameController.SimulationResults.JokerCount);

			GUILayout.Label("Max Win " + gameController.SimulationResults.MaxWin);

			if (gameController.SimulationResults.IconsInfo.Count > 0)
			{
				GUILayout.BeginHorizontal("Box");
				
				GUILayout.Label("Icon");
				
				GUILayout.Label("Count");
				
				GUILayout.Label("3s");
				
				GUILayout.Label("4s");
				
				GUILayout.Label("5s");
				
				GUILayout.Label("Wins");

				GUILayout.Label("%");
				
				GUILayout.EndHorizontal();
				
				foreach (string iconKey in gameController.SimulationResults.IconsInfo.Keys)
				{
					SlotsSimulationIconInfo iconInfo = gameController.SimulationResults.IconsInfo[iconKey];
					
					if (iconInfo != null)
					{
						GUILayout.BeginHorizontal("Box");
						
						GUILayout.Label(iconKey);
						
						GUILayout.Label(iconInfo.Count.ToString());
						
						GUILayout.Label(iconInfo.Trippels.ToString());
						
						GUILayout.Label(iconInfo.Forths.ToString());
						
						GUILayout.Label(iconInfo.Fifths.ToString());
						
						GUILayout.Label(iconInfo.TotalWin.ToString());

						GUILayout.Label((iconInfo.Percentage * 100).ToString());
						
						GUILayout.EndHorizontal();
					}
					
				}

			}

			GUILayout.EndVertical();
		}

		GUILayout.EndHorizontal();


		GUILayout.Space(20.0f);

		GUILayout.EndVertical();

		base.DrawDefaultInspector();
	}

	private void ExportSlotsData()
	{
		SlotsGameController gameController = (SlotsGameController)target;
		SlotsDefenition slotsDefenition = gameController.GameDefenition;


		BinaryFormatter binaryFormatter = new BinaryFormatter();
		
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		
		ColorSurrogate colorSurrogate = new ColorSurrogate();
		surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All) , colorSurrogate);
		
		SlotsIconDefenitionSurrogate slotsIconDefenitionSurrogate = new SlotsIconDefenitionSurrogate();
		surrogateSelector.AddSurrogate(typeof(SlotsIconDefenition), new StreamingContext(StreamingContextStates.All) , slotsIconDefenitionSurrogate);

		binaryFormatter.SurrogateSelector = surrogateSelector;
		
		_exportString = ObjectSerializerUtil.Serialize(slotsDefenition, binaryFormatter);

	}

	private void LoadSlotsData()
	{
		SlotsGameController gameController = (SlotsGameController)target;

		BinaryFormatter binaryFormatter = new BinaryFormatter();
		
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		
		ColorSurrogate colorSurrogate = new ColorSurrogate();
		surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All) , colorSurrogate);
		
		SlotsIconDefenitionSurrogate slotsIconDefenitionSurrogate = new SlotsIconDefenitionSurrogate();
		surrogateSelector.AddSurrogate(typeof(SlotsIconDefenition), new StreamingContext(StreamingContextStates.All) , slotsIconDefenitionSurrogate);
		
		binaryFormatter.SurrogateSelector = surrogateSelector;

		SlotsDefenition slotsDefenition = (SlotsDefenition)ObjectSerializerUtil.Deserialize(_loadingString, binaryFormatter);
		gameController.GameDefenition = slotsDefenition;
		_loadingString = "";
		_loadFromString = false;
	}

	



}
