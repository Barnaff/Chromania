using UnityEngine;
using System.Collections;

public class ColorZoneCharacterController : MonoBehaviour {

    #region Private Properties

    [SerializeField]
    private GameObject _whirlwind;

    [SerializeField]
    private GameObject _glow;

    private Animator _animator;

    #endregion


    #region Initialization

    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
    }

    #endregion


    #region Public

    public void Collected()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Collected");
        }
    }

    public void Intro()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Intro");
        }
    }

    #endregion
}
