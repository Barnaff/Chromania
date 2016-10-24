using UnityEngine;
using System.Collections;

public class GameplayLivesManager : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private int _startingLives;

    [SerializeField]
    private int _maxLives;

    [SerializeField]
    private int _currentLives;

    [SerializeField]
    private GameplayTrackingData _gameplayTrackingData;

    #endregion


    #region Public

    public void Init(int startingLives, GameplayTrackingData gameplayTrackingData)
    {
        _gameplayTrackingData = gameplayTrackingData;
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
        _startingLives = startingLives;
        _maxLives = _startingLives;
        _currentLives = _startingLives;

        GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives);
    }

    #endregion


    #region Events

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        LooseLife();
    }

    #endregion


    #region Private

    private void LooseLife()
    {
        if (_currentLives <= 0)
        {
            GameplayEventsDispatcher.SendGameOver();
        }
        else
        {
            _currentLives--;
            GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives);
        }
    }

    private void AddLife()
    {
        if (_currentLives < _maxLives)
        {
            _currentLives++;
            GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives);
        }
    }

    #endregion
}
