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
        switch (gameplayMode)
        {
            case eGameplayMode.Classic:
                {
                    SetGameplayModeClassic();
                    break;
                }
            case eGameplayMode.Rush:
                {
                    SetGameplayModeRush();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    #endregion


    #region Private 

    private void SetGameplayModeClassic()
    {
        if (_scoreIndicatorController != null)
        {
            _scoreIndicatorController.gameObject.SetActive(true);
        }
        if (_livesIndicatorController != null)
        {
            _livesIndicatorController.gameObject.SetActive(true);
        }
        if (_timeIndicatorController != null)
        {
            _timeIndicatorController.gameObject.SetActive(false);
        }
    }

    private void SetGameplayModeRush()
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
            _timeIndicatorController.gameObject.SetActive(true);
        }
    }

    #endregion
}
