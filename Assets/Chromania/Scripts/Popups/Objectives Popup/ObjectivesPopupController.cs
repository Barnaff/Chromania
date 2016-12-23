using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class ObjectivesPopupController : PopupBaseController {

    #region Private Properties

    [SerializeField]
    private ObjectivePopupObjectiveCellController _objectiveCellPrefab;

    [SerializeField]
    private Transform _objectiveListContent;

    private GameplayTrackingData _gameplayTrackingData;

    private List<ObjectivePopupObjectiveCellController> _objectiveCells;

    #endregion


    #region Initialization

    void Start()
    {
        if (_objectiveCellPrefab != null)
        {
            _objectiveCellPrefab.gameObject.SetActive(false);
        }
        PopulateObjectiveList();

        Timing.RunCoroutine(ObjectiveUpdateCorutine());
    }

    #endregion


    #region Public

    public void SetGameplayTrackingData(GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;
    }

    #endregion


    #region Private

    [Kobapps.Button]
    private void ProgresAllObjectives()
    {
        foreach (ObjectiveProgress objectiveProgress in ObjectivesManager.Instance.ActiveObjectives)
        {
            objectiveProgress.UpdateObjective((int)(objectiveProgress.Objective.CountObjective * Random.Range(0f, 1f)));
        }

        Timing.RunCoroutine(ObjectiveUpdateCorutine());
    }

    private void PopulateObjectiveList()
    {
        _objectiveCells = new List<ObjectivePopupObjectiveCellController>();

        foreach (ObjectiveProgress objectiveProgress in ObjectivesManager.Instance.ActiveObjectives)
        {
            if (objectiveProgress != null)
            {
                ObjectivePopupObjectiveCellController objectiveCell = CreateObjectiveCell(objectiveProgress);
                _objectiveCells.Add(objectiveCell);
            }
        }
    }

    private ObjectivePopupObjectiveCellController CreateObjectiveCell(ObjectiveProgress objectiveProgress)
    {
        ObjectivePopupObjectiveCellController objectiveCell = Instantiate(_objectiveCellPrefab) as ObjectivePopupObjectiveCellController;
        objectiveCell.gameObject.SetActive(true);
        objectiveCell.gameObject.transform.SetParent(_objectiveListContent);
        objectiveCell.gameObject.transform.localScale = Vector3.one;

        objectiveCell.SetObjectiverProgress(objectiveProgress);

        return objectiveCell;
    }

    private IEnumerator<float> ObjectiveUpdateCorutine()
    {
        for (int i = 0; i < _objectiveCells.Count; i++)
        {
            Debug.Log(ObjectivesManager.Instance.ActiveObjectives.Length);
            if (ObjectivesManager.Instance.ActiveObjectives.Length >= i)
            {
                Debug.Log(ObjectivesManager.Instance.ActiveObjectives[i]);
                ObjectiveProgress objectiveProgress = ObjectivesManager.Instance.ActiveObjectives[i];
                objectiveProgress.UpdateObjective(_gameplayTrackingData);
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(_objectiveCells[i].UpdateNewObjectiveValues(objectiveProgress)));

                if (objectiveProgress.IsCompleted)
                {
                    yield return Timing.WaitUntilDone(Timing.RunCoroutine(_objectiveCells[i].CompleteObjective()));

                    InventoryManager.Instance.AddInventoryItem(_objectiveCells[i].ObjectiveProgress.Objective.Reword);
                }
            }
        }

        yield return Timing.WaitForSeconds(0.5f);

        for (int i = 0; i < _objectiveCells.Count; i++)
        {
            if (_objectiveCells[i].ObjectiveProgress.IsCompleted)
            {
                ObjectiveProgress newObjectiveProgress = ObjectivesManager.Instance.ReplaceObjective(_objectiveCells[i].ObjectiveProgress);
                if (newObjectiveProgress != null)
                {
                    _objectiveCells[i].SetObjectiverProgress(newObjectiveProgress);
                    yield return Timing.WaitForSeconds(0.5f);
                }
            }
        }

        ObjectivesManager.Instance.SaveProgress();
    }

    #endregion
}
