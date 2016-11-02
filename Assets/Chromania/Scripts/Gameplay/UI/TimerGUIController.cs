using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerGUIController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private Text _timerLabel;

    #endregion


    #region Initialization

    void Start()
    {
        GameplayEventsDispatcher.Instance.OnTimeUpdate += OnTimeUpdateHandler;
    }

    #endregion


    #region Events

    private void OnTimeUpdateHandler(float currentTime)
    {
        _timerLabel.text = currentTime.ToString();
    }

    #endregion


}
