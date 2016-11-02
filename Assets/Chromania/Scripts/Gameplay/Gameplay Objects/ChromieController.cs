using UnityEngine;
using System.Collections;
using System;

public class ChromieController : MonoBehaviour, IDraggable {

    #region Private Properties

    [Header("Chromie")]
    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    [SerializeField]
    private ChromieCharacterController _characterController;

    [Header("Powerups")]
    [SerializeField]
    private bool _isPowerup;

    [SerializeField]
    private GameObject _powerupIndicator;

    [Header("Controlls")]
    [SerializeField]
    private bool _isDragged;

    private Bounds _screenBounds;

    #endregion


    #region Initilization

    void Start()
    {
        _screenBounds = Camera.main.gameObject.GetComponent<CameraController>().OrthographicBounds();
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    #endregion


    #region Public

    public eChromieType ChromieType
    {
        get
        {
            return _chromieDefenition.ChromieColor;
        }
    }

    public ChromieDefenition ChromieDefenition
    {
        get
        {
            return _chromieDefenition;
        } 
    }

    public void SetChromie(ChromieDefenition chromieDefenition)
    {
       
        _chromieDefenition = chromieDefenition;
        if (_characterController != null)
        {
            Lean.LeanPool.Despawn(_characterController.gameObject);
            _characterController = null;
        }

        GameObject characterGameobject = Lean.LeanPool.Spawn(chromieDefenition.CharacterPrefab);
        characterGameobject.transform.SetParent(this.transform);
        characterGameobject.transform.localPosition = Vector3.zero;
        characterGameobject.transform.localScale = Vector3.one;

        _characterController = characterGameobject.GetComponent<ChromieCharacterController>();

        IsPowerup = false;
    }

    public bool IsPowerup
    {
        get
        {
            return _isPowerup;
        }
        set
        {
            _isPowerup = value;

            if (_characterController != null)
            {
                _characterController.IsPowerup = _isPowerup;
            }
        }
    }

    public void SwitchChromie(ChromieDefenition newChromie)
    {
        SetChromie(newChromie);
    }

    #endregion

    #region Idragable Implementation

    public bool IsGrabbed
    {
        get
        {
            return _isDragged; ;
        }

        set
        {
            _isDragged = value;
        }
    }

    public void BeginDrag()
    {
        _isDragged = true;
    }

    public void EndDrag()
    {
        _isDragged = false;
    }

    #endregion



    #region Update

    void LateUpdate()
    {
        if (this.transform.position.x < -_screenBounds.size.x || this.transform.position.x > _screenBounds.size.x ||
            this.transform.position.y < -_screenBounds.size.y || this.transform.position.y > _screenBounds.size.y)
        {
            ChromieDropped();
        }
    }


    #endregion


    #region Private

    private void ChromieDropped()
    {
        GameplayEventsDispatcher.SendChromieDroppedd(this);
    }

    #endregion


    #region Physics

    void OnTriggerEnter2D(Collider2D collider)
    {
        ColorZoneController colorZoneController = collider.gameObject.GetComponent<ColorZoneController>();
        if (colorZoneController != null)
        {
            GameplayEventsDispatcher.SendChromieHitColorZone(this, colorZoneController);
        }
    }

    #endregion



    #region Events

    public void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (chromieController == this)
        {
            //TODO: do stuff when collected
        }

    }

    #endregion




}
