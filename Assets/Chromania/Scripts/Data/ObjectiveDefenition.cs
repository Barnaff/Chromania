using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectiveDefenition {

    [Header("Meta Data")]
    public int ObjectiveId;

    public int MinLevel;

    public string ObjectiveTitle;

    public string ObjectiveDescription;


    [Header("Objective Defenition")]
    public eObjectiveType Type;

    public bool ResetAtGameStart;

    public int CountObjective;

    public eChromieType ChromieType;


    [Header("Reword")]
    public Inventoryitem Reword;

}

[System.Serializable]
public class ObjectiveProgress
{
    public ObjectiveDefenition Objective;

    public float Progress;

    public bool IsCompleted;

    public bool IsNew;

}

public enum eObjectiveType
{
    ColletChromiez,
    UsedPowerups,
    DroppedChromiez,
    EarnCurrency,
    ComboScore,
    ComboCount,
    MaxLives,

}
