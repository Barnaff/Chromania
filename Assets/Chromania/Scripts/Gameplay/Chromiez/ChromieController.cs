using UnityEngine;
using System.Collections;
using System;

public class ChromieController : MonoBehaviour, IDraggable {

    #region Private properties
    
    [Header("Chromie Setup")]
    [SerializeField]
    private ChromieDataObject _chromieData;

    [SerializeField]
    private ChromieCaharcterController _characterController;

    [Header("Powerups")]
    [SerializeField]
    private bool _isPowerup;

    [SerializeField]
    private GameObject _powerupIndicator;


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

    public void Init(ChromieDataObject chromieData)
    {
        _isActive = false;
        _isDragged = false;
        _chromieData = chromieData;
        _isPowerup = false;

        if (_characterController != null)
        {
            Lean.LeanPool.Despawn(_characterController);
        }

        IChromiezAssetsCache chromiezAssetsCache = ComponentFactory.GetAComponent<IChromiezAssetsCache>();
        if (chromiezAssetsCache != null)
        {
            GameObject characterPrefab = chromiezAssetsCache.GetChromieCharacter(chromieData.ChromieColor);
            if (characterPrefab != null)
            {
                GameObject newCharacter = Lean.LeanPool.Spawn(characterPrefab);
                _characterController = newCharacter.GetComponent<ChromieCaharcterController>();
                if (_characterController != null)
                {
                    _characterController.transform.SetParent(this.transform);
                    _characterController.transform.localPosition = Vector3.zero;
                }
            }
        }

        if (_powerupIndicator != null)
        {
            _powerupIndicator.SetActive(false);
        }
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
            return _chromieData.ChromieColor;
        }
    }

    public void CollectChromie()
    {
        Lean.LeanPool.Despawn(this.gameObject);
    }

    public ChromieDataObject ChromieData
    {
        get
        {
            return _chromieData;
        }
    }

    public void SetAsPowerup()
    {
        _isPowerup = true;
        if (_powerupIndicator != null)
        {
            _powerupIndicator.SetActive(true);
        }
    }

    public bool IsPowerup
    {
        get
        {
            return _isPowerup;
        }
    }

    public void ChangeChromie(ChromieDataObject chromieData)
    {
        _chromieData = chromieData;
        IChromiezAssetsCache chromiezAssetsCache = ComponentFactory.GetAComponent<IChromiezAssetsCache>();
        if (chromiezAssetsCache != null)
        {
            GameObject characterPrefab = chromiezAssetsCache.GetChromieCharacter(chromieData.ChromieColor);
            if (characterPrefab != null)
            {
                if (_characterController != null)
                {
                    Lean.LeanPool.Despawn(_characterController);
                }
                GameObject newCharacter = Lean.LeanPool.Spawn(characterPrefab);
                _characterController = newCharacter.GetComponent<ChromieCaharcterController>();
                if (_characterController != null)
                {
                    _characterController.transform.SetParent(this.transform);
                    _characterController.transform.localPosition = Vector3.zero;
                }
            }
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
        if (collider.gameObject.tag == "ColorZone")
        {
            GameplayEventsDispatcher.SendChromieHitColorZone(this, collider.GetComponent<ColorZoneController>());
        }
    }

    #endregion
}
