using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameplayTrackingData  {

    public int Score;

    public eGameplayMode GameplayMode;

    public List<eChromieType> SelectedColors;

    public int DroppedChromiez;

    public int CollectedChromiez;

    public int CollectedPowerups;

    public int CollectedCurrency;

    public Dictionary<eChromieType, int> ColletedColors = new Dictionary<eChromieType, int>();

    public Dictionary<eChromieType, int> CollectedPoweupsColors = new Dictionary<eChromieType, int>();

    public Dictionary<eChromieType, int> DroppedPoweupsColors = new Dictionary<eChromieType, int>();

    public int ComboScore;

    public int MaxComboScore;

    public int MaxComboCount;

    public int MaxLives;

    public int LivesGained;

}
