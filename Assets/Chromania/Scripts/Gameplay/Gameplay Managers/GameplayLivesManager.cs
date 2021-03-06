﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

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

    [SerializeField]
    private GameObject _hitIndicatorPrefab;

    [SerializeField]
    private bool _isActive;

    #endregion


    #region Public

    public void Init(int startingLives, GameplayTrackingData gameplayTrackingData)
    {
        _isActive = true;
        _gameplayTrackingData = gameplayTrackingData;
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
        GameplayEventsDispatcher.Instance.OnGameOver += OnGameOverHandler;
        GameplayEventsDispatcher.Instance.OnKeepPlaying += OnKeepPlayingHandler;
        _startingLives = startingLives;
        _maxLives = _startingLives;
        _currentLives = _startingLives;

        GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives, 0);

        _hitIndicatorPrefab = Resources.Load("HitIndicator") as GameObject;
        if (_hitIndicatorPrefab == null)
        {
            Debug.LogError("ERROR - Hit Indicator could not be found in resources!");
        }
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
            GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives, amount);
        }
    }

    public void AddLifeSlot(int amount = 1)
    {
        _maxLives += amount;
        GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives, 0);
    }

    public void SetImmune(bool isImmune)
    {
        _isImmune = isImmune;
    }

    #endregion


    #region Events

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        if (_isActive)
        {
            LooseLife();

            if (_hitIndicatorPrefab != null)
            {
                Timing.RunCoroutine(DisplayHit(chromieController.transform.position));
            }
        }
    }

    private void OnGameOverHandler()
    {
        _isActive = false;
    }

    private void OnKeepPlayingHandler()
    {
        AddLife(1);
        _isActive = true;
    }

    #endregion


    #region Private

    private IEnumerator<float> DisplayHit(Vector3 position)
    {
        Bounds cameraBounds = Camera.main.gameObject.GetComponent<CameraController>().OrthographicBounds();
        float margins = 1f;
        if (position.x < -cameraBounds.size.x * 0.5f)
        {
            position.x = -cameraBounds.size.x * 0.5f + margins;
        }
        else if (position.x > cameraBounds.size.x * 0.5f)
        {
            position.x = cameraBounds.size.x * 0.5f - margins;
        }
        else if (position.y < -cameraBounds.size.y * 0.5f)
        {
            position.y = -cameraBounds.size.y * 0.5f + margins;
        }
        else if (position.y > cameraBounds.size.y * 0.5f)
        {
            position.y = cameraBounds.size.y * 0.5f - margins;
        }
        GameObject hitIndicator = Lean.LeanPool.Spawn(_hitIndicatorPrefab, position, Quaternion.identity);

        yield return Timing.WaitForSeconds(2.0f);

        Lean.LeanPool.Despawn(hitIndicator);
    }

    private void LooseLife()
    {
        if (_isImmune)
        {
            return;
        }

        if (_currentLives <= 0)
        {
            GameplayEventsDispatcher.SendOutOfLives();
        }
        else
        {
            _currentLives--;
            GameplayEventsDispatcher.SendLiveUpdate(_maxLives, _currentLives, -1);
        }
    }

    #endregion
}
