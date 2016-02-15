using UnityEngine;
using System.Collections;

public class LivesPanelController : MonoBehaviour {

    #region Private properties

    [SerializeField]
    private GameObject _liveIndicatorPrefab;

    [SerializeField]
    private Sprite _fullLiveSprite;

    [SerializeField]
    private Sprite _emptyLiveSprite;

    [SerializeField]
    private int _numberOfInitialLivesSlots;

    [SerializeField]
    private int _currentLivesCount;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start ()
    {
        this.gameObject.SetActive(false);
	}

    #endregion


    #region Public

    public void Init()
    {
        GameplayEventsDispatcher.Instance.OnChromieDropped += OnChromieDroppedHandler;
    }
    
    #endregion


    #region Events

    private void OnChromieDroppedHandler(ChromieController chromieController)
    {
        ReduceLive();
    }

    #endregion


    #region Private

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
