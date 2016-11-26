using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectiveDefenition : ScriptableObject {

    [Header("Meta Data")]
    public int ObjectiveId;

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
