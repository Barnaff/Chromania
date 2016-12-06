using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        StartCoroutine(ObjectiveUpdateCorutine());
    }

    #endregion


    #region Public

    public void SetGameplayTrackingData(GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;
    }

    #endregion


    #region Private

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

    private IEnumerator ObjectiveUpdateCorutine()
    {
        foreach (ObjectivePopupObjectiveCellController objectiveCell in _objectiveCells)
        {

        }
        yield return null;
    }

    #endregion
}
