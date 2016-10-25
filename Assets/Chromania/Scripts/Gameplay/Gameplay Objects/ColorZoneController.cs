using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private ColorZoneCharacterController _colorZoneCharacter;

    [SerializeField]
    private ChromieDefenition _chromieDefenition;

    #endregion


    #region Initiliaze

    void Start()
    {
        GameplayEventsDispatcher.Instance.OnChromieCollected += OnChromieCollectedHandler;
    }

    #endregion

    #region Public

    public void SetColorZone(ChromieDefenition chromieDefention)
    {
        GameObject colorZoneCharacter = Instantiate(chromieDefention.ColorZonePrefab) as GameObject;
        colorZoneCharacter.transform.SetParent(this.transform);
        colorZoneCharacter.transform.localPosition = Vector3.zero;
        _colorZoneCharacter = colorZoneCharacter.GetComponent<ColorZoneCharacterController>();
        _chromieDefenition = chromieDefention;
    }

    public eChromieType Color
    {
        get
        {
            return _chromieDefenition.ChromieColor;
        }
    }

    #endregion


    #region Events

    public void OnChromieCollectedHandler(ChromieController chromieController, ColorZoneController colorZone)
    {
        if (colorZone == this)
        {
            //TODO: Display collection animation 
        }
    }

    #endregion
}
