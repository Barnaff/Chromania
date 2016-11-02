using UnityEngine;
using System.Collections;

public class GameplayGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _scoreIndicatorController;

    [SerializeField]
    private GameObject _timeIndicatorController;

    [SerializeField]
    private GameObject _livesIndicatorController;

    #endregion


    #region Initilization

    void Awake()
    {
        if (_scoreIndicatorController != null)
        {
            _scoreIndicatorController.gameObject.SetActive(false);
        }
        if (_livesIndicatorController != null)
        {
            _livesIndicatorController.gameObject.SetActive(false);
        }
        if (_timeIndicatorController != null)
        {
            _timeIndicatorController.gameObject.SetActive(false);
        }
    }

    #endregion


    #region Public

    public void DisplayGameplayGUI(eGameplayMode gameplayMode)
    {
        if (_scoreIndicatorController != null)
        {
            _scoreIndicatorController.gameObject.SetActive(true);
        }
        if (_livesIndicatorController != null)
        {
            _livesIndicatorController.gameObject.SetActive(gameplayMode == eGameplayMode.Classic);
        }
        if (_timeIndicatorController != null)
        {
            _timeIndicatorController.gameObject.SetActive(gameplayMode == eGameplayMode.Rush);
        }
    }

    public void PauseButtonAction()
    {
        PauseManager.Instance.PauseGame();
    }

    #endregion


    #region Private 

 

    #endregion
}
