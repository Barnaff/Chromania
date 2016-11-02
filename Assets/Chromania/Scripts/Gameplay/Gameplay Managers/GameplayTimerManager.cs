using UnityEngine;
using System.Collections;

public class GameplayTimerManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private float _currentTime;

    [SerializeField]
    private bool _isTimerRunning;

    #endregion

    #region Public

    public void Init(float initialTime)
    {
        _currentTime = initialTime;
    }

    public void Run()
    {
        _isTimerRunning = true;
    }

    public void Stop()
    {
        _isTimerRunning = false;
    }

    public void AddTime(float timeToAdd)
    {
        _currentTime += timeToAdd;
    }

    #endregion


    #region Update

    void Update()
    {
        if (_isTimerRunning)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                // time's up!
                GameplayEventsDispatcher.SendTimeUp();
            }
            GameplayEventsDispatcher.SendTimerUpdate(_currentTime);
        }
    }

    #endregion

}
