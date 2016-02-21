using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerPanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private int _gameplayRoundTime = 60;

    [SerializeField]
    private Text _timerLabel;

    private float _currentTime;

    private bool _isTimerRunning;

    #endregion


    #region Update

    // Update is called once per frame
    void Update ()
    {
	    if (_isTimerRunning)
        {
            if (_currentTime < 0)
            {
                TimeUp();
            }
            _currentTime -= Time.deltaTime;
            UpdateTimerLabel();
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.T))
        {
            _currentTime--;
        }

#endif
    }

    #endregion


    #region Public

    public void Init()
    { 
        this.gameObject.SetActive(true);
    }

    public void StartTimer()
    {
        _currentTime = _gameplayRoundTime;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    public void AddTimer(float timeToAdd)
    {
        _currentTime += timeToAdd;
        UpdateTimerLabel();
    }

    #endregion


    #region Private

    private void UpdateTimerLabel()
    {
        _timerLabel.text = ((int)_currentTime).ToString();
    }

    private void TimeUp()
    {
        _isTimerRunning = false;
        GameplayEventsDispatcher.SendTimeUp();
    }

    #endregion
}
