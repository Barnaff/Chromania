using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChromieDefenition
{
    #region Public Properties
    
    public int ChromieID;

    public eChromieType ChromieColor;

    public string ChromieName;

    public Color ColorValue;

    public string ColorName;

    public int CountForPowerup;

    [Header("Resources")]
    public Sprite ChromieSprite;

    public GameObject CharacterPrefab;

    public GameObject ColorZonePrefab;

    #endregion


    #region Powerups

    [Header("Powqerups")]
    public PowerupBase ActivePowerup;

    public PowerupBase PassivePowerup;

    public PowerupBase DroppedPowerup;

    #endregion
}
