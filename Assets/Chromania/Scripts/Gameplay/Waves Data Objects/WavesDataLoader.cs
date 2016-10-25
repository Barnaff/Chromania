using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WavesDataLoader : MonoBehaviour {

	public string DataFileName;

	private static List<WaveDefenition> _waves;


	// Use this for initialization
	void Awake () {
		if (WavesDataLoader._waves == null)
		{
			LoadWavesData();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private void LoadWavesData()
	{
		//Debug.Log("load waves data");
		TextAsset textAsset = Resources.Load(DataFileName) as TextAsset;
        //Debug.Log(textAsset.text);

      //  WavesDataLoader._waves = JsonUtility.FromJson<List<WaveDefenition>>(textAsset.text);

        
		JSONObject j = new JSONObject(textAsset.text);
		JSONObject waves = j["wavesArray"];
		WavesDataLoader._waves = new List<WaveDefenition>();
		for (int i=0; i< waves.Count; i++)
		{
			JSONObject waveData = waves[i];
			WaveDefenition wave = new WaveDefenition(waveData);
			WavesDataLoader._waves.Add(wave);
		}
        
    }

	public static List<WaveDefenition> WavesList()
	{
		return WavesDataLoader._waves;
	}


}
