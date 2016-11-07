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

    [SerializeField]
    private bool _isImmune;

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

    void OnDestroy()
    {
        GameplayEventsDispatcher.Instance.OnChromieDropped -= OnChromieDroppedHandler;
    }

    public void AddLife(int amount = 1)
    {
        if (_currentLives < _maxLives)
        {
            _currentLives += amount;
            GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives);
        }
    }

    public void AddLifeSlot(int amount = 1)
    {
        _maxLives += amount;
        GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives);
    }

    public void SetImmune(bool isImmune)
    {
        _isImmune = isImmune;
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
        if (_isImmune)
        {
            return;
        }

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

    #endregion
}
