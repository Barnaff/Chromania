using UnityEngine;
using System.Collections;
using System;

public class ChromieController : MonoBehaviour, IDraggable {

    #region Private Properties

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    [SerializeField]
    private ChromieCharacterController _characterController;

    [SerializeField]
    private bool _isDragged;

    [SerializeField]
    private bool _isActive;

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
        _isActive = true;
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
