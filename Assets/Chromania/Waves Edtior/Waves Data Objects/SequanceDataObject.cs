using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SequanceDataObject {

    public string Identifier;
    public int MinLevel = 0;
    public int MaxLevel = 0;
    public float LevelModifier = 0.98f;
    public bool Enabled = true;
    public eGameplayMode GameMode;
    public List<WaveDataObject> Waves;

#if UNITY_EDITOR
    public bool Selected = false;
#endif

    public SequanceDataObject()
    {
        Waves = new List<WaveDataObject>();
    }
}
