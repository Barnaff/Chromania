using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectivePopupObjectiveCellController : MonoBehaviour {

    #region Private Properties

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

    #endregion


    #region Public

    public void SetObjectiverProgress(ObjectiveProgress objectiveProgress)
    {
        _objectiveProgress = objectiveProgress;
        _currentProgress = _objectiveProgress.Progress;
        _currentProgressValue = _objectiveProgress.ProgressValue();

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

    #endregion

}
