using UnityEngine;
using System.Collections;

public class ChromieCharacterController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _characterSprite;

    [SerializeField]
    private bool _isPowerup;

    [SerializeField]
    private GameObject _powerupEffect;

    #endregion


    #region Public

    public bool IsPowerup
    {
        set
        {
            _isPowerup = value;
            if (_powerupEffect != null)
            {
                _powerupEffect.SetActive(_isPowerup);
            }
        }
    }

    #endregion

}
