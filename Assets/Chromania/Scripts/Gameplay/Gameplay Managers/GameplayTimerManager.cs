using UnityEngine;
using System.Collections;

public class GameplayTimerManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private float _currentTime;

    [SerializeField]
    private bool _isTimerRunning;

    [SerializeField]
    private bool _isActive;

    #endregion

    #region Public

    public void Init(float initialTime)
    {
        _currentTime = initialTime;
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;
        GameplayEventsDispatcher.Instance.OnKeepPlaying += OnKeepPlayingHandler;
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


    #region Events

    private void OnGameOverHandler()
    {
        Stop();
    }

    private void OnKeepPlayingHandler()
    {
        AddTime(15);
        Run();
    }


    #endregion

}
