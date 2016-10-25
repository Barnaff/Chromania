using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChromiezCacheManager : Kobapps.Singleton<ChromiezCacheManager> {

    #region Private Properties

    [SerializeField]
    private Dictionary<eChromieType, Sprite> _cachedChromiezSprites;

    private const string CHROMIE_TEXTURES_PATH = "Chromiez Sprites/";

    #endregion


    #region Initializae

    void Awake()
    {
        _cachedChromiezSprites = new Dictionary<eChromieType, Sprite>();
    }

    #endregion


    #region Public

    public Sprite GetChromieSprite(eChromieType chromieType)
    {
        Sprite chromieSprite = null;
        _cachedChromiezSprites.TryGetValue(chromieType, out chromieSprite);
        if (chromieSprite != null)
        {
            return chromieSprite;
        }

        string chromiePath = CHROMIE_TEXTURES_PATH + chromieType.ToString();
        chromieSprite = Resources.Load<Sprite>(chromiePath);

        if (chromieSprite != null)
        {
            _cachedChromiezSprites.Add(chromieType, chromieSprite);
            return chromieSprite;
        }


        return null;
    }

    #endregion


    #region Private

    

    #endregion

}
