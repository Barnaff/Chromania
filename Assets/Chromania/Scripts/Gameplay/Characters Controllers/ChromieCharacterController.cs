using UnityEngine;
using System.Collections;

public class ChromieCharacterController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _characterSprite;

    [SerializeField]
    private bool _isPowerup;

    #endregion


    #region Public

    public bool IsPowerup
    {
        set
        {
            _isPowerup = value;
        }
    }

    #endregion



    #region Update

    void Update()
    {
        if (_isPowerup)
        {
            if (Time.frameCount % 10 == 0)
            {
                if (_characterSprite != null)
                {
                    Color color = _characterSprite.GetComponent<SpriteRenderer>().color;
                    if (color.a == 1)
                    {
                        color.a = 0.5f;
                    }
                    else
                    {
                        color.a = 1f;
                    }
                    _characterSprite.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }

    #endregion
}
