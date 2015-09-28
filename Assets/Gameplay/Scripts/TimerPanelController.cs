using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerPanelController : MonoBehaviour {

    #region Public Properties

    public Text TimerLabel;

    public int BaseGameTime = 60;

    public OnGameOverDelegate OnGameOver;

    #endregion


    #region Private Properties

    private float _currentTime;

    private bool _running;

    private bool _isTimerEnabled;

    #endregion

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (_isTimerEnabled && _running)
        {
            if (_currentTime < 0)
            {
                if (OnGameOver != null)
                {
                    OnGameOver();
                }
                _running = false;
            }
            _currentTime -= Time.deltaTime;
            UpdateTimerLabel();
        }
	}

    #region Public

    public void EnableTimer()
    {
        _currentTime = BaseGameTime;
        _isTimerEnabled = true;
        UpdateTimerLabel();
    }

    public void StartTimer()
    {
        _running = true;
    }

    public void PauseTimer()
    {
        _running = false;
    }

    public void ResumeTimer()
    {
        _running = true;
    }

    public void AddTime(float timeToAdd)
    {
        _currentTime += timeToAdd;
        UpdateTimerLabel();
    }

    #endregion


    #region Private

    private void UpdateTimerLabel()
    {
        TimerLabel.text = ((int)_currentTime).ToString();
    }

    #endregion
}
