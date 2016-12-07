using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;

public class ObjectivePopupObjectiveCellController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _cellContent;

    [SerializeField]
    private Image _Icon;

    [SerializeField]
    private Text _descriptionLabel;

    [SerializeField]
    private Text _progressLabel;

    [SerializeField]
    private Image _progressBarImage;

    [SerializeField]
    private ObjectiveProgress _objectiveProgress;

    private float _currentProgress;

    private float _currentProgressValue;

    [SerializeField]
    private float _progressAnimationDuration = 3.0f;

    #endregion


    #region Public

    public void SetObjectiverProgress(ObjectiveProgress objectiveProgress)
    {
        _objectiveProgress = objectiveProgress;
        _currentProgress = _objectiveProgress.Progress;
        _currentProgressValue = _objectiveProgress.ProgressValue();

        _cellContent.SetActive(true);

        _descriptionLabel.text = _objectiveProgress.FullDescription();

        _progressLabel.text = _currentProgress + "/" + _objectiveProgress.TargetValue();

        _progressBarImage.fillAmount = _currentProgressValue;
    }

    public ObjectiveProgress ObjectiveProgress
    {
        get
        {
            return _objectiveProgress;
        }
    }

    public IEnumerator<float> UpdateNewObjectiveValues(ObjectiveProgress objectiveProgress)
    {
        for (float timer = 0; timer < _progressAnimationDuration; timer += Time.deltaTime)
        {
            float progress = timer / _progressAnimationDuration;

            _progressLabel.text = ((int)Mathf.Lerp(_currentProgress, objectiveProgress.Progress, progress)).ToString() + "/" + _objectiveProgress.TargetValue();
            _progressBarImage.fillAmount = Mathf.Lerp(_currentProgressValue, objectiveProgress.ProgressValue(), progress);
            Debug.Log("progress: " + progress);
            yield return 0f;
        }

        _objectiveProgress = objectiveProgress;
        _currentProgress = _objectiveProgress.Progress;
        _currentProgressValue = _objectiveProgress.ProgressValue();
        _progressLabel.text = _currentProgress + "/" + _objectiveProgress.TargetValue();
        _progressBarImage.fillAmount = _currentProgressValue;
    }

    public IEnumerator<float> CompleteObjective()
    {
        yield return Timing.WaitForSeconds(1.0f);

        _cellContent.SetActive(false);

        yield return Timing.WaitForSeconds(0.5f);
    }

    #endregion



}
