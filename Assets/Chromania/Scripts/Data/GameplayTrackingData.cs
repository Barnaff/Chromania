using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameplayTrackingData  {

    [SerializeField]
    public int Score;

    [SerializeField]
    public eGameplayMode GameplayMode;

    [SerializeField]
    public List<eChromieType> SelectedColors;

    [SerializeField]
    public int DroppedChromiez;

    [SerializeField]
    public int CollectedChromiez;

    [SerializeField]
    public int CollectedPowerups;

    [SerializeField]
    public int CollectedCurrency;


}
