using UnityEngine;
using System.Collections;
using System;

public class ChromieController : MonoBehaviour, IDraggable {

    #region Private properties

    [SerializeField]
    private eChromieType _chromieType;

    private bool _isDragged;

    private Bounds _screenBounds;

    private bool _isActive;

    #endregion


    #region Initialization

    // Use this for initialization
    void Start ()
    {
        _screenBounds = Camera.main.gameObject.GetComponent<CameraController>().OrthographicBounds();
    }

    public void Init()
    {
        _isActive = false;
        _isDragged = false;
    }

    #endregion


    #region Update

    // Update is called once per frame
    void LateUpdate()
    {

        if (this.transform.position.x < -_screenBounds.size.x || this.transform.position.x > _screenBounds.size.x ||
            this.transform.position.y < -_screenBounds.size.y || this.transform.position.y > _screenBounds.size.y)
        {
            ChromieDropped();
        }
    }

    #endregion


    #region IDraggable Implementation

    public void BeginDrag()
    {
        _isDragged = true;
        _isActive = true;
    }

    public void EndDrag()
    {
        _isDragged = false;
    }

    public bool IsGrabbed
    {
        get
        {
            return _isDragged;
        }

        set
        {
            _isDragged = value;
        }
    }

    #endregion


    #region Public

    public eChromieType ChromieType
    {
        get
        {
            return _chromieType;
        }
    }

    #endregion


    #region Private

    private void ChromieDropped()
    {
        GameplayEventsDispatcher.SendChromieDroppedd(this);
    }

    #endregion
}
