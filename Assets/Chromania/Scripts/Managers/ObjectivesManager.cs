using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesManager : Kobapps.Singleton<ObjectivesManager> {

    #region Public

    public delegate void ObjectiveCompletedDelegate(ObjectiveDefenition objective);

    public event ObjectiveCompletedDelegate OnObjectiveCompleted;

    #endregion


    #region Private Properties

    [SerializeField]
    private List<ObjectiveDefenition> _allObjectives;

    [SerializeField]
    private List<ObjectiveProgress> _activeObjecties;

    #endregion


    #region Public

    public List<ObjectiveProgress> ActiveObjectives
    {
        get
        {
            return _activeObjecties;
        }
    }

    public void FinshedGame(GameplayTrackingData trackingData)
    {

    }

    #endregion


    #region Private

    private void UpdateActiveObjectives(GameplayTrackingData gameplayTrackingData)
    {
        foreach (ObjectiveProgress objectivProgresse in _activeObjecties)
        {
            objectivProgresse.UpdateObjective(gameplayTrackingData);
        }
    }

    #endregion
}

public static class ObjectiveProgressExtension
{
    public static bool UpdateObjective(this ObjectiveProgress objectiveProgress, GameplayTrackingData gameplayTrackingData)
    {
        ObjectiveDefenition objective = objectiveProgress.Objective;
        switch (objective.Type)
        {
            case eObjectiveType.ColletChromiez:
                {
                    if (objective.ChromieType != eChromieType.None)
                    {
                        if (gameplayTrackingData.ColletedColors.ContainsKey(objective.ChromieType))
                        {
                            objectiveProgress.Progress += gameplayTrackingData.ColletedColors[objective.ChromieType];
                        }
                    }
                    else
                    {
                        objectiveProgress.Progress += gameplayTrackingData.CollectedChromiez;
                    }
                    break;
                }
            case eObjectiveType.UsedPowerups:
                {
                    if (objective.ChromieType != eChromieType.None)
                    {
                        if (gameplayTrackingData.CollectedPoweupsColors.ContainsKey(objective.ChromieType))
                        {
                            objectiveProgress.Progress += gameplayTrackingData.CollectedPoweupsColors[objective.ChromieType];
                        }
                    }
                    else
                    {
                        objectiveProgress.Progress += gameplayTrackingData.CollectedPowerups;
                    }
                    break;
                }
            case eObjectiveType.DroppedChromiez:
                {
                    if (objective.ChromieType != eChromieType.None)
                    {
                        if (gameplayTrackingData.DroppedPoweupsColors.ContainsKey(objective.ChromieType))
                        {
                            objectiveProgress.Progress += gameplayTrackingData.DroppedPoweupsColors[objective.ChromieType];
                        }
                    }
                    else
                    {
                        objectiveProgress.Progress += gameplayTrackingData.DroppedChromiez;
                    }
                    break;
                }
            case eObjectiveType.EarnCurrency:
                {
                    objectiveProgress.Progress += gameplayTrackingData.CollectedCurrency;
                    break;
                }
            case eObjectiveType.ComboScore:
                {
                    break;
                }
            case eObjectiveType.ComboCount:
                {
                    break;
                }
            case eObjectiveType.MaxLives:
                {
                    break;
                }
        }
        return false;
    }
}
