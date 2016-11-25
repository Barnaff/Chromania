using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private ColorZoneCharacterController _colorZoneCharacter;

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    private ChromieDefenition _overrideChromieDefenition = null;

    [SerializeField]
    private float _baseScale = 1f;

    #endregion


    #region Initiliaze

    void Start()
    {
        _overrideChromieDefenition = null;
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    void OnEnable()
    {
        UpdateScaleFactor();
    }

    #endregion

    #region Public

    public void SetColorZone(ChromieDefenition chromieDefention)
    {
        _chromieDefenition = chromieDefention;
        SetColorZoneCharacter(_chromieDefenition);
    }

    public ChromieDefenition ChromieDefenition
    {
        get
        {
            return _chromieDefenition;
        }
    }

    public eChromieType Color
    {
        get
        {
            if (_overrideChromieDefenition != null)
            {
                return _overrideChromieDefenition.ChromieColor;
            }
            return _chromieDefenition.ChromieColor;
        }
    }

    public void SetOverrideColor(ChromieDefenition chromieDefention)
    {
        _overrideChromieDefenition = chromieDefention;
        if (_overrideChromieDefenition == null)
        {
            SetColorZoneCharacter(_chromieDefenition);
        }
        else
        {
            SetColorZoneCharacter(_overrideChromieDefenition);
        }
    }

    public void DisplayIntroAndActivate()
    {
        this.gameObject.SetActive(true);
        _colorZoneCharacter.Intro();
    }

    public void UpdateScaleFactor()
    {
        this.transform.localScale = Vector3.one * _baseScale * GameplayBuffsManager.GetValue(eBuffType.ColorZoneSizeMultiplier);
    }

    #endregion

    #region Private 

    private void SetColorZoneCharacter(ChromieDefenition chromieDefention)
    {
        if (_colorZoneCharacter != null && _colorZoneCharacter.gameObject != null)
        {
            Destroy(_colorZoneCharacter.gameObject);
        }
        GameObject colorZoneCharacter = Instantiate(chromieDefention.ColorZonePrefab) as GameObject;
        colorZoneCharacter.transform.SetParent(this.transform);
        colorZoneCharacter.transform.localPosition = Vector3.zero;
        _colorZoneCharacter = colorZoneCharacter.GetComponent<ColorZoneCharacterController>();
       
    }

    #endregion


    #region Events

    public void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (colorZone == this)
        {
            //TODO: Display collection animation 
            _colorZoneCharacter.Collected();
            
        }
    }

    #endregion
}
