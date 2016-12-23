using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectivesManager : Kobapps.Singleton<ObjectivesManager> {

    #region Public

    public delegate void ObjectiveUpdateDelegate(ObjectiveProgress objective);

    public event ObjectiveUpdateDelegate OnObjectiveCompleted;

    public event ObjectiveUpdateDelegate OnNewObjectiveAdded;


    #endregion


    #region Private Properties

    [SerializeField]
    private List<ObjectiveDefenition> _allObjectives;

    [SerializeField]
    private ObjectiveProgress[] _activeObjecties;

    [SerializeField]
    private List<int> _usedObjectives;

    private const string STORED_ACTIVE_OBJECTIVES = "storedActiveObjective";
    private const string STORED_USED_OBJECTIVES = "usedActiveObjective";

    private const int NUMBER_OF_ACTIVE_OBJECTIVES = 3;

    private bool _isInitialized = false;

    #endregion


    #region Initialized

    void Start()
    {
        if (!_isInitialized)
        {
            Init();
        }
    }

    #endregion


    #region Public

    public void Init()
    {
        LoadCurrentProgress();
        UpdateObjectivesList();
        _isInitialized = true;
    }

    public void GameStarted()
    {
        if (_activeObjecties != null)
        {
            foreach (ObjectiveProgress objectiveProgress in _activeObjecties)
            {
                if (objectiveProgress.Objective.ResetAtGameStart)
                {
                    objectiveProgress.Progress = 0;
                }
            }
        }
    }

    public ObjectiveProgress[] ActiveObjectives
    {
        get
        {
            return _activeObjecties;
        }
    }

    public void UpdateObjective(ObjectiveProgress objectiveProgress, GameplayTrackingData gameplayTrackingData)
    {
        objectiveProgress.UpdateObjective(gameplayTrackingData);
    }

    public ObjectiveProgress ReplaceObjective(ObjectiveProgress objectiveProgress)
    {
        for (int i = 0; i < _activeObjecties.Length; i++)
        {
            if (_activeObjecties[i] == objectiveProgress)
            {
                _activeObjecties[i] = GetNewObjective();
                return _activeObjecties[i];
            }
        }
        return null;
    }

    public void SaveProgress()
    {
        Save();
    }

    #endregion


    #region Private


    private void LoadCurrentProgress()
    {
        if (PlayerPrefsUtil.HasKey(STORED_ACTIVE_OBJECTIVES))
        {
            _activeObjecties = (ObjectiveProgress[])PlayerPrefsUtil.GetObject(STORED_ACTIVE_OBJECTIVES);
        }
        else
        {
            _activeObjecties = new ObjectiveProgress[NUMBER_OF_ACTIVE_OBJECTIVES];
        }

        if (PlayerPrefsUtil.HasKey(STORED_USED_OBJECTIVES))
        {
            _usedObjectives = (List<int>)PlayerPrefsUtil.GetObject(STORED_USED_OBJECTIVES);
        }
        else
        {
            _usedObjectives = new List<int>();
        }
    }

    private void Save()
    {
        PlayerPrefsUtil.SetObject(STORED_ACTIVE_OBJECTIVES, _activeObjecties);
        PlayerPrefsUtil.SetObject(STORED_USED_OBJECTIVES, _usedObjectives);
    }

    private void UpdateObjectivesList()
    {
        for (int i = 0; i < _activeObjecties.Length; i++)
        {
            if (_activeObjecties[i] == null)
            {
                _activeObjecties[i] = GetNewObjective();

                if (OnNewObjectiveAdded != null)
                {
                    OnNewObjectiveAdded(_activeObjecties[i]);
                }
            }
        }
        Save();
    }

    private ObjectiveProgress GetNewObjective()
    {
        ObjectiveProgress objectiveProgress = new ObjectiveProgress();

        ObjectiveDefenition newObjective = null;

        List<ObjectiveDefenition> avalableObjectives = new List<ObjectiveDefenition>();
        foreach (ObjectiveDefenition objective in AppSettings.Instance.Objectives)
        {
            if (!_usedObjectives.Contains(objective.ObjectiveId))
            {
                avalableObjectives.Add(objective);
            }
        }

        if (avalableObjectives.Count == 0)
        {
            _usedObjectives.Clear();
            newObjective = AppSettings.Instance.Objectives.RandomItem();
        }
        else
        {
            newObjective = avalableObjectives.RandomItem();
        }

        _usedObjectives.Add(newObjective.ObjectiveId);

        objectiveProgress.Objective = newObjective;
        objectiveProgress.Progress = 0;
        objectiveProgress.IsCompleted = false;
        return objectiveProgress;
    }

    #endregion
}

public static class ObjectiveProgressExtension
{
    public static string FullDescription(this ObjectiveProgress objectiveProgress)
    {
        string description = objectiveProgress.Objective.ObjectiveDescription;
        description = description.Replace("&", objectiveProgress.Objective.CountObjective.ToString());
        if (objectiveProgress.Objective.ChromieType != eChromieType.None)
        {
            description = description.Replace("@", objectiveProgress.Objective.ChromieType.ToString());
        }
        else
        {
            description = description.Replace("@", "");
        }
        return description;
    }

    public static float ProgressValue(this ObjectiveProgress objectiveProgress)
    {
        return objectiveProgress.Progress / objectiveProgress.Objective.CountObjective;
    }

    public static int TargetValue(this ObjectiveProgress objectiveProgress)
    {
        return objectiveProgress.Objective.CountObjective;
    }

    public static bool UpdateObjective(this ObjectiveProgress objectiveProgress, int progressValue)
    {
        objectiveProgress.Progress += progressValue;

        if (objectiveProgress.Progress >= objectiveProgress.Objective.CountObjective)
        {
            objectiveProgress.IsCompleted = true;
            objectiveProgress.Progress = objectiveProgress.Objective.CountObjective;
        }

        return objectiveProgress.IsCompleted;
    }

    public static bool UpdateObjective(this ObjectiveProgress objectiveProgress, GameplayTrackingData gameplayTrackingData)
    {
        if (gameplayTrackingData == null)
        {
            return false;
        }

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

        if (objectiveProgress.Progress >= objective.CountObjective)
        {
            objectiveProgress.IsCompleted = true;
            objectiveProgress.Progress = objective.CountObjective;
        }

        return objectiveProgress.IsCompleted;
    }

}
