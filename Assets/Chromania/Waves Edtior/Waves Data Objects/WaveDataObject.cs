using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveDataObject  {

    public bool Enabled = true;
    public float Delay = 0;
    public List<SpawnedItemDataObject> SpawnedItems;

 
    public WaveDataObject()
    {
        SpawnedItems = new List<SpawnedItemDataObject>();
    }
}
