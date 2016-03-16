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

    [SerializeField]
    private Vector2 _baseIconSize;

    [SerializeField]
    private float _scaleMultiplierPerIcon;


    [Header("Dropped Chromie Indicator")]
    [SerializeField]
    private bool _disaplyDropppedChromieIndication;

    [SerializeField]
    private GameObject _dropedChromieIndicatorPrefab;


    [Header("Debug")]
    [SerializeField]
    private bool _enableDebug;

    [SerializeField]
    private bool _endlessLives = false;

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

    public void AddLife()
    {
        AddLive();
    }
    
    #endregion


    #region Events

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        ReduceLive();
        if (_disaplyDropppedChromieIndication)
        {
            StartCoroutine(DisplayHit(chromieController.transform.position));

            Camera.main.gameObject.GetComponent<CameraController>().Shake();
        }
    }

    #endregion


    #region Private

    private void PopulateLivesIndicators()
    {
        _liveIndicators = new List<GameObject>();
        float scaleFactor = 1;
        for (int i=0; i < _numberOfInitialLivesSlots; i++)
        {
            GameObject liveIndicator = Instantiate(_liveIndicatorPrefab);
            liveIndicator.SetActive(true);
            liveIndicator.transform.SetParent(_livesIndicatorContainer);
            liveIndicator.transform.SetAsFirstSibling();
            liveIndicator.name = "Life " + i;

            _liveIndicators.Add(liveIndicator);
            liveIndicator.GetComponent<Image>().sprite = _emptyLiveSprite;

            liveIndicator.GetComponent<LayoutElement>().minWidth = _baseIconSize.x * scaleFactor;
            liveIndicator.GetComponent<LayoutElement>().minHeight = _baseIconSize.y * scaleFactor;
            scaleFactor /= _scaleMultiplierPerIcon;
        }

        for (int i=0; i < _currentLivesCount; i++)
        {
            AnimateLiveIndicator(i, _fullLiveSprite, false);
        }
    }

    private void AddLive()
    {
        if (_currentLivesCount < _numberOfInitialLivesSlots)
        {
            AnimateLiveIndicator(_currentLivesCount, _fullLiveSprite, true);
            _currentLivesCount++;
        }
    }

    private void ReduceLive()
    {
        if (!_endlessLives)
        {
            _currentLivesCount--;
        }
       
        AnimateLiveIndicator(_currentLivesCount, _emptyLiveSprite, true);
        if (_currentLivesCount <= 0)
        {
            GameplayEventsDispatcher.SendGameOver();
        }
    }

    private void AnimateLiveIndicator(int index, Sprite sprite, bool animated)
    {
        if (index >= 0 && index < _liveIndicators.Count)
        {
            GameObject liveIndicator = _liveIndicators[index];
            if (liveIndicator != null)
            {
                Image liveImage = liveIndicator.GetComponent<Image>();
                if (liveImage != null)
                {
                    liveImage.sprite = sprite;
                }
            }
        }
    }


    public IEnumerator DisplayHit(Vector3 chromiePosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(chromiePosition);
        Vector3 indicatorPosition = screenPosition;

        if (screenPosition.x < 0)
        {
            indicatorPosition.x = 50;
        }

        if (screenPosition.x > Screen.width)
        {
            indicatorPosition.x = Screen.width - 50;
        }

        if (screenPosition.y < 0)
        {
            indicatorPosition.y = 50;
        }

        if (screenPosition.y > Screen.height)
        {
            indicatorPosition.y = Screen.height - 50;
        }
        indicatorPosition = Camera.main.ScreenToWorldPoint(indicatorPosition);

        GameObject hitIndicator = Lean.LeanPool.Spawn(_dropedChromieIndicatorPrefab, indicatorPosition, Quaternion.identity);
        hitIndicator.transform.localScale = new Vector3(2, 2, 2);
        iTween.PunchScale(hitIndicator, iTween.Hash("time", 0.5f, "x", 1.2f, "y", 1.2f));
        iTween.ScaleTo(hitIndicator, iTween.Hash("time", 0.5f, "x", 0, "y", 0, "delay", 1.0f));

        yield return new WaitForSeconds(1.5f);

        Lean.LeanPool.Despawn(hitIndicator);

       
    }

    #endregion



#if UNITY_EDITOR

    void OnGUI()
    {
        if (_enableDebug)
        {
            GUILayout.BeginVertical("Box");

            if (GUILayout.Button("Add Life"))
            {
                AddLive();
            }
            if (GUILayout.Button("Reduce Life"))
            {
                ReduceLive();
            }
            GUILayout.EndVertical();
        }
    }

#endif
}
