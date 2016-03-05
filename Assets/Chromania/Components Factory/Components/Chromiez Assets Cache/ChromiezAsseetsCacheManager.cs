using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChromiezAsseetsCacheManager : FactoryComponent, IChromiezAssetsCache
{
    #region Private Properties

    private Dictionary<eChromieType, GameObject> _loadedChromiezCharacters;

    private Dictionary<eChromieType, Sprite> _loadedChromiezSprites;

    private const string GAMEPLAY_CHROMIEZ_PATH = "Chromiez Characters/";
    private const string GAMEPLAY_CHROMIEZ_SPRITES_PATH = "Chromiez Sprites/";

    #endregion


    #region Component Factory Implementation

    public override void InitComponentAtAwake()
    {
        
    }

    public override void InitComponentAtStart()
    {
        _loadedChromiezCharacters = new Dictionary<eChromieType, GameObject>();
        _loadedChromiezSprites = new Dictionary<eChromieType, Sprite>();
    }

    #endregion


    #region IChromiezAssetsCache Implementation

    public void ClearCache()
    {
        _loadedChromiezCharacters.Clear();
    }

    public Sprite GetChromieSprite(eChromieType colorType)
    {
        if (_loadedChromiezSprites.ContainsKey(colorType))
        {
            return _loadedChromiezSprites[colorType];
        }

        string chromiePath = GAMEPLAY_CHROMIEZ_SPRITES_PATH + colorType.ToString();
        Sprite chromieSprite = Resources.Load<Sprite>(chromiePath);

        if (chromieSprite != null)
        {
            _loadedChromiezSprites.Add(colorType, chromieSprite);
            return chromieSprite;
        }

        return null;
    }

    public GameObject GetChromieCharacter(eChromieType colorType)
    {
        if (_loadedChromiezCharacters.ContainsKey(colorType))
        {
            return _loadedChromiezCharacters[colorType];
        }

        string chromiePath = GAMEPLAY_CHROMIEZ_PATH + colorType.ToString();
        GameObject chromieCharacterGameobject = Resources.Load(chromiePath) as GameObject;
        if (chromieCharacterGameobject != null)
        {
            _loadedChromiezCharacters.Add(colorType, chromieCharacterGameobject);
            return chromieCharacterGameobject;
        }

        Debug.LogError("ERROR loading asset for chromie " + colorType);
        return null;
    }


    public void LoadAll()
    {
        throw new System.NotImplementedException();
    }

    #endregion;
}
