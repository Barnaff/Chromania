using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SequanceDataObject {

    public string Identifier;
    public int MinLevel;
    public int MaxLevel;
    public float LevelModifier;
    public bool Enabled;
    public eGameplayMode GameMode;
    public List<WaveDataObject> Waves;

    public SequanceDataObject()
    {
        Waves = new List<WaveDataObject>();
    }
}
