using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveDataObject  {

    public bool Enabled;
    public float Delay;
    public List<SpawnedItemDataObject> SpawnedItems;

 
    public WaveDataObject()
    {
        SpawnedItems = new List<SpawnedItemDataObject>();
    }
}
