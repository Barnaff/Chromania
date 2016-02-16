using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LivesPanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private GameObject _liveIndicatorPrefab;

    [SerializeField]
    private Transform _livesIndicatorContainer;

    [SerializeField]
    private Sprite _fullLiveSprite;

    [SerializeField]
    private Sprite _emptyLiveSprite;

    [SerializeField]
    private int _numberOfInitialLivesSlots;

    [SerializeField]
    private int _currentLivesCount;

    private List<GameObject> _liveIndicators;

    private int _lastLiveIndicatorIndex;

    [SerializeField]
    private Vector2 _baseIconSize;

    [SerializeField]
    private float _scaleMultiplierPerIcon;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start ()
    {
        this.gameObject.SetActive(false);

        _liveIndicatorPrefab.SetActive(false);
    }

    #endregion


    #region Public

    public void Init()
    {
        this.gameObject.SetActive(true);
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
        PopulateLivesIndicators();
    }
    
    #endregion


    #region Events

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        ReduceLive();
    }

    #endregion


    #region Private

    private void PopulateLivesIndicators()
    {
        _liveIndicators = new List<GameObject>();
        float scaleFactor = _scaleMultiplierPerIcon;
        for (int i=0; i < _numberOfInitialLivesSlots; i++)
        {
            GameObject liveIndicator = Instantiate(_liveIndicatorPrefab);
            liveIndicator.SetActive(true);
            liveIndicator.transform.SetParent(_livesIndicatorContainer);
            _liveIndicators.Add(liveIndicator);
            liveIndicator.GetComponent<Image>().sprite = _emptyLiveSprite;

            liveIndicator.GetComponent<LayoutElement>().minWidth = _baseIconSize.x * scaleFactor;
            liveIndicator.GetComponent<LayoutElement>().minHeight = _baseIconSize.y * scaleFactor;
            scaleFactor *= _scaleMultiplierPerIcon;
        }
    }

    private void AddLive()
    {

    }

    private void ReduceLive()
    {
        _currentLivesCount--;
        if (_currentLivesCount <= 0)
        {
            GameplayEventsDispatcher.SendGameOver();
        }
    }

    #endregion
}
