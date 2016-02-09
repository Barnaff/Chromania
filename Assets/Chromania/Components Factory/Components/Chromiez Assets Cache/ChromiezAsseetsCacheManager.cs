using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChromiezAsseetsCacheManager : FactoryComponent, IChromiezAssetsCache
{
    #region Private Properties

    private Dictionary<eChromieType, GameObject> _loadedChromiezGameobjects;

    private Dictionary<eChromieType, Sprite> _loadedChromiezSprites;

    private const string GAMEPLAY_CHROMIEZ_PATH = "Gameplay Chromiez/";
    private const string GAMEPLAY_CHROMIEZ_SPRITES_PATH = "Chromiez Sprites/";

    #endregion


    #region Component Factory Implementation

    public override void InitComponentAtAwake()
    {
        
    }

    public override void InitComponentAtStart()
    {
        _loadedChromiezGameobjects = new Dictionary<eChromieType, GameObject>();
        _loadedChromiezSprites = new Dictionary<eChromieType, Sprite>();
    }

    #endregion


    #region IChromiezAssetsCache Implementation

    public void ClearCache()
    {
        _loadedChromiezGameobjects.Clear();
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

    public GameObject GetGameplayChromie(eChromieType colorType)
    {
        if (_loadedChromiezGameobjects.ContainsKey(colorType))
        {
            return _loadedChromiezGameobjects[colorType];
        }

        string chromiePath = GAMEPLAY_CHROMIEZ_PATH + colorType.ToString();
        GameObject chromieGameobject = Resources.Load(chromiePath) as GameObject;
        if (chromieGameobject != null)
        {
            _loadedChromiezGameobjects.Add(colorType, chromieGameobject);
            return chromieGameobject;
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
